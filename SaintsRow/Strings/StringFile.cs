using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ThomasJepp.SaintsRow.Strings
{
    public class StringFile
    {
        private StringHeader Header;
        private List<Dictionary<UInt32, string>> Buckets = new List<Dictionary<UInt32, string>>();

        public void AddString(string key, string text)
        {
            UInt32 hash = Hashes.CrcVolition(key);
            AddString(hash, text);
        }

        public void AddString(UInt32 hash, string text)
        {
            UInt32 mask = (UInt32)(Buckets.Count - 1);
            UInt32 bucketIdx = (UInt32)(hash & mask);
            Buckets[(int)bucketIdx].Add(hash, text);
        }

        public string GetString(string key)
        {
            UInt32 hash = Hashes.CrcVolition(key);
            return GetString(hash);
        }

        public string GetString(UInt32 hash)
        {
            UInt32 mask = (UInt32)(Buckets.Count - 1);
            UInt32 bucketIdx = (UInt32)(hash & mask);
            return Buckets[(int)bucketIdx][hash];
        }

        public List<UInt32> GetHashes()
        {
            IEnumerable<UInt32> hashes = new List<UInt32>();
            
            foreach (var bucket in Buckets)
            {
                hashes = hashes.Concat(bucket.Keys);
            }

            return hashes.ToList();
        }

        public bool FileIsSaintsRow2
        {
            get;
            set;
        }

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

        public StringFile(UInt16 bucketCount)
            : this(bucketCount, false)
        {
        }

        public StringFile(UInt16 bucketCount, bool sr2)
        {
            FileIsSaintsRow2 = sr2;
            Header = new StringHeader();
            Header.BucketCount = bucketCount;
            Header.ID = 0xA84C7F73;
            Header.Version = 0x0001;
            for (int i = 0; i < bucketCount; i++)
            {
                Buckets.Add(new Dictionary<UInt32, string>());
            }
        }

        public StringFile(Stream stream)
            : this(stream, false)
        {
        }

        public StringFile(Stream stream, bool sr2)
        {
            FileIsSaintsRow2 = sr2;
            Header = stream.ReadStruct<StringHeader>();

            for (int i = 0; i < Header.BucketCount; i++)
            {
                // Seek to the start of our new bucket
                stream.Seek(Marshal.SizeOf(typeof(StringHeader)) + (i * Marshal.SizeOf(typeof(StringBucket))), SeekOrigin.Begin);
                StringBucket bucket = stream.ReadStruct<StringBucket>();
                                
                Dictionary<UInt32, string> bucketData = new Dictionary<uint, string>();
                for (int j = 0; j < bucket.StringCount; j++)
                {
                    stream.Seek(bucket.StringOffset + (sizeof(UInt32) * j), SeekOrigin.Begin);
                    UInt32 stringOffset = stream.ReadUInt32();
                    
                    stream.Seek(stringOffset, SeekOrigin.Begin);
                    UInt32 stringHash = stream.ReadUInt32();
                    if (FileIsSaintsRow2)
                        stringHash = stringHash.Swap();

                    int length = 0;
                    while (true)
                    {
                        UInt16 charValue = stream.ReadUInt16();
                        if (charValue == 0x0000)
                            break;
                        length++;
                    }

                    stream.Seek(stringOffset+4, SeekOrigin.Begin);
                    byte[] buffer = new byte[length*2];
                    stream.Read(buffer, 0, buffer.Length);

                    string text = FileIsSaintsRow2 ? Encoding.BigEndianUnicode.GetString(buffer) : Encoding.Unicode.GetString(buffer);

                    bucketData.Add(stringHash, text);
                }
                Buckets.Add(bucketData);
            }
        }

        public void Save(Stream stream)
        {
            int total = 0;
            foreach (var bucket in Buckets)
            {
                total += bucket.Count;
            }
            Header.StringCount = (UInt32)total;

            stream.WriteStruct<StringHeader>(Header);
            int nextBucketData = Buckets.Count * Marshal.SizeOf(typeof(StringBucket)) + Marshal.SizeOf(typeof(StringHeader));
            int nextStringPos = Buckets.Count * Marshal.SizeOf(typeof(StringBucket)) + Marshal.SizeOf(typeof(StringHeader)) + Marshal.SizeOf(typeof(UInt32)) * total;
            foreach (var bucket in Buckets)
            {
                long bucketPos = stream.Position;
                StringBucket strBucket = new StringBucket();
                strBucket.StringCount = (UInt32)bucket.Count;
                strBucket.StringOffset = (UInt32)nextBucketData;

                foreach (var pair in bucket)
                {
                    stream.Seek(nextBucketData, SeekOrigin.Begin);
                    stream.WriteUInt32((UInt32)nextStringPos);
                    nextBucketData = (int)stream.Position;
                    stream.Seek(nextStringPos, SeekOrigin.Begin);
                    UInt32 hash = FileIsSaintsRow2 ? pair.Key.Swap() : pair.Key;
                    stream.WriteUInt32(hash);

                    byte[] text = FileIsSaintsRow2 ? Encoding.BigEndianUnicode.GetBytes(pair.Value) : Encoding.Unicode.GetBytes(pair.Value);
                    stream.Write(text, 0, text.Length);
                    stream.WriteByte(0);
                    stream.WriteByte(0);
                    nextStringPos = (int)stream.Position;
                }

                stream.Seek(bucketPos, SeekOrigin.Begin);
                stream.WriteStruct(strBucket);
            }
        }
    }
}
