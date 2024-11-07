using System.Windows.Media;
using SSEMPMI = Sucrose.Shared.Engine.MpvPlayer.Manage.Internal;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;

namespace Sucrose.Shared.Engine.MpvPlayer.Helper
{
    internal static class Gif
    {
        public static async void Pause()
        {
            try
            {
                SSEMPMI.MediaEngine.Pause();
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void Play()
        {
            try
            {
                SSEMPMI.MediaEngine.Resume();
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void Stop()
        {
            try
            {
                SSEMPMI.MediaEngine.Stop();
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void SetLoop(bool State)
        {
            try
            {
                if (State && SSEMPMI.MediaEngine.Remaining <= TimeSpan.Zero)
                {
                    SSEMPMI.MediaEngine.Load(SSEMPMI.Source, true);
                }

                SSEMPMI.MediaEngine.Loop = State;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void SetSpeed(double Speed)
        {
            try
            {
                SSEMPMI.MediaEngine.Speed = Speed;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void SetStretch(Stretch Mode)
        {
            try
            {
                string KeepAspect = Mode != Stretch.Fill ? "yes" : "no";
                string VideoUnscaled = Mode != Stretch.None ? "no" : "yes";
                string Panscan = Mode == Stretch.UniformToFill ? "1.0" : "0.0";

                SSEMPMI.MediaEngine.API.SetPropertyString("panscan", Panscan);
                SSEMPMI.MediaEngine.API.SetPropertyString("keepaspect", KeepAspect);
                SSEMPMI.MediaEngine.API.SetPropertyString("video-unscaled", VideoUnscaled);

                /*
                    switch (Mode)
                    {
                        case Stretch.None:
                            SSEMPMI.MediaEngine.API.SetPropertyString("panscan", "0.0");
                            SSEMPMI.MediaEngine.API.SetPropertyString("keepaspect", "yes");
                            SSEMPMI.MediaEngine.API.SetPropertyString("video-unscaled", "yes");
                            break;
                        case Stretch.Fill:
                            SSEMPMI.MediaEngine.API.SetPropertyString("panscan", "0.0");
                            SSEMPMI.MediaEngine.API.SetPropertyString("keepaspect", "no");
                            SSEMPMI.MediaEngine.API.SetPropertyString("video-unscaled", "no");
                            break;
                        case Stretch.Uniform:
                            SSEMPMI.MediaEngine.API.SetPropertyString("panscan", "0.0");
                            SSEMPMI.MediaEngine.API.SetPropertyString("keepaspect", "yes");
                            SSEMPMI.MediaEngine.API.SetPropertyString("video-unscaled", "no");
                            break;
                        case Stretch.UniformToFill:
                            SSEMPMI.MediaEngine.API.SetPropertyString("panscan", "1.0");
                            SSEMPMI.MediaEngine.API.SetPropertyString("keepaspect", "yes");
                            SSEMPMI.MediaEngine.API.SetPropertyString("video-unscaled", "no");
                            break;
                        default:
                            SSEMPMI.MediaEngine.API.SetPropertyString("panscan", "0.0");
                            SSEMPMI.MediaEngine.API.SetPropertyString("keepaspect", "no");
                            SSEMPMI.MediaEngine.API.SetPropertyString("video-unscaled", "no");
                            break;
                    }
                */
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }
    }
}