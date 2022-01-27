using Kage.Utils;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Kage.Copernic360.Configuration
{
    // Configuration data for a single frame, which may either
    // be a static image, or a frame of a longer video.
    // Contains the parameters of the viewing sphere for that
    // frame.
    [DataContract]
    public readonly struct FrameConfiguration : IEquatable<FrameConfiguration>
    {
        [DataMember]
        public readonly float Height;

        [DataMember]
        public readonly float Radius;

        [DataMember]
        public readonly float Deformation;

        public float FloorRadius
            => Mathf.Lerp(Radius, Height, Deformation);

        [DataMember]
        public readonly float MovementRange;

        public FrameConfiguration(float height, float radius, float deformation, float movementRange)
        {
            Height = Mathf.Max(height, 0f);
            Radius = Mathf.Max(radius, 0f);
            Deformation = Mathf.Clamp01(deformation);
            MovementRange = Mathf.Clamp01(movementRange);
        }

        public FrameConfiguration(FrameConfiguration other)
        {
            this = other;
        }

        public static FrameConfiguration Default
            => new FrameConfiguration(1.6f, 2.5f, 0f, 0.5f);

        internal void WriteToStream(Stream stream, int version)
        {
            var writer = new BinaryWriter(stream, Encoding.UTF8, true);
            switch (version)
            {
            case 1:
                writer.Write(Height);
                writer.Write(Radius);
                writer.Write(Deformation);
                writer.Write(MovementRange);
                break;
            default:
                throw new ArgumentException($"Unknown serialisation version {version}.");
            }
        }

        internal static FrameConfiguration ReadFromStream(FlexibleBinaryReader reader, int version)
        {
            switch (version)
            {
            case 1:
                float height = reader.ReadSingle();
                float radius = reader.ReadSingle();
                float deformation = reader.ReadSingle();
                float movementRange = reader.ReadSingle();

                return new FrameConfiguration(height, radius, deformation, movementRange);
            default:
                throw new ArgumentException($"Unknown serialisation version {version}.");
            }
        }

        #region Equality
        // C#'s custom equality pattern is a bit verbose...
        public bool Equals(FrameConfiguration other)
        {
            return Height == other.Height
                && Radius == other.Radius
                && Deformation == other.Deformation
                && MovementRange == other.MovementRange;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(FrameConfiguration lhs, FrameConfiguration rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(FrameConfiguration lhs, FrameConfiguration rhs)
            => !(lhs == rhs);

        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 29) + Height.GetHashCode();
            hash = (hash * 29) + Radius.GetHashCode();
            hash = (hash * 29) + Deformation.GetHashCode();
            hash = (hash * 29) + MovementRange.GetHashCode();
            return hash;
        }
        #endregion
    }
}