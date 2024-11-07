using System.Windows.Media.Animation;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSEXMI = Sucrose.Shared.Engine.Xavier.Manage.Internal;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;
using SXAGAB = Sucrose.XamlAnimatedGif.AnimationBehavior;

namespace Sucrose.Shared.Engine.Xavier.Helper
{
    internal static class Gif
    {
        public static async void Play()
        {
            try
            {
                SXAGAB.GetAnimator(SSEXMI.ImageEngine).Play();
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void Pause()
        {
            try
            {
                SXAGAB.GetAnimator(SSEXMI.ImageEngine).Pause();
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void Resume()
        {
            try
            {
                SXAGAB.GetAnimator(SSEXMI.ImageEngine).Resume();
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void Rewind()
        {
            try
            {
                SXAGAB.GetAnimator(SSEXMI.ImageEngine).Rewind();
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
                if (State)
                {
                    if (SXAGAB.GetRepeatBehavior(SSEXMI.ImageEngine) != RepeatBehavior.Forever && !SSEMI.PausePerformance)
                    {
                        SXAGAB.SetRepeatBehavior(SSEXMI.ImageEngine, RepeatBehavior.Forever);

                        Uri Source = SXAGAB.GetSourceUri(SSEXMI.ImageEngine);

                        SSEXMI.ImageEngine.Source = null;
                        SXAGAB.SetSourceUri(SSEXMI.ImageEngine, null);

                        SXAGAB.SetSourceUri(SSEXMI.ImageEngine, Source);
                    }
                }
                else
                {
                    SXAGAB.SetRepeatBehavior(SSEXMI.ImageEngine, new RepeatBehavior(1));
                }
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void SetMemory(bool State)
        {
            try
            {
                if (SXAGAB.GetCacheFramesInMemory(SSEXMI.ImageEngine) != State)
                {
                    SXAGAB.SetCacheFramesInMemory(SSEXMI.ImageEngine, State);
                }
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }
    }
}