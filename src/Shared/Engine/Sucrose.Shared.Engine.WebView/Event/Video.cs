﻿using Microsoft.Web.WebView2.Core;
using System.Collections;
using SELLT = Skylark.Enum.LevelLogType;
using SMME = Sucrose.Manager.Manage.Engine;
using SMMG = Sucrose.Manager.Manage.General;
using SMMI = Sucrose.Manager.Manage.Internal;
using SSEHS = Sucrose.Shared.Engine.Helper.Source;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSEWVHV = Sucrose.Shared.Engine.WebView.Helper.Video;
using SSEWVMI = Sucrose.Shared.Engine.WebView.Manage.Internal;
using SSWHD = Sucrose.Shared.Watchdog.Helper.Dataset;

namespace Sucrose.Shared.Engine.WebView.Event
{
    internal static class Video
    {
        public static void WebEngineProcessFailed(object sender, CoreWebView2ProcessFailedEventArgs e)
        {
            SSWHD.Add("WebEngine Process Failed", new Hashtable()
            {
                { "Exit Code", e.ExitCode },
                { "Reason", $"{e.Reason}" },
                { "Process Description", e.ProcessDescription },
                { "Process Failed Kind", $"{e.ProcessFailedKind}" },
                { "Failure Source Module Path", e.FailureSourceModulePath },
                { "Frame Infos For Failed Process", e.FrameInfosForFailedProcess }
            });

            SMMI.WebViewLiveLogManager.Log(SELLT.Fatal, $"Reason: {e.Reason}");
            SMMI.WebViewLiveLogManager.Log(SELLT.Fatal, $"Exit Code: {e.ExitCode}");
            SMMI.WebViewLiveLogManager.Log(SELLT.Fatal, $"Process Failed Kind: {e.ProcessFailedKind}");
            SMMI.WebViewLiveLogManager.Log(SELLT.Fatal, $"Process Description: {e.ProcessDescription}");
            SMMI.WebViewLiveLogManager.Log(SELLT.Fatal, $"Failure Source Module Path: {e.FailureSourceModulePath}");

            if (e.FrameInfosForFailedProcess != null && e.FrameInfosForFailedProcess.Any())
            {
                foreach (CoreWebView2FrameInfo FrameInfo in e.FrameInfosForFailedProcess)
                {
                    SMMI.WebViewLiveLogManager.Log(SELLT.Fatal, $"Failed Process; Frame ID: {FrameInfo.FrameId}, Frame Kind: {FrameInfo.FrameKind}, Name: {FrameInfo.Name}, Source: {FrameInfo.Source}");
                }
            }
        }

        public static void WebEngineDOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            SSEWVHV.Load();

            SSEMI.Initialized = true;
        }

        public static void WebEngineInitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            SSEWVMI.WebEngine.CoreWebView2.ServerCertificateErrorDetected += WebEngineServerCertificateErrorDetected;

            SSEWVMI.WebEngine.CoreWebView2.ProcessFailed += WebEngineProcessFailed;

            SSEWVMI.WebEngine.CoreWebView2.Settings.UserAgent = SMMG.UserAgent;

            Uri Video = SSEHS.GetSource(SSEMI.Info.Source, SSEMI.Host);

            if (SSEHS.GetExtension(Video))
            {
                SSEWVMI.WebEngine.Source = Video;
            }
            else
            {
                string Path = SSEHS.GetVideoContentPath();

                SSEHS.WriteVideoContent(Path, Video);

                SSEWVMI.WebEngine.Source = SSEHS.GetSource(Path);
            }

            SSEWVMI.WebEngine.CoreWebView2.DOMContentLoaded += WebEngineDOMContentLoaded;

            if (SMME.DeveloperMode)
            {
                SSEWVMI.WebEngine.CoreWebView2.OpenDevToolsWindow();
            }
        }

        public static void WebEngineServerCertificateErrorDetected(object sender, CoreWebView2ServerCertificateErrorDetectedEventArgs e)
        {
            CoreWebView2Certificate Certificate = e.ServerCertificate;

            e.Action = CoreWebView2ServerCertificateErrorAction.AlwaysAllow;
        }
    }
}