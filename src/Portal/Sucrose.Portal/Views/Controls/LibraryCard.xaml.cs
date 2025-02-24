﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;
using SHV = Skylark.Helper.Versionly;
using SMMB = Sucrose.Manager.Manage.Backgroundog;
using SMMC = Sucrose.Manager.Manage.Cycling;
using SMMCC = Sucrose.Memory.Manage.Constant.Cycling;
using SMMCL = Sucrose.Memory.Manage.Constant.Library;
using SMMI = Sucrose.Manager.Manage.Internal;
using SMML = Sucrose.Manager.Manage.Library;
using SMMP = Sucrose.Manager.Manage.Portal;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SMMRF = Sucrose.Memory.Manage.Readonly.Folder;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SPEIL = Sucrose.Portal.Extension.ImageLoader;
using SPVCTD = Sucrose.Portal.Views.Controls.ThemeDelete;
using SPVCTE = Sucrose.Portal.Views.Controls.ThemeEdit;
using SPVCTR = Sucrose.Portal.Views.Controls.ThemeReview;
using SPVCTS = Sucrose.Portal.Views.Controls.ThemeShare;
using SRER = Sucrose.Resources.Extension.Resources;
using SSDECT = Sucrose.Shared.Dependency.Enum.CommandType;
using SSDEWT = Sucrose.Shared.Dependency.Enum.WallpaperType;
using SSLHK = Sucrose.Shared.Live.Helper.Kill;
using SSLHR = Sucrose.Shared.Live.Helper.Run;
using SSSHL = Sucrose.Shared.Space.Helper.Live;
using SSSHP = Sucrose.Shared.Space.Helper.Processor;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;
using SXAGAB = Sucrose.XamlAnimatedGif.AnimationBehavior;

namespace Sucrose.Portal.Views.Controls
{
    /// <summary>
    /// LibraryCard.xaml etkileşim mantığı
    /// </summary>
    public partial class LibraryCard : UserControl, IDisposable
    {
        private readonly string Theme = string.Empty;
        private readonly SPEIL Loader = new();
        private SSTHI Info = new();
        public bool Delete;

        internal LibraryCard(string Theme, SSTHI Info)
        {
            this.Info = Info;
            this.Theme = Theme;

            InitializeComponent();
        }

        private void Use()
        {
            if (Directory.Exists(Theme))
            {
                if ((!SMMB.ClosePerformance && !SMMB.PausePerformance) || !SSSHP.Work(SSSMI.Backgroundog))
                {
                    if (SMML.Selected != Path.GetFileName(Theme) || !SSSHL.Run())
                    {
                        SMMI.LibrarySettingManager.SetSetting(SMMCL.Selected, Path.GetFileName(Theme));

                        if (SSSHL.Run())
                        {
                            SSLHK.Stop();
                        }

                        SSLHR.Start();

                        Cursor = Cursors.Arrow;
                    }
                }
            }
        }

        private void UpdateInfo()
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
        }

        private void MenuUse_Click(object sender, RoutedEventArgs e)
        {
            if (Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
            {
                Use();
            }
        }

        private void MenuFind_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Theme))
            {
                SSSHP.Run(Theme);
            }
        }

        private void MenuPreview_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Theme))
            {
                return;
            }
        }

        private void MenuCustomize_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Theme))
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMMRG.StartCommand}{SSDECT.PropertyA}{SMMRG.ValueSeparator}{SSSMI.Property}{SMMRG.ValueSeparator}{Path.GetFileName(Theme)}");
            }
        }

        private void MenuCyclingAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> Exclusion = SMMC.Exclusion;

            if (Exclusion.Contains(Path.GetFileName(Theme)))
            {
                Exclusion.Remove(Path.GetFileName(Theme));

                SMMI.CyclingSettingManager.SetSetting(SMMCC.Exclusion, Exclusion);
            }
        }

        private async void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Theme))
            {
                SPVCTE ThemeEdit = new()
                {
                    Info = Info,
                    Theme = Theme
                };

                ContentDialogResult Result = await ThemeEdit.ShowAsync();

                if (Result == ContentDialogResult.Primary)
                {
                    Info = SSTHI.ReadJson(Path.Combine(Theme, SMMRC.SucroseInfo));

                    UpdateInfo();
                }

                ThemeEdit.Dispose();
            }
        }

        private async void MenuShare_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Theme))
            {
                SPVCTS ThemeShare = new()
                {
                    Info = Info,
                    Theme = Theme
                };

                await ThemeShare.ShowAsync();

                ThemeShare.Dispose();
            }
        }

        private async void MenuReview_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Theme))
            {
                SPVCTR ThemeReview = new()
                {
                    Info = Info,
                    Theme = Theme
                };

                await ThemeReview.ShowAsync();

                ThemeReview.Dispose();
            }
        }

        private async void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            bool Confirm = SMML.DeleteConfirm;

            ContentDialogResult Result = ContentDialogResult.None;

            if (Confirm)
            {
                SPVCTD ThemeDelete = new()
                {
                    Info = Info,
                    Theme = Theme
                };

                Result = await ThemeDelete.ShowAsync();

                ThemeDelete.Dispose();
            }

            if (!Confirm || Result == ContentDialogResult.Primary)
            {
                Dispose();

                MinWidth = 0;
                MinHeight = 0;

                Delete = true;

                Imagine.Source = null;
                Imaginer.Source = null;

                Visibility = Visibility.Hidden;

                await Task.Run(() =>
                {
                    string PropertiesCache = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.Properties);
                    string PropertiesFile = Path.Combine(PropertiesCache, $"{Path.GetFileName(Theme)}.json");

                    if (File.Exists(PropertiesFile))
                    {
                        File.Delete(PropertiesFile);
                    }
                });

                await Task.Run(() =>
                {
                    if (Directory.Exists(Theme))
                    {
                        Directory.Delete(Theme, true);
                    }
                });
            }
        }

        private void MenuCyclingRemove_Click(object sender, RoutedEventArgs e)
        {
            List<string> Exclusion = SMMC.Exclusion;

            if (!Exclusion.Contains(Path.GetFileName(Theme)))
            {
                Exclusion.Add(Path.GetFileName(Theme));

                SMMI.CyclingSettingManager.SetSetting(SMMCC.Exclusion, Exclusion);
            }
        }

        private void ThemeMore_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = true;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            MenuUse.Header = SRER.GetValue("Portal", "LibraryCard", "MenuUse");
            MenuDelete.Header = SRER.GetValue("Portal", "LibraryCard", "MenuDelete");
            MenuCustomize.Header = SRER.GetValue("Portal", "LibraryCard", "MenuCustomize");

            string PropertiesPath = Path.Combine(Theme, SMMRC.SucroseProperties);

            if ((Info.Type is SSDEWT.Gif or SSDEWT.Web or SSDEWT.Video) && (File.Exists(PropertiesPath) || File.Exists(Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.MpvPlayer, SMMRC.SucroseProperties))))
            {
                MenuCustomize.IsEnabled = true;
            }
            else
            {
                MenuCustomize.IsEnabled = false;
            }

            if (SMMC.Active)
            {
                if (SMMC.Exclusion.Contains(Path.GetFileName(Theme)))
                {
                    MenuCyclingAdd.Visibility = Visibility.Visible;
                    MenuCyclingRemove.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MenuCyclingAdd.Visibility = Visibility.Collapsed;
                    MenuCyclingRemove.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MenuCyclingAdd.Visibility = Visibility.Collapsed;
                MenuCyclingRemove.Visibility = Visibility.Collapsed;
            }

            if ((!SMMB.ClosePerformance && !SMMB.PausePerformance) || !SSSHP.Work(SSSMI.Backgroundog))
            {
                if (SMML.Selected == Path.GetFileName(Theme) && SSSHL.Run())
                {
                    MenuUse.IsEnabled = false;
                    MenuDelete.IsEnabled = false;

                    MenuUse.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Selected")})";
                    MenuDelete.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Selected")})";
                }
                else
                {
                    if (Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
                    {
                        MenuUse.IsEnabled = true;
                    }
                    else
                    {
                        MenuUse.IsEnabled = false;

                        MenuUse.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Incompatible")})";
                    }

                    MenuDelete.IsEnabled = true;
                }
            }
            else
            {
                MenuUse.IsEnabled = false;
                MenuDelete.IsEnabled = false;
                MenuCustomize.IsEnabled = false;

                if (SMMB.ClosePerformance)
                {
                    MenuUse.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Closed")})";
                    MenuDelete.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Closed")})";
                    MenuCustomize.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Closed")})";
                }
                else if (SMMB.PausePerformance)
                {
                    MenuUse.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Paused")})";
                    MenuDelete.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Paused")})";
                    MenuCustomize.Header += $" ({SRER.GetValue("Portal", "LibraryCard", "Paused")})";

                }
            }
        }

        private void Imaginer_MediaOpened(object sender, RoutedEventArgs e)
        {
            Imaginer.Visibility = Visibility.Visible;
            Imagine.Visibility = Visibility.Hidden;

            if (SMMP.LibraryPreviewHide)
            {
                Preview.Visibility = Visibility.Hidden;
            }

            Dispose();
        }

        private void LibraryCard_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SMMP.LibraryPreview)
            {
                Imaginer.Source = null;
                SXAGAB.SetSourceUri(Imaginer, null);

                Imagine.Visibility = Visibility.Visible;
                Imaginer.Visibility = Visibility.Hidden;

                if (SMMP.LibraryPreviewHide)
                {
                    Preview.Visibility = Visibility.Visible;
                }

                Dispose();
            }
        }

        private void LibraryCard_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((SMML.Selected == Path.GetFileName(Theme) && SSSHL.Run()) || Info.AppVersion.CompareTo(SHV.Entry()) > 0)
            {
                Cursor = Cursors.Arrow;
            }
            else
            {
                Cursor = Cursors.Hand;
            }

            if (SMMP.LibraryPreview)
            {
                string GifPath = Path.Combine(Theme, Info.Preview);

                if (File.Exists(GifPath))
                {
                    SXAGAB.SetSourceUri(Imaginer, new(GifPath));
                    SXAGAB.AddLoadedHandler(Imaginer, Imaginer_MediaOpened);
                }
            }
        }

        private async void LibraryCard_Loaded(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                UpdateInfo();

                if (Info.AppVersion.CompareTo(SHV.Entry()) > 0)
                {
                    ThemeMore.Visibility = Visibility.Collapsed;
                    IncompatibleVersion.Visibility = Visibility.Visible;
                }

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

                await Task.Delay(100);

                Card.Visibility = Visibility.Visible;
                Progress.Visibility = Visibility.Collapsed;

                Dispose();
            });
        }

        private void IncompatibleVersion_Click(object sender, RoutedEventArgs e)
        {
            if (!SSSHP.Work(SSSMI.Update))
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMMRG.StartCommand}{SSDECT.Update}{SMMRG.ValueSeparator}{SSSMI.Update}");
            }
        }

        private void LibraryCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
            {
                Use();
            }
        }

        public void Dispose()
        {
            Loader.Dispose();

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}