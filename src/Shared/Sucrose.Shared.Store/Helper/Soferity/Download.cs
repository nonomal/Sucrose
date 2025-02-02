﻿using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using SMMP = Sucrose.Manager.Manage.Portal;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SMMRS = Sucrose.Memory.Manage.Readonly.Soferity;
using SMMRU = Sucrose.Memory.Manage.Readonly.Url;
using SPMI = Sucrose.Portal.Manage.Internal;
using SSDESST = Sucrose.Shared.Dependency.Enum.StoreServerType;
using SSSHF = Sucrose.Shared.Space.Helper.Filing;
using SSSHS = Sucrose.Shared.Store.Helper.Store;
using SSSIC = Sucrose.Shared.Store.Interface.Contents;
using SSSID = Sucrose.Shared.Store.Interface.Data;
using SSSIW = Sucrose.Shared.Store.Interface.Wallpaper;
using SSSMI = Sucrose.Shared.Store.Manage.Internal;

namespace Sucrose.Shared.Store.Helper.Soferity
{
    internal static class Download
    {
        public static bool Store(string Store, string Agent)
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
                        SSSHF.Delete(Store);
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

            InitializeClient(Agent);

            try
            {
                string StoreUri = $"{SSSHS.Source(SSDESST.Soferity)}/{SMMRS.StoreSource}/{SMMRC.StoreFile}";

                using HttpResponseMessage Response = SSSMI.Client.GetAsync(StoreUri).Result;

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
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static bool Pattern(string Pattern, string Agent)
        {
            string PatternPath = Path.GetDirectoryName(Pattern);

            if (Directory.Exists(PatternPath))
            {
                if (File.Exists(Pattern))
                {
                    DateTime CurrentTime = DateTime.Now;
                    DateTime ModificationTime = File.GetLastWriteTime(Pattern);

                    TimeSpan ElapsedDuration = CurrentTime - ModificationTime;

                    if (ElapsedDuration >= TimeSpan.FromHours(SMMP.StoreDuration) || !SSSHS.ReadCheck(Pattern))
                    {
                        SSSHF.Delete(Pattern);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(PatternPath);
            }

            InitializeClient(Agent);

            try
            {
                string PatternUri = $"{SMMRU.Soferity}/{SMMRS.Version}/{SMMRS.Kernel}/{SMMRS.Pattern}";

                using HttpResponseMessage Response = SSSMI.Client.GetAsync(PatternUri).Result;

                Response.EnsureSuccessStatusCode();

                if (Response.IsSuccessStatusCode)
                {
                    using (Stream Stream = Response.Content.ReadAsStreamAsync().Result)
                    using (FileStream FStream = new(Pattern, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        Stream.CopyTo(FStream);
                    }

                    return SSSHS.ReadCheck(Pattern);
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static bool Cache(KeyValuePair<string, SSSIW> Wallpaper, string Theme, string Agent)
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
                        SSSHF.Delete(InfoPath);
                        SSSHF.Delete(CoverPath);

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
                        SSSHF.Delete(InfoPath);
                    }

                    if (File.Exists(CoverPath))
                    {
                        SSSHF.Delete(CoverPath);
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

                InitializeClient(Agent);

                try
                {
                    string InfoUri = EncodeSpacesOnly($"{SMMRU.SoferityStore}/{Wallpaper.Value.Source}/{Wallpaper.Key}/{SMMRC.SucroseInfo}");
                    string CoverUri = EncodeSpacesOnly($"{SMMRU.SoferityStore}/{Wallpaper.Value.Source}/{Wallpaper.Key}/{Wallpaper.Value.Cover}");

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

        public static async Task<bool> Theme(string Source, string Output, string Agent, string Guid, string Keys, bool Sub = true)
        {
            InitializeClient(Agent);

            SSSMI.StoreService.Info[Keys] = new SSSID(0, 0, 0, "0%", "0/0", Guid);

            return await DownloadFolder(Source, Output, Agent, Keys, Sub);
        }

        private static string EncodeSpacesOnly(string Source)
        {
            return Source.Replace(" ", "%20");
        }

        private static void InitializeClient(string Agent)
        {
            if (SSSMI.State)
            {
                SSSMI.State = false;

                SSSMI.Client.DefaultRequestHeaders.Clear();

                SSSMI.Client.DefaultRequestHeaders.Add("User-Agent", Agent);
            }
        }

        private static async Task<bool> DownloadFolder(string Source, string Output, string Agent, string Keys, bool Sub)
        {
            SSSMI.StoreService.TotalFileCount(Keys, await GetTotalFileCount(Source, Agent, Sub));

            return await DownloadFilesRecursively(Source, Output, Agent, Keys, Sub);
        }

        private static async Task<int> GetTotalFileCount(string Source, string Agent, bool Sub)
        {
            List<SSSIC> Contents = ContentsList(SMMRS.StoreDirectory, Source, Agent);

            int Count = 0;

            foreach (SSSIC Content in Contents)
            {
                if (Content.Type == "file")
                {
                    Count++;
                }
                else if (Content.Type == "dir" && Sub)
                {
                    Source = Content.Path;

                    int SubTotalFileCount = await GetTotalFileCount(Source, Agent, Sub);

                    Count += SubTotalFileCount;
                }
            }

            return Count;
        }

        private static async Task<bool> DownloadFilesRecursively(string Source, string Output, string Agent, string Keys, bool Sub)
        {
            List<SSSIC> Contents = ContentsList(SMMRS.StoreDirectory, Source, Agent);

            foreach (SSSIC Content in Contents)
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

                    await DownloadFilesRecursively(Source, SubOutput, Agent, Keys, Sub);
                }
            }

            return true;
        }

        private static List<SSSIC> ContentsList(string Repository, string Path, string Agent)
        {
            return JsonConvert.DeserializeObject<List<SSSIC>>(Contents(Repository, Path, Agent));
        }

        private static string Contents(string Repository, string Path, string Agent)
        {
            InitializeClient(Agent);

            string BaseUri = $"{SMMRU.Soferity}/{SMMRS.Version}/{SMMRS.Kernel}/{SMMRS.File}/{Repository}";

            if (!string.IsNullOrEmpty(Path))
            {
                string Replace = $"{SMMRS.StoreDirectory}/";

#if NET48_OR_GREATER
                Path = Path.StartsWith(Replace) ? Path.Substring(Replace.Length) : Path;
#else
                Path = Path.StartsWith(Replace) ? Path[Replace.Length..] : Path;
#endif

                BaseUri += $"/{Path}";
            }

            HttpResponseMessage Response = SSSMI.Client.GetAsync(BaseUri).Result;

            string Result = Response.Content.ReadAsStringAsync().Result;

            Response.EnsureSuccessStatusCode();

            if (Response.IsSuccessStatusCode)
            {
                return Result;
            }
            else
            {
                throw new Exception(Result);
            }
        }
    }
}