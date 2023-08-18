﻿using System.IO;
using System.Net.Http;
using SMR = Sucrose.Memory.Readonly;
using SSHG = Skylark.Standard.Helper.GitHub;
using SSIIC = Skylark.Standard.Interface.IContents;
using SSSHS = Sucrose.Shared.Space.Helper.Security;
using SSSID = Sucrose.Shared.Store.Interface.Data;
using SSSMI = Sucrose.Shared.Store.Manage.Internal;
using SSSIW = Sucrose.Shared.Store.Interface.Wallpaper;

namespace Sucrose.Shared.Store.Helper
{
    internal static class Download
    {
        public static bool Store(string Store, string Agent, string Key)
        {
            if (Directory.Exists(Path.GetDirectoryName(Store)))
            {
                if (File.Exists(Store))
                {
                    DateTime CurrentTime = DateTime.Now;
                    DateTime ModificationTime = File.GetLastWriteTime(Store);

                    TimeSpan ElapsedDuration = CurrentTime - ModificationTime;

                    if (ElapsedDuration >= SSSMI.RequiredDuration)
                    {
                        File.Delete(Store);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Store));
            }

            InitializeClient(Agent, Key);

            List<SSIIC> Contents = SSHG.ContentsList(SMR.Owner, SMR.WallpaperRepository, SMR.WallpaperSource, SMR.Branch, Agent, Key);

            foreach (SSIIC Content in Contents)
            {
                if (Content.Name == SMR.StoreFile)
                {
                    using HttpResponseMessage Response = SSSMI.Client.GetAsync(Content.DownloadUrl).Result;

                    Response.EnsureSuccessStatusCode();

                    if (Response.IsSuccessStatusCode)
                    {
                        using HttpContent HContent = Response.Content;

                        using Stream Stream = HContent.ReadAsStreamAsync().Result;
                        using FileStream FStream = new(Store, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

                        Stream.CopyTo(FStream);

                        return true;
                    }

                    break;
                }
            }

            return false;
        }

        public static bool Cache(string Theme, KeyValuePair<string, SSSIW> Wallpaper, string Agent, string Key)
        {
            string Info = Path.Combine(Theme, SMR.SucroseInfo);
            string Cover = Path.Combine(Theme, Wallpaper.Value.Cover);

            if (Directory.Exists(Theme))
            {
                if (File.Exists(Info) && File.Exists(Cover))
                {
                    DateTime CurrentTime = DateTime.Now;
                    DateTime ModificationTime = File.GetLastWriteTime(Theme);

                    TimeSpan ElapsedDuration = CurrentTime - ModificationTime;

                    if (ElapsedDuration >= SSSMI.RequiredDuration)
                    {
                        File.Delete(Info);
                        File.Delete(Cover);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (File.Exists(Info))
                    {
                        File.Delete(Info);
                    }

                    if (File.Exists(Cover))
                    {
                        File.Delete(Cover);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Theme);
            }

            InitializeClient(Agent, Key);

            using HttpResponseMessage ResponseInfo = SSSMI.Client.GetAsync($"{SMR.RawWebsite}/{Wallpaper.Value.Source}/{Wallpaper.Key}/{SMR.SucroseInfo}").Result;
            using HttpResponseMessage ResponseCover = SSSMI.Client.GetAsync($"{SMR.RawWebsite}/{Wallpaper.Value.Source}/{Wallpaper.Key}/{Wallpaper.Value.Cover}").Result;

            ResponseInfo.EnsureSuccessStatusCode();
            ResponseCover.EnsureSuccessStatusCode();

            if (ResponseInfo.IsSuccessStatusCode && ResponseCover.IsSuccessStatusCode)
            {
                using HttpContent InfoContent = ResponseInfo.Content;
                using HttpContent CoverContent = ResponseCover.Content;

                using Stream InfoStream = InfoContent.ReadAsStreamAsync().Result;
                using Stream CoverStream = CoverContent.ReadAsStreamAsync().Result;

                using FileStream InfoFile = new(Info, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                using FileStream CoverFile = new(Cover, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

                InfoStream.CopyTo(InfoFile);
                CoverStream.CopyTo(CoverFile);

                return true;
            }

            return false;
        }

        public static async Task<bool> Theme(string Source, string Output, string Agent, string Keys, string Key, bool Sub = true)
        {
            InitializeClient(Agent, Key);

            SSSMI.StoreService.Info.Add(Keys, new SSSID(0, 0, 0, "0%", "0/0"));

            return await DownloadFolder(Source, Output, Agent, Keys, Key, Sub);
        }

        private static void InitializeClient(string Agent, string Key)
        {
            SSSHS.Apply();

            SSSMI.Client.DefaultRequestHeaders.Clear();

            SSSMI.Client.DefaultRequestHeaders.Add("User-Agent", Agent);

            if (!string.IsNullOrEmpty(Key))
            {
                SSSMI.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Key}");
            }
        }

        private static async Task<bool> DownloadFolder(string Source, string Output, string Agent, string Keys, string Key, bool Sub)
        {
            SSSMI.StoreService.TotalFileCount(Keys, await GetTotalFileCount(Source, Agent, Key, Sub));

            return await DownloadFilesRecursively(Source, Output, Agent, Keys, Key, Sub);
        }

        private static async Task<int> GetTotalFileCount(string Source, string Agent, string Key, bool Sub)
        {
            List<SSIIC> Contents = SSHG.ContentsList(SMR.Owner, SMR.WallpaperRepository, Source, SMR.Branch, Agent, Key);

            int Count = 0;

            foreach (SSIIC Content in Contents)
            {
                if (Content.Type == "file")
                {
                    Count++;
                }
                else if (Content.Type == "dir" && Sub)
                {
                    Source = Content.Path;

                    int SubTotalFileCount = await GetTotalFileCount(Source, Agent, Key, Sub);

                    Count += SubTotalFileCount;
                }
            }

            return Count;
        }

        private static async Task<bool> DownloadFilesRecursively(string Source, string Output, string Agent, string Keys, string Key, bool Sub)
        {
            List<SSIIC> Contents = SSHG.ContentsList(SMR.Owner, SMR.WallpaperRepository, Source, SMR.Branch, Agent, Key);

            foreach (SSIIC Content in Contents)
            {
                if (Content.Type == "file")
                {
                    string FilePath = Path.Combine(Output, Content.Name);

                    if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                    }

                    using HttpResponseMessage Response = SSSMI.Client.GetAsync(Content.DownloadUrl).Result;
                    using Stream Stream = Response.Content.ReadAsStreamAsync().Result;
                    using FileStream FStream = new(FilePath, FileMode.Create);

                    Stream.CopyToAsync(FStream).Wait();

                    Response.Dispose();
                    FStream.Dispose();
                    Stream.Dispose();

                    SSSMI.StoreService.DownloadedFileCount(Keys, SSSMI.StoreService.Info[Keys].DownloadedFileCount + 1);
                    SSSMI.StoreService.ProgressPercentage(Keys, (double)SSSMI.StoreService.Info[Keys].DownloadedFileCount / SSSMI.StoreService.Info[Keys].TotalFileCount * 100);

                    SSSMI.StoreService.Percentage(Keys, $"{SSSMI.StoreService.Info[Keys].ProgressPercentage:F2}%"); //F2 - F0
                    SSSMI.StoreService.State(Keys, $"{SSSMI.StoreService.Info[Keys].DownloadedFileCount}/{SSSMI.StoreService.Info[Keys].TotalFileCount}");
                }
                else if (Content.Type == "dir" && Sub)
                {
                    Source = Content.Path;
                    string SubOutput = Path.Combine(Output, Content.Name);

                    await DownloadFilesRecursively(Source, SubOutput, Agent, Keys, Key, Sub);
                }
            }

            return true;
        }
    }
}