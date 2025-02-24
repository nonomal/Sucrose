﻿using HandyControl.Tools;
using System.Globalization;
using System.IO;
using System.Windows;
using Application = System.Windows.Application;
using SHC = Skylark.Helper.Culture;
using SMMG = Sucrose.Manager.Manage.General;
using SMMI = Sucrose.Manager.Manage.Internal;
using SMML = Sucrose.Manager.Manage.Library;
using SMMRA = Sucrose.Memory.Manage.Readonly.App;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SMMRF = Sucrose.Memory.Manage.Readonly.Folder;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRM = Sucrose.Memory.Manage.Readonly.Mutex;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SPMI = Sucrose.Property.Manage.Internal;
using SPVMW = Sucrose.Property.View.MainWindow;
using SRHR = Sucrose.Resources.Helper.Resources;
using SSDEWT = Sucrose.Shared.Dependency.Enum.WallpaperType;
using SSSHI = Sucrose.Shared.Space.Helper.Instance;
using SSSHW = Sucrose.Shared.Space.Helper.Watchdog;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSTHP = Sucrose.Shared.Theme.Helper.Properties;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;

namespace Sucrose.Property
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static bool HasError { get; set; } = true;

        public App()
        {
            AppDomain.CurrentDomain.FirstChanceException += async (s, e) =>
            {
                Exception Exception = e.Exception;

                await SSWEW.Watch_FirstChanceException(Exception);

                //Close();
                //Message(Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += async (s, e) =>
            {
                Exception Exception = (Exception)e.ExceptionObject;

                await SSWEW.Watch_GlobalUnhandledException(Exception);

                //Close();
                Message(Exception);
            };

            TaskScheduler.UnobservedTaskException += async (s, e) =>
            {
                Exception Exception = e.Exception;

                await SSWEW.Watch_UnobservedTaskException(Exception);

                e.SetObserved();

                //Close();
                Message(Exception);
            };

            Current.DispatcherUnhandledException += async (s, e) =>
            {
                Exception Exception = e.Exception;

                await SSWEW.Watch_DispatcherUnhandledException(Exception);

                e.Handled = true;

                //Close();
                Message(Exception);
            };

            ConfigHelper.Instance.SetLang(SMMG.Culture);
            SHC.All = new CultureInfo(SMMG.Culture, true);
        }

        protected void Close()
        {
            Environment.Exit(0);
            Current.Shutdown();
            Shutdown();
        }

        protected void Message(Exception Exception)
        {
            if (HasError)
            {
                HasError = false;

                string Path = SMMI.PropertyLogManager.LogFile();

                SSSHW.Start(SMMRA.Property, Exception, Path);

                Close();
            }
        }

        protected void Configure(string[] Args)
        {
            SPMI.LibraryLocation = SMML.Location;
            SPMI.LibrarySelected = SMML.Selected;

            if (Args.Any())
            {
                string[] Arguments = Args.First().Split(SMMRG.ValueSeparatorChar);

                if (Arguments.Any() && Arguments.Count() == 1)
                {
                    SPMI.LibrarySelected = Arguments.First();
                }
            }

            if (SMMI.LibrarySettingManager.CheckFile() && !string.IsNullOrEmpty(SPMI.LibrarySelected))
            {
                SPMI.Path = Path.Combine(SPMI.LibraryLocation, SPMI.LibrarySelected);

                SPMI.PropertiesPath = Path.Combine(SPMI.Path, SMMRC.SucroseProperties);

                if (!File.Exists(SPMI.PropertiesPath))
                {
                    SPMI.PropertiesPath = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.MpvPlayer, SMMRC.SucroseProperties);
                }

                if (File.Exists(SPMI.PropertiesPath))
                {
                    SPMI.InfoPath = Path.Combine(SPMI.Path, SMMRC.SucroseInfo);

                    if (File.Exists(SPMI.InfoPath))
                    {
                        SPMI.Info = SSTHI.ReadJson(SPMI.InfoPath);

                        if (SPMI.Info.Type is SSDEWT.Gif or SSDEWT.Web or SSDEWT.Video)
                        {
                            SPMI.PropertiesCache = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.Properties);
                            SPMI.PropertiesFile = Path.Combine(SPMI.PropertiesCache, $"{SPMI.LibrarySelected}.json");
                            SPMI.WatcherFile = Path.Combine(SPMI.PropertiesCache, $"*.{SPMI.LibrarySelected}.json");

                            if (!Directory.Exists(SPMI.PropertiesCache))
                            {
                                Directory.CreateDirectory(SPMI.PropertiesCache);
                            }

                            if (!File.Exists(SPMI.PropertiesFile))
                            {
                                File.Copy(SPMI.PropertiesPath, SPMI.PropertiesFile, true);
                            }

                            try
                            {
                                SPMI.Properties = SSTHP.ReadJson(SPMI.PropertiesFile);
                            }
                            catch (NotSupportedException Ex)
                            {
                                File.Delete(SPMI.PropertiesFile);

                                throw new NotSupportedException(Ex.Message);
                            }
                            catch (Exception Ex)
                            {
                                File.Delete(SPMI.PropertiesFile);

                                throw new Exception(Ex.Message, Ex.InnerException);
                            }

                            SPVMW MainWindow = new();
                            MainWindow.ShowDialog();
                        }
                    }
                }
            }

            Close();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            //

            Close();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SRHR.SetLanguage(SMMG.Culture);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (SSSHI.Basic(SMMRM.Property, SMMRA.Property))
            {
                Configure(e.Args);
            }
            else
            {
                Close();
            }
        }
    }
}