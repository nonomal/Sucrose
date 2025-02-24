﻿using Microsoft.Win32;
using System.Windows;
using SMMB = Sucrose.Manager.Manage.Backgroundog;
using SSEEH = Sucrose.Shared.Engine.Event.Handler;
using SSEHD = Sucrose.Shared.Engine.Helper.Data;
using SSEHR = Sucrose.Shared.Engine.Helper.Run;
using SSEHV = Sucrose.Shared.Engine.Helper.Volume;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSEWVEYT = Sucrose.Shared.Engine.WebView.Event.YouTube;
using SSEWVHYT = Sucrose.Shared.Engine.WebView.Helper.YouTube;
using SSEWVMI = Sucrose.Shared.Engine.WebView.Manage.Internal;

namespace Sucrose.Shared.Engine.WebView.View
{
    /// <summary>
    /// Interaction logic for YouTube.xaml
    /// </summary>
    public sealed partial class YouTube : Window, IDisposable
    {
        public YouTube()
        {
            InitializeComponent();

            SystemEvents.DisplaySettingsChanged += (s, e) => SSEEH.DisplaySettingsChanged(this);

            ContentRendered += (s, e) => SSEEH.ContentRendered(this);

            Content = SSEWVMI.WebEngine;

            SSEMI.GeneralTimer.Tick += new EventHandler(GeneralTimer_Tick);
            SSEMI.GeneralTimer.Interval = new TimeSpan(0, 0, 1);
            SSEMI.GeneralTimer.Start();

            SSEWVMI.WebEngine.CoreWebView2InitializationCompleted += SSEWVEYT.WebEngineInitializationCompleted;

            Closing += (s, e) => SSEWVMI.WebEngine.Dispose();
            Loaded += (s, e) => SSEEH.WindowLoaded(this);

            SSEHV.Start();
        }

        private void GeneralTimer_Tick(object sender, EventArgs e)
        {
            if (SSEMI.Initialized)
            {
                Dispose();

                SSEHR.Control();

                SSEWVHYT.First();

                SSEWVHYT.SetLoop(SSEHD.GetLoop());

                SSEWVHYT.SetVolume(SSEHD.GetVolume());

                SSEWVHYT.SetShuffle(SSEHD.GetShuffle());

                if (SMMB.PausePerformance)
                {
                    SSEWVHYT.Pause();

                    SSEMI.PausePerformance = true;
                }
                else if (SSEMI.PausePerformance)
                {
                    SSEWVHYT.Play();

                    SSEMI.PausePerformance = false;
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