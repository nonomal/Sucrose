﻿using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;
using SMMB = Sucrose.Manager.Manage.Backgroundog;
using SSEEH = Sucrose.Shared.Engine.Event.Handler;
using SSEHD = Sucrose.Shared.Engine.Helper.Data;
using SSEHR = Sucrose.Shared.Engine.Helper.Run;
using SSEHS = Sucrose.Shared.Engine.Helper.Source;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSEMPEG = Sucrose.Shared.Engine.MpvPlayer.Event.Gif;
using SSEMPHG = Sucrose.Shared.Engine.MpvPlayer.Helper.Gif;
using SSEMPMI = Sucrose.Shared.Engine.MpvPlayer.Manage.Internal;

namespace Sucrose.Shared.Engine.MpvPlayer.View
{
    /// <summary>
    /// Interaction logic for Gif.xaml
    /// </summary>
    public sealed partial class Gif : Window, IDisposable
    {
        public Gif()
        {
            InitializeComponent();

            SystemEvents.DisplaySettingsChanged += (s, e) => SSEEH.DisplaySettingsChanged(this);

            SSEMPMI.Source = SSEHS.GetSource(SSEMI.Info.Source, SSEMI.Host).ToString();

            ContentRendered += (s, e) => SSEEH.ContentRendered(this);

            SSEMPMI.MediaEngine = new(PlayerHost.Handle, SSEMPMI.MediaPath)
            {
                AutoPlay = true,
                Loop = SSEHD.GetLoop()
            };

            SSEMPMI.MediaEngine.MediaLoaded += SSEMPEG.MediaEngineMediaLoaded;

            SSEMPMI.MediaEngine.LoadConfig(SSEMPMI.MpvConfig);

            SSEMPMI.MediaEngine.Load(SSEMPMI.Source);

            SSEMI.GeneralTimer.Tick += new EventHandler(GeneralTimer_Tick);
            SSEMI.GeneralTimer.Interval = new TimeSpan(0, 0, 1);
            SSEMI.GeneralTimer.Start();

            Closing += (s, e) => SSEMPMI.MediaEngine.Dispose();
            Loaded += (s, e) => SSEEH.WindowLoaded(this);
        }

        private void GeneralTimer_Tick(object sender, EventArgs e)
        {
            Dispose();

            SSEHR.Control();

            SSEMPHG.SetLoop(SSEHD.GetLoop());

            SSEMPHG.SetStretch((Stretch)SSEHD.GetStretch());

            if (SMMB.PausePerformance)
            {
                SSEMPHG.Pause();

                SSEMI.PausePerformance = true;
            }
            else if (SSEMI.PausePerformance)
            {
                SSEMPHG.Play();

                SSEMI.PausePerformance = false;
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}