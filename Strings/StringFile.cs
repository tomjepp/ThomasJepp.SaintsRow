using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ThomasJepp.SaintsRow.GameInstances;
using ThomasJepp.SaintsRow.Localization;

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

            if (ContainsKey(hash))
                return Buckets[(int)bucketIdx][hash];
            else
                return null;
        }

        public bool ContainsKey(string key)
        {
            UInt32 hash = Hashes.CrcVolition(key);
            return ContainsKey(hash);
        }

        public bool ContainsKey(UInt32 hash)
        {
            UInt32 mask = (UInt32)(Buckets.Count - 1);
            UInt32 bucketIdx = (UInt32)(hash & mask);
            return (Buckets[(int)bucketIdx].ContainsKey(hash));
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
            get
            {
                return GameInstance.Game == GameSteamID.SaintsRow2;
            }
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

        public Language Language { get; set; }
        public IGameInstance GameInstance { get; set; }

        public StringFile(UInt16 bucketCount, Language language, IGameInstance instance)
        {
            GameInstance = instance;
            Language = language;
            Header = new StringHeader();
            Header.BucketCount = bucketCount;
            Header.ID = 0xA84C7F73;
            Header.Version = 0x0001;
            for (int i = 0; i < bucketCount; i++)
            {
                Buckets.Add(new Dictionary<UInt32, string>());
            }
        }

        public StringFile(Stream stream, Language language, IGameInstance instance)
        {
            GameInstance = instance;
            Language = language;
            Header = stream.ReadStruct<StringHeader>();

            var map = LanguageUtility.GetDecodeCharMap(GameInstance, Language);

            StringBuilder sb = new StringBuilder();

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

                    sb.Clear();
                    
                    int length = 0;
                    while (true)
                    {
                        UInt16 charValue = stream.ReadUInt16();

                        if (charValue == 0x0000)
                            break;

                        if (FileIsSaintsRow2)
                            charValue = charValue.Swap();

                        char src = (char)charValue;

                        char value = src;

                        if (map.ContainsKey(src))
                            value = map[src];

                        sb.Append(value);

                        length++;
                    }

                    string text = sb.ToString();
                    bucketData.Add(stringHash, text);
                }
                Buckets.Add(bucketData);
            }
        }

        public void Save(Stream stream)
        {
            var map = LanguageUtility.GetEncodeCharMap(GameInstance, Language);

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

                    using (MemoryStream ms = new MemoryStream())
                    {
                        for (int i = 0; i < pair.Value.Length; i++)
                        {
                            char src = pair.Value[i];
                            char value = src;
                            if (map.ContainsKey(src))
                                value = map[src];

                            UInt16 charValue = (UInt16)value;
                            if (FileIsSaintsRow2)
                                charValue = charValue.Swap();

                            ms.WriteUInt16(charValue);
                        }
                        ms.WriteInt16(0);
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.CopyTo(stream);
                    }

                    nextStringPos = (int)stream.Position;
                }

                stream.Seek(bucketPos, SeekOrigin.Begin);
                stream.WriteStruct(strBucket);
            }
        }
    }
}
