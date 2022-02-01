using Kage.Utils;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace Kage.Copernic360.Configuration
{
    // Configuration data for a static image.
    [DataContract]
    public class ImageConfiguration : ContentConfiguration
    {
        [DataMember]
        public FrameConfiguration ImageData { get; set; }

        public ImageConfiguration()
            : this(FrameConfiguration.Default)
        { }

        public ImageConfiguration(FrameConfiguration imageData)
            : base()
        {
            ImageData = imageData;
        }

        // Attempts to load an image's configuration data from a stream.
        // If that fails for any reason, returns null instead.
        public static ImageConfiguration LoadFromStream(Stream stream)
        {
            using (var reader = new FlexibleBinaryReader(stream))
            {
                // Byte-order marker
                bool isLittleEndian = reader.ReadUInt16() < 256;
                reader.SetEndianness(isLittleEndian);
                // Serialisation version
                int version = reader.ReadInt32();
                switch (version)
                {
                case 1:
                    FrameConfiguration frame = FrameConfiguration.ReadFromStream(reader, version);
                    return new ImageConfiguration(frame);
                default:
                    throw new ArgumentException($"Unknown serialisation version {version}.");
                }
            }
        }

        // Attempts to load an image's configuration data from a file.
        // If that fails for any reason, returns null instead.
        internal static ImageConfiguration LoadFromJsonFile(string filePath)
            => LoadFromJsonFile<ImageConfiguration>(filePath);

        // Saves this configuration to the given stream.
        public void SaveToStream(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                // Byte-order marker
                writer.Write((ushort)1);
                // Serialisation version
                int version = 1;
                writer.Write(version);
                ImageData.WriteToStream(stream, version);
            }
        }
    }
}