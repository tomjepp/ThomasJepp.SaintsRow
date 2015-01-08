using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

using ThomasJepp.SaintsRow.Stream2;

namespace ThomasJepp.SaintsRow.Packfiles.Version04
{
    public class Packfile : IPackfile
    {
        private List<IPackfileEntry> m_Files;
        private PackfileFileData FileData;

        private Dictionary<string, Stream> m_Streams;

        public long DataOffset = 0;
        public Stream DataStream;

        public Packfile()
        {
            m_Files = new List<IPackfileEntry>();
            m_Streams = new Dictionary<string, Stream>();
        }

        public Packfile(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            FileData = stream.ReadStruct<PackfileFileData>();

            m_Files = new List<IPackfileEntry>();

            stream.Seek(GetEntryDataOffset(), SeekOrigin.Begin);

            List<PackfileEntryFileData> entryFileData = new List<PackfileEntryFileData>();

            for (int i = 0; i < FileData.IndexCount; i++)
            {
                var fileData = stream.ReadStruct<PackfileEntryFileData>();
                entryFileData.Add(fileData);
                
            }

            List<string> fileNames = new List<string>();
            for (int i = 0; i < FileData.IndexCount; i++)
            {
                var fileData = entryFileData[i];
                stream.Seek(CalculateEntryNamesOffset() + fileData.FilenameOffset, SeekOrigin.Begin);
                string name = stream.ReadAsciiNullTerminatedString();
                stream.Seek(CalculateExtensionsOffset() + fileData.ExtensionOffset, SeekOrigin.Begin);
                string extension = stream.ReadAsciiNullTerminatedString();

                m_Files.Add(new PackfileEntry(this, fileData, name + "." + extension));
            }
        }

        public void Dispose()
        {
            if (DataOffset == 0)
                DataStream.Dispose();
        }

        public List<IPackfileEntry> Files
        {
            get { return m_Files; }
        }

        public IPackfileEntry this[int i]
        {
            get { return m_Files[i]; }
        }


        public bool IsCompressed
        {
            get { return FileData.Flags.HasFlag(PackfileFlags.Compressed); }
            set { if (value) { FileData.Flags |= PackfileFlags.Compressed; } else { FileData.Flags &= ~PackfileFlags.Compressed; } }
        }

        public bool IsCondensed
        {
            get { return false; }
            set { throw new NotImplementedException(); }
        }


        public void AddFile(Stream stream, string filename)
        {
//            Files.Add(new PackfileEntry(this, new PackfileEntryFileData(), filename));
            m_Streams.Add(filename, stream);
        }

        private long GetEntryDataOffset()
        {
            return (0x180).Align(2048);
        }

        private long CalculateEntryNamesOffset()
        {
            return (GetEntryDataOffset() + FileData.IndexSize).Align(2048);
        }

        private long CalculateExtensionsOffset()
        {
            return (CalculateEntryNamesOffset() + FileData.NamesSize).Align(2048);
        }

        private long CalculateDataStartOffset()
        {
            return (CalculateExtensionsOffset() + FileData.ExtensionsSize).Align(2048);
        }

        public void Save(Stream stream)
        {
        }


        public void Update(Container container)
        {
        }
    }
}
