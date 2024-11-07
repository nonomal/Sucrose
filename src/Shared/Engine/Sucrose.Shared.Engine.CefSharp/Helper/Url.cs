﻿using SSECSHM = Sucrose.Shared.Engine.CefSharp.Helper.Management;
using SSECSMI = Sucrose.Shared.Engine.CefSharp.Manage.Internal;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;
using SWEACAM = Skylark.Wing.Extension.AudioController.AudioManager;
using SWEVPCAM = Skylark.Wing.Extension.VideoPlayerController.AudioManager;
using SWNM = Skylark.Wing.Native.Methods;

namespace Sucrose.Shared.Engine.CefSharp.Helper
{
    internal static class Url
    {
        public static async void Play()
        {
            try
            {
                if (!SSECSMI.State)
                {
                    SSECSMI.State = true;

                    //SSECSMI.CefEngine.Address = SSECSMI.Url;

                    if (SSEMI.Processes.Any())
                    {
                        foreach (int Process in SSEMI.Processes.ToList())
                        {
                            _ = SWNM.DebugActiveProcessStop((uint)Process);
                        }
                    }

                    //if (SSEMI.IntermediateD3DWindow > 0)
                    //{
                    //    _ = SWNM.DebugActiveProcessStop((uint)SSEMI.IntermediateD3DWindow);
                    //}
                }
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
                if (SSECSMI.State)
                {
                    SSECSMI.State = false;

                    //string Path = SSEHS.GetImageContentPath();

                    //SSEHS.WriteImageContent(Path, SSECSES.Capture());

                    //SSECSMI.CefEngine.Address = SSEHS.GetSource(Path).ToString();

                    if (SSEMI.Processes.Any())
                    {
                        foreach (int Process in SSEMI.Processes.ToList())
                        {
                            _ = SWNM.DebugActiveProcess((uint)Process);
                        }
                    }

                    //if (SSEMI.IntermediateD3DWindow > 0)
                    //{
                    //    _ = SWNM.DebugActiveProcess((uint)SSEMI.IntermediateD3DWindow);
                    //}
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
                if (SSEMI.Processes.Any())
                {
                    foreach (int Process in SSEMI.Processes.ToList())
                    {
                        try
                        {
                            SWEVPCAM.SetApplicationVolume(Process, Volume);
                        }
                        catch
                        {
                            try
                            {
                                SWEACAM.SetApplicationVolume(Process, Volume);
                            }
                            catch { }
                        }
                    }
                }

                if (SSECSMI.Try < 3)
                {
                    await Task.Run(() =>
                    {
                        SSECSMI.Try++;
                        SSECSHM.SetProcesses();
                    });
                }
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }
    }
}