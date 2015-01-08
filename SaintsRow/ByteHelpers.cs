using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow
{
    public static class ByteHelpers
    {
        public static T ReadStruct<T>(this byte[] buffer, int offset)
        {
            return ReadStruct<T>(buffer, offset, Marshal.SizeOf(typeof(T)));
        }

        public static T ReadStruct<T>(this byte[] buffer, int offset, int length)
        {
            IntPtr ptr = Marshal.AllocHGlobal(length);
            Marshal.Copy(buffer, offset, ptr, length);
            T structInstance = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return structInstance;
        }

        public static void WriteStruct<T>(this byte[] buffer, T structToWrite, int offset)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structToWrite, ptr, true);
            Marshal.Copy(ptr, buffer, offset, size);
            Marshal.FreeHGlobal(ptr);
        }
    }
}
