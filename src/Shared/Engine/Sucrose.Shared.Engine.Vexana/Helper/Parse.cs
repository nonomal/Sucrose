﻿using System.Drawing.Imaging;
using System.IO;
using SMMRF = Sucrose.Memory.Manage.Readonly.Folder;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SSEVMI = Sucrose.Shared.Engine.Vexana.Manage.Internal;
using SSEVSG = Sucrose.Shared.Engine.Vexana.Struct.Gif;

namespace Sucrose.Shared.Engine.Vexana.Helper
{
    internal static class Parse
    {
        public static SSEVSG Gif(string GifPath)
        {
            SSEVSG Result = SSEVMI.ImageResult;

            string CachePath = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.Gif);

            if (Directory.Exists(CachePath))
            {
                string[] Files = Directory.GetFiles(CachePath);

                foreach (string Record in Files)
                {
                    File.Delete(Record);
                }
            }
            else
            {
                Directory.CreateDirectory(CachePath);
            }

            using (Image GifImage = Image.FromFile(GifPath.Replace("file:///", "")))
            {
                FrameDimension Dimension = new(GifImage.FrameDimensionsList[0]);

                int FrameCount = GifImage.GetFrameCount(Dimension);
                int[] FrameDurations = new int[FrameCount];

                for (int FrameIndex = 0; FrameIndex < FrameCount; FrameIndex++)
                {
                    GifImage.SelectActiveFrame(Dimension, FrameIndex);

                    PropertyItem FrameDelayItem = GifImage.PropertyItems.FirstOrDefault(pi => pi.Id == 0x5100);

                    int FrameDelay = 100;

                    if (FrameDelayItem != null)
                    {
                        FrameDelay = BitConverter.ToInt32(FrameDelayItem.Value, FrameIndex * 4) * 10;
                    }

                    FrameDurations[FrameIndex] = FrameDelay;

                    using Image Frame = new Bitmap(GifImage.Width, GifImage.Height);

                    using (Graphics Graphic = Graphics.FromImage(Frame))
                    {
                        Graphic.DrawImage(GifImage, Point.Empty);
                    }

                    int FrameTime = (int)TimeSpan.FromMilliseconds(FrameDurations[FrameIndex]).TotalMilliseconds;

                    //Result.Total += FrameTime;

                    //Result.Min = Math.Min(Result.Min, FrameTime);
                    //Result.Max = Math.Max(Result.Max, FrameTime);

                    string OutputGifImage = Path.Combine(CachePath, $"frame_{FrameIndex}_{FrameTime}.png");

                    Result.List.Add(OutputGifImage);

                    Frame.Save(OutputGifImage, ImageFormat.Png);
                }

                //File.Create(Path.Combine(CachePath, $"min_{Result.Min}"));
                //File.Create(Path.Combine(CachePath, $"max_{Result.Max}"));
                //File.Create(Path.Combine(CachePath, $"total_{Result.Total}"));
            }

            return Result;
        }
    }
}