using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Strings
{
    public class StringFile
    {
        private StringHeader Header;
        private List<Dictionary<UInt32, string>> Buckets = new List<Dictionary<uint, string>>();

        public UInt32 ID
        {
            get
            {
                return Header.ID;
            }
            set
            {
                Header.ID = value;
            }
        }

        public UInt16 Version
        {
            get
            {
                return Header.Version;
            }
            set
            {
                Header.Version = value;
            }
        }

        public StringFile()
        {
            Header = new StringHeader();
            Header.ID = 0xA84C7F73;
            Header.Version = 0x0001;
        }

        public StringFile(Stream stream)
        {
            Header = stream.ReadStruct<StringHeader>();
            for (int i = 0; i < Header.BucketCount; i++)
            {
                // Seek to the start of our new bucket
                stream.Seek(Marshal.SizeOf(typeof(StringHeader)) + (i * Marshal.SizeOf(typeof(StringBucket))), SeekOrigin.Begin);
                StringBucket bucket = stream.ReadStruct<StringBucket>();

                Console.WriteLine("{0} strings at address {1:X8}", bucket.StringCount, bucket.StringOffset);

                
                Dictionary<UInt32, string> bucketData = new Dictionary<uint, string>();
                for (int j = 0; j < bucket.StringCount; j++)
                {
                    stream.Seek(bucket.StringOffset + (sizeof(UInt32) * j), SeekOrigin.Begin);
                    UInt32 stringOffset = stream.ReadUInt32();
                    
                    stream.Seek(stringOffset, SeekOrigin.Begin);
                    Console.WriteLine(stream.Position);
                    UInt32 stringHash = stream.ReadUInt32();
                    Console.WriteLine(stream.Position);
                    int length = 0;
                    while (true)
                    {
                        UInt16 charValue = stream.ReadUInt16();
                        if (charValue == 0x0000)
                            break;
                        length++;
                    }

                    stream.Seek(stringOffset+4, SeekOrigin.Begin);
                    Console.WriteLine(stream.Position);
                    byte[] buffer = new byte[length*2];
                    stream.Read(buffer, 0, buffer.Length);
                    string text = Encoding.Unicode.GetString(buffer);

                    Console.WriteLine("String data offset: {2:X8} gives: {0:X8}: {1}", stringHash, text, stringOffset+4);

                    bucketData.Add(stringHash, text);
                }
                Buckets.Add(bucketData);
            }
        }
    }
}
;