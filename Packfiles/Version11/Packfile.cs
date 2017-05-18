using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

using ThomasJepp.SaintsRow.AssetAssembler;

namespace ThomasJepp.SaintsRow.Packfiles.Version11
{
    public class Packfile : IPackfile
    {
        private List<IPackfileEntry> m_Files;
        private PackfileFileData FileData;

        private Dictionary<string, Stream> m_Streams;

        public long DataOffset = 0;
        public Stream DataStream;
        public bool IsStr2;

        public Packfile(bool isStr2)
        {
            IsStr2 = isStr2;
            m_Files = new List<IPackfileEntry>();
            m_Streams = new Dictionary<string, Stream>();
        }

        public Packfile(Stream stream, bool isStr2)
        {
            IsStr2 = isStr2;
            stream.Seek(0, SeekOrigin.Begin);
            FileData = stream.ReadStruct<PackfileFileData>();

            m_Files = new List<IPackfileEntry>();
            m_Streams = new Dictionary<string, Stream>();

            List<PackfileEntryFileData> entryFileData = new List<PackfileEntryFileData>();
            using (MemoryStream dirStream = stream.ReadMemoryStream(FileData.DirSize))
            {
                for (int i = 0; i < FileData.NumFiles; i++)
                {
                    PackfileEntryFileData data = dirStream.ReadStruct<PackfileEntryFileData>();
                    entryFileData.Add(data);
                }
            }

            // get names
            using (MemoryStream nameStream = stream.ReadMemoryStream(FileData.FilenameSize))
            {
                for (int i = 0; i < FileData.NumFiles; i++)
                {
                    PackfileEntryFileData data = entryFileData[i];
                    nameStream.Seek((long)data.Filepath, SeekOrigin.Begin);
                    string path = nameStream.ReadAsciiNullTerminatedString();
                    nameStream.Seek((long)data.FilenameOffset, SeekOrigin.Begin);
                    string name = nameStream.ReadAsciiNullTerminatedString();

                    m_Files.Add(new PackfileEntry(this, data, name, path));
                }
            }

            // get data stream
            DataStream = stream;
            DataOffset = (long)FileData.DataOffset;
        }

        public void Dispose()
        {
            if (DataOffset == 0)
            {
                if (DataStream != null)
                {
                    DataStream.Dispose();
                }
            }
        }

        public List<IPackfileEntry> Files
        {
            get { return m_Files; }
        }

        public IPackfileEntry this[int i]
        {
            get { return m_Files[i]; }
        }

        public IPackfileEntry this[string s]
        {
            get
            {
                string lowercase = s.ToLowerInvariant();
                foreach (IPackfileEntry entry in m_Files)
                {
                    if (entry.Name.ToLowerInvariant() == lowercase)
                        return entry;
                }

                return null;
            }
        }

        public int Version
        {
            get
            {
                return 0x11;
            }
        }

        public bool IsCompressed
        {
            get { return FileData.Flags.HasFlag(PackfileFlags.Compressed); }
            set { if (value) { FileData.Flags |= PackfileFlags.Compressed; } else { FileData.Flags &= ~PackfileFlags.Compressed; } }
        }

        public bool IsCondensed
        {
            get { return FileData.Flags.HasFlag(PackfileFlags.Condensed); }
            set { if (value) { FileData.Flags |= PackfileFlags.Condensed; } else { FileData.Flags &= ~PackfileFlags.Condensed; } }
        }


        public void AddFile(Stream stream, string filename)
        {
            string name = Path.GetFileName(filename);
            string path = Path.GetDirectoryName(filename);

            Files.Add(new PackfileEntry(this, new PackfileEntryFileData(), name, path, true));
            m_Streams.Add(filename, stream);
        }

        public void RemoveFile(IPackfileEntry entry)
        {
            Files.Remove(entry);
            if (m_Streams != null && m_Streams.ContainsKey(entry.Name))
                m_Streams.Remove(entry.Name);
        }

        public bool ContainsFile(string filename)
        {
            foreach (PackfileEntry entry in Files)
            {
                if (entry.Name == filename)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveFile(string filename)
        {
            PackfileEntry entry = null;

            foreach (PackfileEntry e in Files)
            {
                if (e.Name == filename)
                {
                    entry = e;
                    break;
                }
            }

            if (entry == null)
                return;

            RemoveFile(entry);
        }

        private long GetEntryDataOffset()
        {
            return System.Runtime.InteropServices.Marshal.SizeOf(typeof(PackfileFileData));
        }

        private long CalculateEntryNamesOffset()
        {
            return GetEntryDataOffset() + (System.Runtime.InteropServices.Marshal.SizeOf(typeof(PackfileEntryFileData)) * Files.Count);
            // needs alignment!
        }

        private long CalculateDataStartOffset()
        {
            long offset = CalculateEntryNamesOffset();

            foreach (IPackfileEntry entry in Files)
            {
                offset = offset.Align(2);
                offset += entry.Name.Length;
                offset += 2;
                offset = offset.Align(2);
            }

            // needs alignment

            return offset;
        }

        public void Save(Stream stream)
        {
            throw new NotImplementedException();
        }


        public void Update(IContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
