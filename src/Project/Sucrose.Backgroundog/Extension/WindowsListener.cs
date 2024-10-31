using HRESULT = Skylark.Wing.Native.Methods.HRESULT;
using SBMI = Sucrose.Backgroundog.Manage.Internal;
using SWIIEL = Skylark.Wing.Interface.IEventListener;

namespace Sucrose.Backgroundog.Extension
{
    internal class WindowsListener : SWIIEL
    {
        public HRESULT OnLogon(string userName)
        {
            SBMI.WindowsSession = true;
            return HRESULT.S_OK;
        }

        public HRESULT OnLogoff(string userName)
        {
            SBMI.WindowsSession = false;
            return HRESULT.S_OK;
        }

        public HRESULT OnStartShell(string userName)
        {
            //Console.WriteLine($"{userName} started shell");
            return HRESULT.S_OK;
        }

        public HRESULT OnDisplayLock(string userName)
        {
            SBMI.WindowsLock = true;
            return HRESULT.S_OK;
        }

        public HRESULT OnDisplayUnlock(string userName)
        {
            SBMI.WindowsLock = false;
            return HRESULT.S_OK;
        }

        public HRESULT OnStopScreenSaver(string userName)
        {
            SBMI.WindowsScreenSaver = false;

            return HRESULT.S_OK;
        }

        public HRESULT OnStartScreenSaver(string userName)
        {
            SBMI.WindowsScreenSaver = true;

            return HRESULT.S_OK;
        }
    }
}