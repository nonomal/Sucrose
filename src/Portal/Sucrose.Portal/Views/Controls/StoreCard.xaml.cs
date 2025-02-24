﻿using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;
using SHG = Skylark.Helper.Generator;
using SHV = Skylark.Helper.Versionly;
using SMMB = Sucrose.Manager.Manage.Backgroundog;
using SMMCL = Sucrose.Memory.Manage.Constant.Library;
using SMME = Sucrose.Manager.Manage.Engine;
using SMMG = Sucrose.Manager.Manage.General;
using SMMI = Sucrose.Manager.Manage.Internal;
using SMML = Sucrose.Manager.Manage.Library;
using SMMP = Sucrose.Manager.Manage.Portal;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SMMRF = Sucrose.Memory.Manage.Readonly.Folder;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SMMRS = Sucrose.Memory.Manage.Readonly.Soferity;
using SMMRU = Sucrose.Memory.Manage.Readonly.Url;
using SMMVA = Sucrose.Memory.Manage.Valuable.App;
using SPEIL = Sucrose.Portal.Extension.ImageLoader;
using SPMI = Sucrose.Portal.Manage.Internal;
using SPVCTR = Sucrose.Portal.Views.Controls.ThemeReport;
using SRER = Sucrose.Resources.Extension.Resources;
using SSCHV = Sucrose.Shared.Core.Helper.Version;
using SSDECT = Sucrose.Shared.Dependency.Enum.CommandType;
using SSDESST = Sucrose.Shared.Dependency.Enum.StoreServerType;
using SSDMMP = Sucrose.Shared.Dependency.Manage.Manager.Portal;
using SSLHK = Sucrose.Shared.Live.Helper.Kill;
using SSLHR = Sucrose.Shared.Live.Helper.Run;
using SSSHC = Sucrose.Shared.Space.Helper.Copy;
using SSSHF = Sucrose.Shared.Space.Helper.Filing;
using SSSHGHD = Sucrose.Shared.Store.Helper.GitHub.Download;
using SSSHL = Sucrose.Shared.Space.Helper.Live;
using SSSHN = Sucrose.Shared.Space.Helper.Network;
using SSSHP = Sucrose.Shared.Space.Helper.Processor;
using SSSHS = Sucrose.Shared.Store.Helper.Store;
using SSSHSD = Sucrose.Shared.Store.Helper.Soferity.Download;
using SSSHU = Sucrose.Shared.Space.Helper.User;
using SSSID = Sucrose.Shared.Store.Interface.Data;
using SSSIW = Sucrose.Shared.Store.Interface.Wallpaper;
using SSSMDTD = Sucrose.Shared.Space.Model.DownloadTelemetryData;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;
using SSSTMI = Sucrose.Shared.Store.Manage.Internal;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;
using SXAGAB = Sucrose.XamlAnimatedGif.AnimationBehavior;

namespace Sucrose.Portal.Views.Controls
{
    /// <summary>
    /// StoreCard.xaml etkileşim mantığı
    /// </summary>
    public partial class StoreCard : UserControl, IDisposable
    {
        private readonly KeyValuePair<string, SSSIW> Wallpaper = new();
        private readonly string Theme = string.Empty;
        private readonly SPEIL Loader = new();
        private string Keys = string.Empty;
        private readonly string Agent;
        private readonly string Guid;
        private readonly string Key;
        private bool State;
        private SSTHI Info;
        private bool Error;

        internal StoreCard(string Theme, KeyValuePair<string, SSSIW> Wallpaper, string Agent, string Key)
        {
            this.Key = Key;
            this.Agent = Agent;
            this.Theme = Theme;
            this.Wallpaper = Wallpaper;
            Guid = Path.Combine(Wallpaper.Value.Source, Wallpaper.Key);

            InitializeComponent();
        }

        private async void Start()
        {
            if (Info != null && Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
            {
                if (DownloadSymbol.Symbol == SymbolRegular.CloudArrowDown24)
                {
                    if (SSSHN.GetHostEntry())
                    {
                        State = true;
                        Error = false;

                        DownloadSymbol.Symbol = SymbolRegular.Empty;

                        DownloadRing.Visibility = Visibility.Visible;
                        DownloadSymbol.Visibility = Visibility.Collapsed;

                        await Task.Run(SendDownload);

                        await Task.Run(DownloadTheme);
                    }
                    else
                    {
                        DownloadSymbol.Foreground = SRER.GetResource<Brush>("PaletteRedBrush");
                        DownloadSymbol.Symbol = SymbolRegular.CloudDismiss24;

                        await Task.Delay(3000);

                        DownloadSymbol.Foreground = SRER.GetResource<Brush>("TextFillColorPrimaryBrush");
                        DownloadSymbol.Symbol = SymbolRegular.CloudArrowDown24;
                    }
                }
            }
            else if (Info != null)
            {
                DownloadSymbol.Visibility = Visibility.Hidden;
                IncompatibleVersion.Visibility = Visibility.Visible;

                if (!SSSHP.Work(SSSMI.Update))
                {
                    SSSHP.Run(SSSMI.Commandog, $"{SMMRG.StartCommand}{SSDECT.Update}{SMMRG.ValueSeparator}{SSSMI.Update}");
                }
            }
        }

        private async void SendDownload()
        {
            try
            {
                if (SMMG.TelemetryData)
                {
                    using HttpClient Client = new();

                    Client.DefaultRequestHeaders.Add("User-Agent", SMMG.UserAgent);

                    SSSMDTD DownloadData = new()
                    {
                        AppVersion = SSCHV.GetText(),
                        WallpaperTitle = Wallpaper.Key,
                        WallpaperVersion = $"{Info.Version}",
                        WallpaperAppVersion = $"{Info.AppVersion}",
                        WallpaperLocation = $"{Wallpaper.Value.Source.Split('/').LastOrDefault()}/{Wallpaper.Key}"
                    };

                    StringContent Content = new(JsonConvert.SerializeObject(DownloadData, Formatting.Indented), SMMRS.Encoding, SMMRS.ApplicationJson);

                    HttpResponseMessage Response = await Client.PostAsync($"{SMMRU.Soferity}/{SMMRS.Version}/{SMMRS.Telemetry}/{SMMRS.Download}/{SSSHU.GetGuid()}", Content);

                    Response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        private async void DownloadTheme()
        {
            try
            {
                do
                {
                    Keys = SHG.GenerateString(SMMVA.Chars, 25, SMMRG.Randomise);
                } while (Directory.Exists(Path.Combine(SMML.Location, Keys)));

                SSSTMI.StoreService.InfoChanged += (s, e) => StoreService_InfoChanged(Keys);

                string LibraryPath = Path.Combine(SMML.Location, Keys);
                string TemporaryPath = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.Store, SMMRF.Temporary, Keys);

                switch (SSDMMP.StoreServerType)
                {
                    case SSDESST.GitHub:
                        await SSSHGHD.Theme(Path.Combine(Wallpaper.Value.Source, Wallpaper.Key), TemporaryPath, Agent, Guid, Keys, Key);
                        break;
                    default:
                        await SSSHSD.Theme(Path.Combine(Wallpaper.Value.Source, Wallpaper.Key), TemporaryPath, Agent, Guid, Keys);
                        break;
                }

                await Task.Delay(100);

                if (Directory.Exists(TemporaryPath))
                {
                    SSSHC.Folder(TemporaryPath, LibraryPath);

                    SSSHF.Write(Path.Combine(LibraryPath, SMMRC.SucroseStore), string.Empty);

                    if ((!SMMB.ClosePerformance && !SMMB.PausePerformance) || !SSSHP.Work(SSSMI.Backgroundog))
                    {
                        if (SMME.StoreStart)
                        {
                            SMMI.LibrarySettingManager.SetSetting(SMMCL.Selected, Path.GetFileName(Keys));

                            if (SSSHL.Run())
                            {
                                SSLHK.Stop();
                            }

                            SSLHR.Start();
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                Error = true;

                await SSWEW.Watch_CatchException(Exception);

                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    if (!string.IsNullOrEmpty(Keys) && SSSTMI.StoreService.Info.ContainsKey(Keys))
                    {
                        SSSTMI.StoreService.Info.Remove(Keys);
                    }

                    State = false;

                    Download.ToolTip = null;

                    DownloadRing.Visibility = Visibility.Collapsed;
                    DownloadSymbol.Visibility = Visibility.Visible;

                    DownloadSymbol.Foreground = SRER.GetResource<Brush>("PaletteRedBrush");
                    DownloadSymbol.Symbol = SymbolRegular.CloudOff24;

                    await Task.Delay(3000);

                    DownloadSymbol.Foreground = SRER.GetResource<Brush>("TextFillColorPrimaryBrush");
                    DownloadSymbol.Symbol = SymbolRegular.CloudArrowDown24;
                });
            }
        }

        private async Task<bool> DownloadCache()
        {
            if (SPMI.StoreDownloader.ContainsKey(Theme))
            {
                while (!SPMI.StoreDownloading.ContainsKey(Theme) || !SPMI.StoreDownloading[Theme])
                {
                    await Task.Delay(100);
                }

                Info = SSTHI.ReadJson(Path.Combine(Theme, SMMRC.SucroseInfo));

                return true;
            }
            else
            {
                SPMI.StoreDownloader[Theme] = false;

                SPMI.StoreDownloader[Theme] = SSDMMP.StoreServerType switch
                {
                    SSDESST.GitHub => SSSHGHD.Cache(Wallpaper, Theme, Agent, Key),
                    _ => SSSHSD.Cache(Wallpaper, Theme, Agent),
                };

                if (SPMI.StoreDownloader[Theme])
                {
                    while (!SPMI.StoreDownloading.ContainsKey(Theme) || !SPMI.StoreDownloading[Theme])
                    {
                        await Task.Delay(100);
                    }

                    Info = SSTHI.ReadJson(Path.Combine(Theme, SMMRC.SucroseInfo));

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async void StoreService_InfoChanged(string Keys)
        {
            if (this.Keys == Keys && SSSTMI.StoreService.Info.ContainsKey(Keys))
            {
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    DownloadRing.Progress = SSSTMI.StoreService.Info[Keys].ProgressPercentage;

                    ToolTip RingTip = new()
                    {
                        Content = SSSTMI.StoreService.Info[Keys].Percentage
                    };

                    Download.ToolTip = RingTip;

                    if (SSSTMI.StoreService.Info[Keys].ProgressPercentage >= 100)
                    {
                        if (State)
                        {
                            State = false;

                            await Task.Delay(1500);

                            if (!Error)
                            {
                                Download.ToolTip = null;
                                DownloadRing.Visibility = Visibility.Collapsed;
                                DownloadSymbol.Visibility = Visibility.Visible;

                                DownloadSymbol.Foreground = SRER.GetResource<Brush>("SystemFillColorSuccessBrush");
                                DownloadSymbol.Symbol = SymbolRegular.CloudCheckmark24;

                                await Task.Delay(3000);

                                DownloadSymbol.Foreground = SRER.GetResource<Brush>("TextFillColorPrimaryBrush");
                                DownloadSymbol.Symbol = SymbolRegular.CloudArrowDown24;
                            }
                        }
                    }
                });
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void MenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!SSSHP.Work(SSSMI.Update))
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMMRG.StartCommand}{SSDECT.Update}{SMMRG.ValueSeparator}{SSSMI.Update}");
            }
        }

        private void MenuInstall_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            MenuInstall.Header = SRER.GetValue("Portal", "StoreCard", "MenuInstall");

            if (Info != null)
            {
                MenuReport.IsEnabled = true;
            }
            else
            {
                MenuReport.IsEnabled = false;
            }

            if (DownloadSymbol.Symbol == SymbolRegular.CloudArrowDown24 && Info != null && Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
            {
                MenuInstall.IsEnabled = true;

                MenuUpdate.Visibility = Visibility.Collapsed;
            }
            else
            {
                MenuInstall.IsEnabled = false;

                if (Info != null && Info.AppVersion.CompareTo(SHV.Entry()) > 0)
                {
                    MenuUpdate.Visibility = Visibility.Visible;

                    if (SSSHP.Work(SSSMI.Update))
                    {
                        MenuUpdate.IsEnabled = false;

                        MenuUpdate.Header = SRER.GetValue("Portal", "StoreCard", "MenuUpdating");
                    }
                    else
                    {
                        MenuUpdate.IsEnabled = true;

                        MenuUpdate.Header = SRER.GetValue("Portal", "StoreCard", "MenuUpdate");
                    }

                    MenuInstall.Header += $" ({SRER.GetValue("Portal", "StoreCard", "Incompatible")})";
                }
            }
        }

        private void StoreCard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Info != null && SMMP.StorePreview)
            {
                string GifPath = $"{SSSHS.Source(SSDMMP.StoreServerType)}/{Wallpaper.Value.Source}/{Wallpaper.Key}/{Wallpaper.Value.Live}";

                SXAGAB.SetSourceUri(Imaginer, new(GifPath));
                SXAGAB.AddLoadedHandler(Imaginer, Imaginer_MediaOpened);
            }
        }

        private void StoreCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Info != null && SMMP.StorePreview)
            {
                Imaginer.Source = null;
                SXAGAB.SetSourceUri(Imaginer, null);

                Imagine.Visibility = Visibility.Visible;
                Imaginer.Visibility = Visibility.Hidden;

                if (SMMP.StorePreviewHide)
                {
                    Preview.Visibility = Visibility.Visible;
                }

                Dispose();
            }
        }

        private void Imaginer_MediaOpened(object sender, RoutedEventArgs e)
        {
            Imaginer.Visibility = Visibility.Visible;
            Imagine.Visibility = Visibility.Hidden;

            if (SMMP.StorePreviewHide)
            {
                Preview.Visibility = Visibility.Hidden;
            }

            Dispose();
        }

        private async void MenuReport_Click(object sender, RoutedEventArgs e)
        {
            SPVCTR ThemeReport = new()
            {
                Info = Info,
                Theme = Theme,
                Wallpaper = Wallpaper
            };

            await ThemeReport.ShowAsync();

            ThemeReport.Dispose();
        }

        private async void StoreCard_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool Result = await Task.Run(DownloadCache);

                if (Result)
                {
                    ToolTip TitleTip = new()
                    {
                        Content = Info.Title
                    };

                    ToolTip DescriptionTip = new()
                    {
                        Content = Info.Description
                    };

                    ThemeTitle.ToolTip = TitleTip;
                    ThemeDescription.ToolTip = DescriptionTip;

                    ThemeTitle.Text = Info.Title;
                    ThemeDescription.Text = Info.Description;

                    string ImagePath = Path.Combine(Theme, Info.Thumbnail);

                    if (File.Exists(ImagePath))
                    {
                        try
                        {
                            Imagine.Source = await Loader.LoadOptimalAsync(ImagePath);
                        }
                        catch (Exception Exception)
                        {
                            await SSWEW.Watch_CatchException(Exception);
                        }
                    }

                    if (Info.AppVersion.CompareTo(SHV.Entry()) > 0)
                    {
                        DownloadSymbol.Visibility = Visibility.Hidden;
                        IncompatibleVersion.Visibility = Visibility.Visible;
                    }

                    KeyValuePair<string, SSSID> Matching = SSSTMI.StoreService.Info.FirstOrDefault(Pair => Pair.Value.Guid == Guid);

                    if (!Matching.Equals(default(KeyValuePair<string, SSSID>)))
                    {
                        Keys = Matching.Key;

                        if (Matching.Value.ProgressPercentage < 100)
                        {
                            State = true;

                            StoreService_InfoChanged(Keys);

                            DownloadSymbol.Symbol = SymbolRegular.Empty;

                            DownloadRing.Visibility = Visibility.Visible;
                            DownloadSymbol.Visibility = Visibility.Collapsed;

                            SSSTMI.StoreService.InfoChanged += (s, e) => StoreService_InfoChanged(Keys);
                        }
                    }

                    await Task.Delay(100);

                    Card.Visibility = Visibility.Visible;
                    Progress.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Cursor = Cursors.Arrow;
                    Warn.Visibility = Visibility.Visible;
                    Progress.Visibility = Visibility.Collapsed;
                }

                Dispose();
            }
            catch
            {
                Cursor = Cursors.Arrow;
                Warn.Visibility = Visibility.Visible;
                Progress.Visibility = Visibility.Collapsed;

                Dispose();
            }
        }

        private void StoreCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Start();
        }

        public void Dispose()
        {
            Loader.Dispose();

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}