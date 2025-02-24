﻿using System.IO;
using System.Net.Http;
using SMMP = Sucrose.Manager.Manage.Portal;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SMMRGH = Sucrose.Memory.Manage.Readonly.GitHub;
using SMMRU = Sucrose.Memory.Manage.Readonly.Url;
using SPMI = Sucrose.Portal.Manage.Internal;
using SSHG = Skylark.Standard.Helper.GitHub;
using SSIIC = Skylark.Standard.Interface.IContents;
using SSSHS = Sucrose.Shared.Store.Helper.Store;
using SSSID = Sucrose.Shared.Store.Interface.Data;
using SSSIW = Sucrose.Shared.Store.Interface.Wallpaper;
using SSSMI = Sucrose.Shared.Store.Manage.Internal;

namespace Sucrose.Shared.Store.Helper.GitHub
{
    internal static class Download
    {
        public static bool Store(string Store, string Agent, string Key)
        {
            string StorePath = Path.GetDirectoryName(Store);

            if (Directory.Exists(StorePath))
            {
                if (File.Exists(Store))
                {
                    DateTime CurrentTime = DateTime.Now;
                    DateTime ModificationTime = File.GetLastWriteTime(Store);

                    TimeSpan ElapsedDuration = CurrentTime - ModificationTime;

                    if (ElapsedDuration >= TimeSpan.FromHours(SMMP.StoreDuration) || !SSSHS.ReadCheck(Store))
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
                Directory.CreateDirectory(StorePath);
            }

            InitializeClient(Agent, Key);

            try
            {
                List<SSIIC> Contents = SSHG.ContentsList(SMMRGH.Owner, SMMRGH.StoreRepository, SMMRGH.StoreSource, SMMRGH.Branch, Agent, Key);

                foreach (SSIIC Content in Contents)
                {
                    if (Content.Name == SMMRC.StoreFile)
                    {
                        using HttpResponseMessage Response = SSSMI.Client.GetAsync(Content.DownloadUrl).Result;

                        Response.EnsureSuccessStatusCode();

                        if (Response.IsSuccessStatusCode)
                        {
                            using (Stream Stream = Response.Content.ReadAsStreamAsync().Result)
                            using (FileStream FStream = new(Store, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                            {
                                Stream.CopyTo(FStream);
                            }

                            return SSSHS.ReadCheck(Store);
                        }

                        break;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static bool Cache(KeyValuePair<string, SSSIW> Wallpaper, string Theme, string Agent, string Key)
        {
            string InfoPath = Path.Combine(Theme, SMMRC.SucroseInfo);
            string CoverPath = Path.Combine(Theme, Wallpaper.Value.Cover);

            if (Directory.Exists(Theme))
            {
                if (File.Exists(InfoPath) && File.Exists(CoverPath))
                {
                    DateTime CurrentTime = DateTime.Now;
                    DateTime ModificationTime = File.GetLastWriteTime(Theme);

                    TimeSpan ElapsedDuration = CurrentTime - ModificationTime;

                    if (ElapsedDuration >= TimeSpan.FromHours(SMMP.StoreDuration))
                    {
                        File.Delete(InfoPath);
                        File.Delete(CoverPath);

                        SPMI.StoreDownloading[Theme] = false;
                    }
                    else
                    {
                        SPMI.StoreDownloading[Theme] = true;

                        return true;
                    }
                }
                else
                {
                    if (File.Exists(InfoPath))
                    {
                        File.Delete(InfoPath);
                    }

                    if (File.Exists(CoverPath))
                    {
                        File.Delete(CoverPath);
                    }

                    SPMI.StoreDownloading[Theme] = false;
                }
            }
            else
            {
                Directory.CreateDirectory(Theme);
            }

            if (SPMI.StoreDownloading.ContainsKey(Theme) && SPMI.StoreDownloading[Theme])
            {
                return true;
            }
            else
            {
                SPMI.StoreDownloading[Theme] = false;

                InitializeClient(Agent, Key);

                try
                {
                    string InfoUri = EncodeSpacesOnly($"{SMMRU.RawGitHubStoreBranch}/{Wallpaper.Value.Source}/{Wallpaper.Key}/{SMMRC.SucroseInfo}");
                    string CoverUri = EncodeSpacesOnly($"{SMMRU.RawGitHubStoreBranch}/{Wallpaper.Value.Source}/{Wallpaper.Key}/{Wallpaper.Value.Cover}");

                    using HttpResponseMessage ResponseInfo = SSSMI.Client.GetAsync(InfoUri).Result;
                    using HttpResponseMessage ResponseCover = SSSMI.Client.GetAsync(CoverUri).Result;

                    ResponseInfo.EnsureSuccessStatusCode();
                    ResponseCover.EnsureSuccessStatusCode();

                    if (ResponseInfo.IsSuccessStatusCode && ResponseCover.IsSuccessStatusCode)
                    {
                        using (FileStream InfoFile = new(InfoPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                        using (FileStream CoverFile = new(CoverPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                        {
                            ResponseInfo.Content.CopyToAsync(InfoFile).Wait();
                            ResponseCover.Content.CopyToAsync(CoverFile).Wait();
                        }

                        SPMI.StoreDownloading[Theme] = true;

                        return true;
                    }
                }
                catch
                {
                    return false;
                }

                return false;
            }
        }

        public static async Task<bool> Theme(string Source, string Output, string Agent, string Guid, string Keys, string Key, bool Sub = true)
        {
            InitializeClient(Agent, Key);

            SSSMI.StoreService.Info[Keys] = new SSSID(0, 0, 0, "0%", "0/0", Guid);

            return await DownloadFolder(Source, Output, Agent, Keys, Key, Sub);
        }

        private static string EncodeSpacesOnly(string Source)
        {
            return Source.Replace(" ", "%20");
        }

        private static void InitializeClient(string Agent, string Key)
        {
            if (SSSMI.State)
            {
                SSSMI.State = false;

                SSSMI.Client.DefaultRequestHeaders.Clear();

                SSSMI.Client.DefaultRequestHeaders.Add("User-Agent", Agent);

                if (!string.IsNullOrEmpty(Key))
                {
                    SSSMI.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Key}");
                }
            }
        }

        private static async Task<bool> DownloadFolder(string Source, string Output, string Agent, string Keys, string Key, bool Sub)
        {
            SSSMI.StoreService.TotalFileCount(Keys, await GetTotalFileCount(Source, Agent, Key, Sub));

            return await DownloadFilesRecursively(Source, Output, Agent, Keys, Key, Sub);
        }

        private static async Task<int> GetTotalFileCount(string Source, string Agent, string Key, bool Sub)
        {
            List<SSIIC> Contents = SSHG.ContentsList(SMMRGH.Owner, SMMRGH.StoreRepository, Source, SMMRGH.Branch, Agent, Key);

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
            List<SSIIC> Contents = SSHG.ContentsList(SMMRGH.Owner, SMMRGH.StoreRepository, Source, SMMRGH.Branch, Agent, Key);

            foreach (SSIIC Content in Contents)
            {
                if (Content.Type == "file")
                {
                    string FilePath = Path.Combine(Output, Content.Name);

                    if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                    }

                    using HttpResponseMessage Response = await SSSMI.Client.GetAsync(Content.DownloadUrl);

                    Response.EnsureSuccessStatusCode();

                    using (Stream Stream = await Response.Content.ReadAsStreamAsync())
                    using (FileStream FStream = new(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await Stream.CopyToAsync(FStream);
                    }

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