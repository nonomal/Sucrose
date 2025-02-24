﻿using CefSharp;
using System.IO;
using System.Windows;
using Application = System.Windows.Application;
using SEIT = Skylark.Enum.InputType;
using SMME = Sucrose.Manager.Manage.Engine;
using SSECSEI = Sucrose.Shared.Engine.CefSharp.Extension.Interaction;
using SSECSHH = Sucrose.Shared.Engine.CefSharp.Helper.Handle;
using SSECSHW = Sucrose.Shared.Engine.CefSharp.Helper.Web;
using SSECSMI = Sucrose.Shared.Engine.CefSharp.Manage.Internal;
using SSEHP = Sucrose.Shared.Engine.Helper.Properties;
using SSEHS = Sucrose.Shared.Engine.Helper.Source;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSTHP = Sucrose.Shared.Theme.Helper.Properties;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;

namespace Sucrose.Shared.Engine.CefSharp.Event
{
    internal static class Web
    {
        public static void CefEngineLoaded(object sender, RoutedEventArgs e)
        {
            SSECSMI.CefEngine.Address = SSEHS.GetSource(SSEMI.Info.Source, SSEMI.Host).ToString();
        }

        public static void CefEngineInitializedChanged(object sender, EventArgs e)
        {
            if (SMME.DeveloperMode)
            {
                SSECSMI.CefEngine.ShowDevTools();
            }

            SSECSHW.StartCompatible();

            SSEMI.Initialized = SSECSMI.CefEngine.IsBrowserInitialized;
        }

        public static void CefEngineFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (!string.IsNullOrEmpty(SSEMI.PropertiesFile))
            {
                SSEMI.Properties = SSTHP.ReadJson(SSEMI.PropertiesFile);
                SSEMI.Properties.State = true;
            }

            SSECSHH.GetInputHandle();

            SSECSHH.GetIntermediateHandle();

            if (SMME.InputType != SEIT.Close)
            {
                SSECSEI.Register();
            }

            if (SSEMI.Properties.State)
            {
                if (SSEMI.PropertiesWatcher)
                {
                    SSEHP.CreatedEventHandler += PropertiesWatcher;
                }

                SSEHP.StartWatcher();

                SSEHP.ExecuteNormal(SSECSMI.CefEngine.ExecuteScriptAsync);
            }
        }

        private static async void PropertiesWatcher(object sender, FileSystemEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    SSEMI.Properties = SSTHP.ReadJson(e.FullPath);

                    if (!SSECSMI.CefEngine.IsDisposed && SSECSMI.CefEngine.IsInitialized && SSECSMI.CefEngine.CanExecuteJavascriptInMainFrame)
                    {
                        SSEHP.ExecuteNormal(SSECSMI.CefEngine.ExecuteScriptAsync);
                    }
                }
                catch (Exception Exception)
                {
                    await SSWEW.Watch_CatchException(Exception);
                }
            });
        }
    }
}