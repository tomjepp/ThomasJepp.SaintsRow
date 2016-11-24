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
        private Dictionary<UInt32, string> Strings = new Dictionary<uint, string>();

        public void AddString(string key, string text)
        {
            UInt32 hash = Hashes.CrcVolition(key);
            AddString(hash, text);
        }

        public void AddString(UInt32 hash, string text)
        {
            if (Strings.ContainsKey(hash))
                Strings[hash] = text;
            else
                Strings.Add(hash, text);
        }

        public string GetString(string key)
        {
            UInt32 hash = Hashes.CrcVolition(key);
            return GetString(hash);
        }

        public string GetString(UInt32 hash)
        {
            if (Strings.ContainsKey(hash))
                return Strings[hash];
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
            return Strings.ContainsKey(hash);
        }

        public List<UInt32> GetHashes()
        {
            return Strings.Keys.ToList();
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

        public StringFile(Language language, IGameInstance instance)
        {
            GameInstance = instance;
            Language = language;
            Header = new StringHeader();
            Header.ID = 0xA84C7F73;
            Header.Version = 0x0001;
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
                    Strings.Add(stringHash, text);
                }
            }
        }

        public void Save(Stream stream)
        {
            var map = LanguageUtility.GetEncodeCharMap(GameInstance, Language);

            UInt16 bucketCount = (UInt16)(Strings.Count / 5);
            if (bucketCount < 32)
                bucketCount = 32;
            else if (bucketCount < 64)
                bucketCount = 64;
            else if (bucketCount < 128)
                bucketCount = 128;
            else if (bucketCount < 256)
                bucketCount = 256;
            else if (bucketCount < 512)
                bucketCount = 512;
            else
                bucketCount = 1024;

            Dictionary<uint, string>[] buckets = new Dictionary<uint, string>[bucketCount];
            for (int i = 0; i < bucketCount; i++)
            {
                buckets[bucketCount] = new Dictionary<uint, string>();
            }

            foreach (var pair in Strings)
            {
                uint hash = pair.Key;
                string text = pair.Value;

                UInt32 mask = (UInt32)(buckets.Length - 1);
                UInt32 bucketIdx = (UInt32)(hash & mask);
                buckets[(int)bucketIdx].Add(hash, text);
            }

            Header.StringCount = (UInt32)Strings.Count;

            stream.WriteStruct<StringHeader>(Header);
            int nextBucketData = buckets.Length * Marshal.SizeOf(typeof(StringBucket)) + Marshal.SizeOf(typeof(StringHeader));
            int nextStringPos = buckets.Length * Marshal.SizeOf(typeof(StringBucket)) + Marshal.SizeOf(typeof(StringHeader)) + Marshal.SizeOf(typeof(UInt32)) * Strings.Count;
            foreach (var bucket in buckets)
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
