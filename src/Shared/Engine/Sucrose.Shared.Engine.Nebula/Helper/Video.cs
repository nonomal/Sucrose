using SSENMI = Sucrose.Shared.Engine.Nebula.Manage.Internal;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;

namespace Sucrose.Shared.Engine.Nebula.Helper
{
    internal static class Video
    {
        public static async void Pause()
        {
            try
            {
                SSENMI.MediaEngine.Pause();
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
                SSENMI.MediaEngine.Play();
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
                SSENMI.MediaEngine.Stop();
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
                if (State && (!SSENMI.MediaEngine.NaturalDuration.HasTimeSpan || SSENMI.MediaEngine.Position >= SSENMI.MediaEngine.NaturalDuration.TimeSpan))
                {
                    SSENMI.MediaEngine.Position = TimeSpan.Zero;
                    SSENMI.MediaEngine.Play();
                }
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void SetVolume(int Volume)
        {
            try
            {
                SSENMI.MediaEngine.Volume = (double)Volume / 100;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }
    }
}