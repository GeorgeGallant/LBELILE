using System;
using System.IO;

namespace Kage.Utils
{
    public class FlexibleBinaryReader : IDisposable
    {
        private bool disposed = false;
        private BinaryReader binaryReader;
        private bool reverse;

        public FlexibleBinaryReader(Stream stream, bool littleEndian = true)
        {
            binaryReader = new BinaryReader(stream);
            SetEndianness(littleEndian);
        }

        public void SetEndianness(bool isLittleEndian)
        {
            reverse = isLittleEndian ^ BitConverter.IsLittleEndian;
        }

        public ushort ReadUInt16()
        {
            if (reverse)
            {
                byte[] data = binaryReader.ReadBytes(2);
                Array.Reverse(data);
                return BitConverter.ToUInt16(data, 0);
            }
            else
                return binaryReader.ReadUInt16();
        }

        public int ReadInt32()
        {
            if (reverse)
            {
                byte[] data = binaryReader.ReadBytes(4);
                Array.Reverse(data);
                return BitConverter.ToInt32(data, 0);
            }
            else
                return binaryReader.ReadInt32();
        }

        public ulong ReadUInt64()
        {
            if (reverse)
            {
                byte[] data = binaryReader.ReadBytes(8);
                Array.Reverse(data);
                return BitConverter.ToUInt64(data, 0);
            }
            else
                return binaryReader.ReadUInt64();
        }

        public float ReadSingle()
        {
            if (reverse)
            {
                byte[] data = binaryReader.ReadBytes(4);
                Array.Reverse(data);
                return BitConverter.ToSingle(data, 0);
            }
            else
                return binaryReader.ReadSingle();
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                binaryReader.Dispose();
            }

            disposed = true;
        }

        ~FlexibleBinaryReader()
        {
            Dispose(false);
        }
    }
}
