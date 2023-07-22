﻿using System.IO;
using System.IO.Compression;
using SSDECT = Sucrose.Shared.Dependency.Enum.CompatibilityType;
using SSDEWT = Sucrose.Shared.Dependency.Enum.WallpaperType;
using SEAET = Skylark.Enum.AppExtensionType;
using SEVET = Skylark.Enum.VideoExtensionType;
using SEWET = Skylark.Enum.WebExtensionType;
using SHV = Skylark.Helper.Versionly;
using SMR = Sucrose.Memory.Readonly;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSTHP = Sucrose.Shared.Theme.Helper.Properties;
using SSTHV = Sucrose.Shared.Theme.Helper.Various;

namespace Sucrose.Shared.Theme.Helper
{
    internal static class Zip
    {
        public static SSDECT Extract(string Archive, string Destination)
        {
            try
            {
                // ZIP dosyasını açma
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
                // Eğer ZIP dosyası varsa silme
                if (File.Exists(Destination))
                {
                    File.Delete(Destination);
                }

                // ZIP dosyası oluşturma
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

        public static SSDECT Check(string Archive)
        {
            try
            {
                // Seçilen dosya var mı?
                if (!File.Exists(Archive))
                {
                    return SSDECT.NotFound;
                }

                // Seçilen dosya .zip uzantılı değil mi?
                if (Path.GetExtension(Archive) != ".zip")
                {
                    return SSDECT.Extension;
                }

                // Seçilen dosya gerçekten ZIP dosyası mı?
                if (!CheckArchive(Archive))
                {
                    return SSDECT.ZipType;
                }

                // Arşivde SucroseInfo.json dosyası var mı?
                if (!CheckFile(Archive, SMR.SucroseInfo))
                {
                    return SSDECT.InfoFile;
                }

                // Arşivdeki SucroseInfo.json dosyasını okuma
                SSTHI Info = SSTHI.FromJson(ReadFile(Archive, SMR.SucroseInfo));

                // Info içindeki Thumbnail dosyası var mı?
                if (!CheckFile(Archive, Info.Thumbnail))
                {
                    return SSDECT.Thumbnail;
                }

                // Info içindeki Preview dosyası var mı?
                if (!CheckFile(Archive, Info.Preview))
                {
                    return SSDECT.Preview;
                }

                // Info içindeki AppVersion sürümü bu uygulamanın düşük mü?
                if (Info.AppVersion.CompareTo(SHV.Executing()) > 0)
                {
                    return SSDECT.AppVersion;
                }

                // Info içindeki Type değeri bu uygulamanın Type enum değerinden büyük mü?
                if ((int)Info.Type >= Enum.GetValues(typeof(SSDEWT)).Length)
                {
                    return SSDECT.Type;
                }

                // Info içindeki Type değerine göre dosya veya url kontrolü
                if (Info.Type == SSDEWT.Web)
                {
                    if (!CheckFile(Archive, Info.Source))
                    {
                        return SSDECT.Source;
                    }
                    else if (!CheckWebExtension(Info.Source))
                    {
                        return SSDECT.InvalidExtension;
                    }
                }
                else if (Info.Type == SSDEWT.Url && !SSTHV.IsUrl(Info.Source))
                {
                    return SSDECT.InvalidUrl;
                }
                else if (Info.Type == SSDEWT.Gif)
                {
                    if (!SSTHV.IsUrl(Info.Source) && !CheckFile(Archive, Info.Source))
                    {
                        return SSDECT.Source;
                    }
                    else if (!CheckGifExtension(Info.Source))
                    {
                        return SSDECT.InvalidExtension;
                    }
                }
                else if (Info.Type == SSDEWT.Video)
                {
                    if (!SSTHV.IsUrl(Info.Source) && !CheckFile(Archive, Info.Source))
                    {
                        return SSDECT.Source;
                    }
                    else if (!CheckVideoExtension(Info.Source))
                    {
                        return SSDECT.InvalidExtension;
                    }
                }
                else if (Info.Type == SSDEWT.YouTube && !SSTHV.IsYouTube(Info.Source) && !SSTHV.IsYouTubeMusic(Info.Source))
                {
                    return SSDECT.InvalidUrl;
                }
                else if (Info.Type == SSDEWT.Application)
                {
                    if (!CheckFile(Archive, Info.Source))
                    {
                        return SSDECT.Source;
                    }
                    else if (!CheckAppExtension(Info.Source))
                    {
                        return SSDECT.InvalidExtension;
                    }
                }

                // Arşivde SucroseProperties.json dosyası var mı?
                if (CheckFile(Archive, SMR.SucroseProperties))
                {
                    // Arşivdeki SucroseProperties.json dosyasını okuma
                    SSTHP Properties = SSTHP.FromJson(ReadFile(Archive, SMR.SucroseProperties));

                    // Properties içindeki TriggerTime değeri 1'den küçük mü?
                    if (Properties.TriggerTime <= 0)
                    {
                        return SSDECT.TriggerTime;
                    }

                    // Properties içindeki LoopMode değeri boş değil ve {0} içermiyor mu?
                    if (!string.IsNullOrEmpty(Properties.LoopMode) && !Properties.LoopMode.Contains("{0}"))
                    {
                        return SSDECT.LoopMode;
                    }

                    // Properties içindeki VolumeLevel değeri boş değil ve {0} içermiyor mu?
                    if (!string.IsNullOrEmpty(Properties.VolumeLevel) && !Properties.VolumeLevel.Contains("{0}"))
                    {
                        return SSDECT.VolumeLevel;
                    }

                    // Properties içindeki ShuffleMode değeri boş değil ve {0} içermiyor mu?
                    if (!string.IsNullOrEmpty(Properties.ShuffleMode) && !Properties.ShuffleMode.Contains("{0}"))
                    {
                        return SSDECT.ShuffleMode;
                    }

                    // Properties içindeki StretchMode değeri boş değil ve {0} içermiyor mu?
                    if (!string.IsNullOrEmpty(Properties.StretchMode) && !Properties.StretchMode.Contains("{0}"))
                    {
                        return SSDECT.StretchMode;
                    }

                    // Properties içindeki ComputerDate değeri boş değil ve {0} içermiyor mu?
                    if (!string.IsNullOrEmpty(Properties.ComputerDate) && !Properties.ComputerDate.Contains("{0}"))
                    {
                        return SSDECT.ComputerDate;
                    }
                }

                return SSDECT.Pass;
            }
            catch
            {
                return SSDECT.UnforeseenConsequences;
            }
        }

        private static string ReadFile(string Archive, string File)
        {
            try
            {
                using ZipArchive Archives = ZipFile.OpenRead(Archive);

                ZipArchiveEntry Entry = Archives.GetEntry(File);

                using StreamReader Reader = new(Entry.Open());

                return Reader.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static bool CheckAppExtension(string File)
        {
            try
            {
                string Extension = Path.GetExtension(File).Replace(".", "");

                return Enum.TryParse<SEAET>(Extension, true, out _);
                //return Enum.IsDefined(typeof(SEAET), Extension.ToUpperInvariant());
            }
            catch
            {
                return false;
            }
        }

        private static bool CheckGifExtension(string File)
        {
            try
            {
                string Extension = Path.GetExtension(File).Replace(".", "");

                return Extension.Equals("GIF", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static bool CheckWebExtension(string File)
        {
            try
            {
                string Extension = Path.GetExtension(File).Replace(".", "");

                return Enum.TryParse<SEWET>(Extension, true, out _);
                //return Enum.IsDefined(typeof(SEWET), Extension.ToUpperInvariant());
            }
            catch
            {
                return false;
            }
        }

        private static bool CheckVideoExtension(string File)
        {
            try
            {
                string Extension = Path.GetExtension(File).Replace(".", "");

                return Enum.TryParse<SEVET>(Extension, true, out _);
                //return Enum.IsDefined(typeof(SEVET), Extension.ToUpperInvariant());
            }
            catch
            {
                return false;
            }
        }

        private static bool CheckFile(string Archive, string File, StringComparison Comparison = StringComparison.Ordinal)
        {
            try
            {
                using ZipArchive Archives = ZipFile.OpenRead(Archive);

                return Archives.Entries.Any(Entry => string.Equals(Entry.Name, File, Comparison));
            }
            catch
            {
                return false;
            }
        }

        private static bool CheckArchive(string Archive)
        {
            try
            {
                using FileStream Stream = new(Archive, FileMode.Open, FileAccess.Read, FileShare.Read);

                byte[] ArchiveHeader = new byte[4];
                Stream.Read(ArchiveHeader, 0, 4);

                byte[] ZipHeader = new byte[] { 0x50, 0x4B, 0x03, 0x04 };

                return ArchiveHeader.SequenceEqual(ZipHeader);
            }
            catch
            {
                return false;
            }
        }
    }
}