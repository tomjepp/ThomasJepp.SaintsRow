using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasJepp.SaintsRow
{
    public class Bitstream
    {
        private byte[] BackingStore;
        public int Length
        {
            get
            {
                return (BackingStore.Length * 8);
            }
        }

        public int Position;

        public Bitstream(byte[] bytes)
        {
            BackingStore = bytes;
        }

        public bool GetBit(byte[] bytes, int offset)
        {
            int byteIndex = offset >> 3;
            int bitIndex = offset % 8;

            byte b = bytes[byteIndex];
            byte mask = (byte)(1 << bitIndex);

            if ((b & mask) != 0)
                return true;
            else
                return false;
        }

        public bool ReadBit()
        {
            if (Position >= Length)
            {
                throw new IndexOutOfRangeException();
            }

            bool bit = GetBit(BackingStore, Position);
            Position++;
            return bit;
        }

        public void WriteBit(bool bit)
        {
            if (Position >= Length)
            {
                throw new IndexOutOfRangeException();
            }

            int byteIndex = Position >> 3;
            int bitIndex = Position % 8;

            byte b = BackingStore[byteIndex];
            byte mask = (byte)(1 << bitIndex);

            if (bit)
            {
                BackingStore[byteIndex] |= mask;
            }
            else
            {
                BackingStore[byteIndex] &= (byte)(~mask);
            }

            Position++;
        }

        private byte[] ReadBits(int bits)
        {
            byte[] bytes = new byte[(int)Math.Ceiling((decimal)bits / 8m)];

            for (int i = 0; i < bits; i++)
            {
                int bytePointer = i >> 3;
                int bitIndex = i % 8;

                if (ReadBit())
                {
                    byte mask = (byte)(1 << bitIndex);
                    bytes[bytePointer] |= mask;
                }
            }

            return bytes;
        }

        public void WriteBits(byte[] bytes, int bits)
        {
            for (int i = 0; i < bits; i++)
            {
                bool bit = GetBit(bytes, i);

                WriteBit(bit);
            }
        }

        public byte ReadU8(int bits)
        {
            byte[] bytes = ReadBits(bits);
            return bytes[0];
        }

        public void WriteU8(byte val, int bits)
        {
            byte[] bytes = new byte[1] { val };
            WriteBits(bytes, bits);
        }

        public ushort ReadU16(int bits)
        {
            byte[] bytes = ReadBits(bits);
            Array.Resize<byte>(ref bytes, 2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public void WriteU16(ushort val, int bits)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            WriteBits(bytes, bits);
        }

        public uint ReadU32(int bits)
        {
            byte[] bytes = ReadBits(bits);
            Array.Resize<byte>(ref bytes, 4);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public void WriteU32(uint val, int bits)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            WriteBits(bytes, bits);
        }
    }
}
