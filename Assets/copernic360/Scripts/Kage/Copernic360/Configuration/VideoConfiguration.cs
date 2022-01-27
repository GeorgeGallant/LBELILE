using Kage.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Kage.Copernic360.Configuration
{
    // Configuration data for a video.
    // Contains a map from key frame indices to frame
    // configuration data. Configuration data between key
    // frames should be interpolated.
    [DataContract]
    public class VideoConfiguration : ContentConfiguration, IEnumerable<KeyValuePair<ulong, VideoConfiguration.Keyframe>>
    {
        public enum KeyframeInterpolation
        {
            None = 0,
            Linear
        }

        [DataContract]
        public readonly struct Keyframe : IEquatable<Keyframe>
        {
            [DataMember]
            public readonly FrameConfiguration FrameData;

            [DataMember]
            public readonly KeyframeInterpolation Interpolation;

            public Keyframe(FrameConfiguration frameData, KeyframeInterpolation interpolation = KeyframeInterpolation.None)
            {
                FrameData = frameData;
                Interpolation = interpolation;
            }

            public Keyframe(Keyframe other)
            {
                FrameData = other.FrameData;
                Interpolation = other.Interpolation;
            }

            public static Keyframe Default
                => new Keyframe(FrameConfiguration.Default);

            internal void WriteToStream(Stream stream, int version)
            {
                var writer = new BinaryWriter(stream, Encoding.UTF8, true);
                switch (version)
                {
                case 1:
                    FrameData.WriteToStream(stream, version);
                    writer.Write((int)Interpolation);
                    break;
                default:
                    throw new ArgumentException($"Unknown serialisation version {version}.");
                }
            }

            internal static Keyframe ReadFromStream(FlexibleBinaryReader reader, int version)
            {
                switch (version)
                {
                case 1:
                    FrameConfiguration frame = FrameConfiguration.ReadFromStream(reader, version);
                    var interpolation = (KeyframeInterpolation)reader.ReadInt32();

                    return new Keyframe(frame, interpolation);
                default:
                    throw new ArgumentException($"Unknown serialisation version {version}.");
                }
            }

            #region Equality
            // C#'s custom equality pattern is a bit verbose...
            public bool Equals(Keyframe other)
            {
                return FrameData == other.FrameData
                    && Interpolation == other.Interpolation;
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public static bool operator ==(Keyframe lhs, Keyframe rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Keyframe lhs, Keyframe rhs)
                => !(lhs == rhs);

            public override int GetHashCode()
            {
                int hash = 17;
                hash = (hash * 29) + FrameData.GetHashCode();
                hash = (hash * 29) + Interpolation.GetHashCode();
                return hash;
            }
            #endregion
        }

        [DataMember]
        private SortedDictionary<ulong, Keyframe> Keyframes;

        public VideoConfiguration()
            : this(new SortedDictionary<ulong, Keyframe>())
        { }

        public VideoConfiguration(SortedDictionary<ulong, Keyframe> keyframes)
            : base()
        {
            Keyframes = keyframes;
            if (Keyframes.Count == 0)
                Keyframes[0] = Keyframe.Default;
            else if (!Keyframes.ContainsKey(0))
                Keyframes[0] = new Keyframe(Keyframes.Values.First());
        }

        // Attempts to load a video's configuration data from a stream.
        // If that fails for any reason, returns null instead.
        public static VideoConfiguration LoadFromStream(Stream stream)
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
                    var keyframes = new SortedDictionary<ulong, Keyframe>();

                    int keyframeCount = reader.ReadInt32();
                    for (int i = 0; i < keyframeCount; ++i)
                    {
                        ulong index = reader.ReadUInt64();
                        keyframes[index] = Keyframe.ReadFromStream(reader, version);
                    }

                    return new VideoConfiguration(keyframes);
                default:
                    throw new ArgumentException($"Unknown serialisation version {version}.");
                }
            }
        }

        // Attempts to load an image's configuration data from a file.
        // If that fails for any reason, returns null instead.
        internal static VideoConfiguration LoadFromJsonFile(string filePath)
            => LoadFromJsonFile<VideoConfiguration>(filePath);

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

                // Serialise dictionary
                writer.Write(Keyframes.Count);
                foreach (var (index, frame) in Keyframes.Select(kvp => (kvp.Key, kvp.Value)))
                {
                    writer.Write(index);
                    frame.WriteToStream(stream, version);
                }
            }
        }

        public bool IsKeyframe(ulong i)
        {
            return Keyframes.ContainsKey(i);
        }

        public void DeleteKeyframe(ulong i)
        {
            // Can't delete first keyframe
            if (i == 0)
                return;

            Keyframes.Remove(i);
        }

        public Keyframe GetPreviousKeyframe(ulong i)
        {
            Keyframe previousKeyframe = Keyframes[0];

            foreach (var (index, frame) in Keyframes.Select(kvp => (kvp.Key, kvp.Value)))
            {
                if (index > i)
                    break;

                previousKeyframe = frame;
            }

            return previousKeyframe;
        }

        public Keyframe? GetNextKeyframe(ulong i)
        {
            foreach (var (index, frame) in Keyframes.Select(kvp => (kvp.Key, kvp.Value)))
            {
                if (index > i)
                    return frame;
            }

            return null;
        }

        public void SetKeyframe(ulong index, Keyframe keyframe)
        {
            Keyframes[index] = keyframe;
        }

        public FrameConfiguration GetFrameData(ulong i)
        {
            ulong previousIndex = 0;
            ulong? nextIndex = null;

            foreach (var index in Keyframes.Keys)
            {
                if (index > i)
                {
                    nextIndex = index;
                    break;
                }

                previousIndex = index;
            }

            FrameConfiguration previousKeyframe = Keyframes[previousIndex].FrameData;

            if (!nextIndex.HasValue || Keyframes[previousIndex].Interpolation == KeyframeInterpolation.None)
                return previousKeyframe;

            FrameConfiguration nextKeyframe = Keyframes[nextIndex.Value].FrameData;

            float fraction = (float)(i - previousIndex) / (nextIndex.Value - previousIndex);
            return new FrameConfiguration(
                Mathf.Lerp(previousKeyframe.Height, nextKeyframe.Height, fraction),
                Mathf.Lerp(previousKeyframe.Radius, nextKeyframe.Radius, fraction),
                Mathf.Lerp(previousKeyframe.Deformation, nextKeyframe.Deformation, fraction),
                Mathf.Lerp(previousKeyframe.MovementRange, nextKeyframe.MovementRange, fraction)
            );
        }

        public IEnumerator<KeyValuePair<ulong, Keyframe>> GetEnumerator()
        {
            return Keyframes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Keyframes.GetEnumerator();
        }
    }
}
