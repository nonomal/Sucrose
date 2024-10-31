using Microsoft.Win32;
using System.Diagnostics;
using SBMI = Sucrose.Backgroundog.Manage.Internal;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;
using SWNM = Skylark.Wing.Native.Methods;

namespace Sucrose.Backgroundog.Extension
{
    internal static class Windows
    {
        public static async void Stop()
        {
            try
            {
                SystemEvents.SessionSwitch += SessionSwitch;
                SystemEvents.PowerModeChanged += PowerModeChanged;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void Start()
        {
            try
            {
                SBMI.WindowsLock = IsSystemLocked();

                SystemEvents.SessionSwitch -= SessionSwitch;
                SystemEvents.PowerModeChanged -= PowerModeChanged;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        private static bool IsSystemLocked()
        {
            try
            {
                SWNM.GetWindowThreadProcessId(SWNM.GetForegroundWindow(), out int PID);

                Process Process = Process.GetProcessById(PID);

                return Process.ProcessName.Equals("LockApp", StringComparison.OrdinalIgnoreCase);
            }
            catch { }

            return false;
        }

        private static async void SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            try
            {
                switch (e.Reason)
                {
                    case SessionSwitchReason.SessionLock:
                        SBMI.WindowsLock = true;
                        break;
                    case SessionSwitchReason.SessionLogon:
                        SBMI.WindowsSession = true;
                        break;
                    case SessionSwitchReason.RemoteConnect:
                        SBMI.WindowsRemote = true;
                        break;
                    case SessionSwitchReason.SessionLogoff:
                        SBMI.WindowsSession = false;
                        break;
                    case SessionSwitchReason.SessionUnlock:
                        SBMI.WindowsLock = false;
                        break;
                    case SessionSwitchReason.ConsoleConnect:
                        SBMI.WindowsConsole = true;
                        break;
                    case SessionSwitchReason.RemoteDisconnect:
                        SBMI.WindowsRemote = false;
                        break;
                    case SessionSwitchReason.ConsoleDisconnect:
                        SBMI.WindowsConsole = false;
                        break;
                    case SessionSwitchReason.SessionRemoteControl:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        private static async void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            try
            {
                switch (e.Mode)
                {
                    case PowerModes.Resume:
                        SBMI.WindowsSleep = false;
                        break;
                    case PowerModes.Suspend:
                        SBMI.WindowsSleep = true;
                        break;
                    case PowerModes.StatusChange:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }
    }
}