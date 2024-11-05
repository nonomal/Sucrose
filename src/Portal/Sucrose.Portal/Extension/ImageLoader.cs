using System.IO;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using SPMI = Sucrose.Portal.Manage.Internal;

namespace Sucrose.Portal.Extension
{
    internal class ImageLoader : IDisposable
    {
        private string ImagePath { get; set; }

        public BitmapImage Load(string ImagePath, bool Decode = true, int Width = 360)
        {
            this.ImagePath = ImagePath;

            if (!SPMI.Images.ContainsKey(ImagePath))
            {
                SPMI.Images.Add(ImagePath, new()
                {
                    UriCachePolicy = new(RequestCacheLevel.NoCacheNoStore),
                    CreateOptions = BitmapCreateOptions.IgnoreImageCache,
                    CacheOption = BitmapCacheOption.None
                });

                if (!SPMI.ImageStream.ContainsKey(ImagePath))
                {
                    SPMI.ImageStream[ImagePath] = new(ImagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                }

                SPMI.Images[ImagePath].BeginInit();

                SPMI.Images[ImagePath].StreamSource = SPMI.ImageStream[ImagePath];

                if (Decode)
                {
                    //SPMI.Images[ImagePath].DecodePixelHeight = 160;
                    SPMI.Images[ImagePath].DecodePixelWidth = Width;
                }

                SPMI.Images[ImagePath].EndInit();
                SPMI.Images[ImagePath].Freeze();
            }

            return SPMI.Images[ImagePath];
        }

        public async Task<BitmapImage> LoadAsync(string ImagePath, bool Decode = true, int Width = 360)
        {
            return await Task.Run(() => Load(ImagePath, Decode, Width));
        }

        public BitmapImage LoadOptimal(string ImagePath, bool Decode = true, int Width = 360)
        {
            BitmapImage Image = new();

            using FileStream Stream = new(ImagePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            Image.BeginInit();

            Image.UriCachePolicy = new(RequestCacheLevel.BypassCache);
            Image.CacheOption = BitmapCacheOption.OnLoad;

            if (Decode)
            {
                //Image.DecodePixelHeight = 160;
                Image.DecodePixelWidth = Width;
            }

            Image.StreamSource = Stream;

            Image.EndInit();

            Image.Freeze();

            return Image;
        }

        public async Task<BitmapImage> LoadOptimalAsync(string ImagePath, bool Decode = true, int Width = 360)
        {
            return await Task.Run(() => LoadOptimal(ImagePath, Decode, Width));
        }

        public void Remove(string ImagePath)
        {
            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (SPMI.Images.ContainsKey(ImagePath))
                {
                    SPMI.Images[ImagePath].StreamSource?.Dispose();
                    SPMI.Images.Remove(ImagePath);
                }

                if (SPMI.ImageStream.ContainsKey(ImagePath))
                {
                    SPMI.ImageStream[ImagePath].Dispose();
                    SPMI.ImageStream.Remove(ImagePath);
                }
            }
        }

        public async Task RemoveAsync(string ImagePath)
        {
            await Task.Run(() => Remove(ImagePath));
        }

        public void Clear()
        {
            foreach (FileStream Stream in SPMI.ImageStream.Values)
            {
                Stream.Dispose();
            }

            SPMI.ImageStream.Clear();

            foreach (BitmapImage Image in SPMI.Images.Values)
            {
                Image.StreamSource?.Dispose();
            }

            SPMI.Images.Clear();
        }

        public async Task ClearAsync()
        {
            await Task.Run(Clear);
        }

        public void Dispose()
        {
            Clear();

            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await ClearAsync();

            GC.SuppressFinalize(this);
        }
    }
}