﻿using System.IO;
using System.Windows;
using Wpf.Ui.Controls;
using SECNT = Skylark.Enum.ClearNumericType;
using SEMST = Skylark.Enum.ModeStorageType;
using SEST = Skylark.Enum.StorageType;
using SHC = Skylark.Helper.Culture;
using SHN = Skylark.Helper.Numeric;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SPEIL = Sucrose.Portal.Extension.ImageLoader;
using SPMI = Sucrose.Portal.Manage.Internal;
using SRER = Sucrose.Resources.Extension.Resources;
using SSESSE = Skylark.Standard.Extension.Storage.StorageExtension;
using SSSHS = Sucrose.Shared.Space.Helper.Size;
using SSSHT = Sucrose.Shared.Space.Helper.Tags;
using SSSSS = Skylark.Struct.Storage.StorageStruct;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSTHV = Sucrose.Shared.Theme.Helper.Various;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;

namespace Sucrose.Portal.Views.Controls
{
    /// <summary>
    /// ThemeReview.xaml etkileşim mantığı
    /// </summary>
    public partial class ThemeReview : ContentDialog, IDisposable
    {
        private readonly SPEIL Loader = new();
        internal string Theme = string.Empty;
        internal SSTHI Info = new();

        public ThemeReview() : base(SPMI.ContentDialogService.GetDialogHost())
        {
            InitializeComponent();
        }

        private string Size(string Path)
        {
            SSSSS Data = SSESSE.AutoConvert(SSSHS.Calc(Path), SEST.Byte, SEMST.Palila);

            return $"{SHN.Numeral(Data.Value, true, true, 1, '0', SECNT.None)} {Data.Short}";
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeTitle.Text = Info.Title;
            ThemeDescription.Text = Info.Description;

            ThemeTypeName.Text = Info.Type.ToString();

            ThemeAuthorName.Text = Info.Author;

            ThemeSizeTotal.Text = Size(Theme);

            ThemeVersionText.Text = $"{Info.Version} ({Info.AppVersion})";

            ThemeTagsText.Text = SSSHT.Join(Info.Tags, ", ", false, SRER.GetValue("Portal", "ThemeReview", "ThemeTagsText"));

            DateTime CreationTime = Directory.GetCreationTime(Theme);

            ThemeCreateDate.Text = CreationTime.ToString(SHC.CurrentUI);

            DateTime ModificationTime = Directory.GetLastWriteTime(Path.Combine(Theme, SMMRC.SucroseInfo));

            ThemeModifyDate.Text = ModificationTime.ToString(SHC.CurrentUI);

            if (string.IsNullOrEmpty(Info.Contact))
            {
                ThemeContact.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (SSTHV.IsMail(Info.Contact))
                {
                    ThemeContact.NavigateUri = $"mailto:{Info.Contact}";
                }
                else
                {
                    ThemeContact.NavigateUri = Info.Contact;
                }
            }

            string ImagePath = Path.Combine(Theme, Info.Thumbnail);

            if (File.Exists(ImagePath))
            {
                try
                {
                    ThemeThumbnail.Source = Loader.LoadOptimal(ImagePath, true, 600);
                }
                catch (Exception Exception)
                {
                    await SSWEW.Watch_CatchException(Exception);
                }
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}