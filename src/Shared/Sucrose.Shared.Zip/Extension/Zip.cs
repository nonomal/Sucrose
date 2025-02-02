﻿using System.IO;
using System.IO.Compression;
using SSDECT = Sucrose.Shared.Dependency.Enum.CompatibilityType;
using SSSHF = Sucrose.Shared.Space.Helper.Filing;
using SSZHZ = Sucrose.Shared.Zip.Helper.Zip;

namespace Sucrose.Shared.Zip.Extension
{
    internal static class Zip
    {
        public static SSDECT Extract(string Archive, string Destination)
        {
            try
            {
#if NET48_OR_GREATER
                ZipFile.ExtractToDirectory(Archive, Destination);
#else
                ZipFile.ExtractToDirectory(Archive, Destination, true);
#endif

                return SSDECT.Pass;
            }
            catch
            {
                return SSDECT.UnforeseenConsequences;
            }
        }

        public static SSDECT Compress(string Source, string Destination)
        {
            try
            {
                if (File.Exists(Destination))
                {
                    SSSHF.Delete(Destination);
                }

#if NET48_OR_GREATER
                ZipFile.CreateFromDirectory(Source, Destination, CompressionLevel.Fastest, false);
#else
                ZipFile.CreateFromDirectory(Source, Destination, CompressionLevel.SmallestSize, false);
#endif

                return SSDECT.Pass;
            }
            catch
            {
                return SSDECT.UnforeseenConsequences;
            }
        }

        public static SSDECT Compress(string[] Sources, string Destination)
        {
            try
            {
                using FileStream ZipFileStream = new(Destination, FileMode.Create);
                using ZipArchive Archive = new(ZipFileStream, ZipArchiveMode.Create);

                foreach (string Source in Sources)
                {
                    string[] Files = Directory.GetFiles(Source, "*", SearchOption.TopDirectoryOnly);

                    foreach (string Record in Files)
                    {
                        string EntryName = SSZHZ.EntryName(Record, Source);

#if NET48_OR_GREATER
                        ZipArchiveEntry Entry = Archive.CreateEntry(EntryName, CompressionLevel.Fastest);
#else
                        ZipArchiveEntry Entry = Archive.CreateEntry(EntryName, CompressionLevel.SmallestSize);
#endif

                        using Stream EntryStream = Entry.Open();
                        using FileStream FileStream = File.OpenRead(Record);

                        FileStream.CopyTo(EntryStream);
                    }
                }

                return SSDECT.Pass;
            }
            catch
            {
                return SSDECT.UnforeseenConsequences;
            }
        }

        public static SSDECT Compress(string[] Sources, string[] Excludes, string Destination)
        {
            try
            {
                using FileStream ZipFileStream = new(Destination, FileMode.Create);
                using ZipArchive Archive = new(ZipFileStream, ZipArchiveMode.Create);

                foreach (string Source in Sources)
                {
                    if (!Directory.Exists(Source))
                    {
                        Directory.CreateDirectory(Source);
                    }

                    string[] Files = Directory.GetFiles(Source, "*", SearchOption.TopDirectoryOnly);

                    foreach (string Record in Files)
                    {
                        if (!Excludes.Contains(Record))
                        {
                            string EntryName = SSZHZ.EntryName(Record, Source);

#if NET48_OR_GREATER
                            ZipArchiveEntry Entry = Archive.CreateEntry(EntryName, CompressionLevel.Fastest);
#else
                            ZipArchiveEntry Entry = Archive.CreateEntry(EntryName, CompressionLevel.SmallestSize);
#endif

                            using Stream EntryStream = Entry.Open();
                            using FileStream FileStream = File.OpenRead(Record);

                            FileStream.CopyTo(EntryStream);
                        }
                    }
                }

                return SSDECT.Pass;
            }
            catch
            {
                return SSDECT.UnforeseenConsequences;
            }
        }
    }
}