using Sucrose.Mpv.NET.API.Interop;
using System.Runtime.InteropServices;

namespace Sucrose.Mpv.NET.API
{
    public class MpvFunctions : IMpvFunctions, IDisposable
    {
        public MpvClientAPIVersion ClientAPIVersion { get; private set; }
        public MpvClientId ClientId { get; private set; }
        public MpvErrorString ErrorString { get; private set; }
        public MpvFree Free { get; private set; }
        public MpvClientName ClientName { get; private set; }
        public MpvCreate Create { get; private set; }
        public MpvInitialise Initialise { get; private set; }
        public MpvDestroy Destroy { get; private set; }
        public MpvTerminateDestroy TerminateDestroy { get; private set; }
        public MpvCreateClient CreateClient { get; private set; }
        public MpvCreateWeakClient CreateWeakClient { get; private set; }
        public MpvLoadConfigFile LoadConfigFile { get; private set; }
        public MpvGetTimeUs GetTimeUs { get; private set; }
        public MpvSetOption SetOption { get; private set; }
        public MpvSetOptionString SetOptionString { get; private set; }
        public MpvCommand Command { get; private set; }
        public MpvCommandString CommandString { get; private set; }
        public MpvCommandAsync CommandAsync { get; private set; }
        public MpvAbortAsyncCommand AbortAsyncCommand { get; private set; }
        public MpvSetProperty SetProperty { get; private set; }
        public MpvSetPropertyString SetPropertyString { get; private set; }
        public MpvSetPropertyAsync SetPropertyAsync { get; private set; }
        public MpvGetProperty GetProperty { get; private set; }
        public MpvGetPropertyString GetPropertyString { get; private set; }
        public MpvGetPropertyOSDString GetPropertyOSDString { get; private set; }
        public MpvGetPropertyAsync GetPropertyAsync { get; private set; }
        public MpvObserveProperty ObserveProperty { get; private set; }
        public MpvUnobserveProperty UnobserveProperty { get; private set; }
        public MpvEventName EventName { get; private set; }
        public MpvRequestEvent RequestEvent { get; private set; }
        public MpvRequestLogMessages RequestLogMessages { get; private set; }
        public MpvWaitEvent WaitEvent { get; private set; }
        public MpvWakeup Wakeup { get; private set; }
        public MpvSetWakeupCallback SetWakeupCallback { get; private set; }
        public MpvWaitAsyncRequests WaitAsyncRequests { get; private set; }
        public MpvHookAdd HookAdd { get; private set; }
        public MpvHookContinue HookContinue { get; private set; }

        // Not strictly part of the C API but are used to invoke mpv_get_property with value data type.
        public MpvGetPropertyDouble GetPropertyDouble { get; private set; }
        public MpvGetPropertyLong GetPropertyLong { get; private set; }

        private IntPtr dllHandle;

        private bool disposed = false;

        public MpvFunctions(string dllPath)
        {
            LoadDll(dllPath);

            LoadFunctions();
        }

        private void LoadDll(string dllPath)
        {
            Guard.AgainstNullOrEmptyOrWhiteSpaceString(dllPath, nameof(dllPath));

            if (!File.Exists(dllPath))
            {
                throw new DllNotFoundException($"Failed to load Mpv DLL. File not found: {dllPath}");
            }

            dllHandle = PlatformDll.Utils.LoadLibrary(dllPath);

            if (dllHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();

                string appArchitecture = "Unknown Architecture";

#if X86
                appArchitecture = "x86 (32-bit)";
#elif X64
                appArchitecture = "x64 (64-bit)";
#elif ARM64
                appArchitecture = "ARM64 (64-bit)";
#endif

                throw new MpvAPIException(
                    $"Failed to load Mpv DLL. Error Code: {errorCode}.\n" +
                    $"Detected application architecture: {appArchitecture}.\n" +
                    "Please ensure you are running the correct version of the application for your system architecture.\n" +
                    "Ensure you're using the correct libmpv version compatible with this architecture.\n" +
                    "For x64, use a 64-bit (x86_64) libmpv; for x86, use a 32-bit (i686) libmpv; for ARM64, ensure compatibility with 64-bit (aarch64) libmpv.\n" +
                    "Check that the required DLLs are present in the application directory, and that your system meets the required dependencies.\n\n" +
                    "If you believe you have the correct architecture, you can visit https://support.microsoft.com/kb/2977003#latest-microsoft-visual-c-redistributable-version " +
                    "to check for any issues related to the necessary dependencies and download the required ones to resolve them.\n\n" +
                    "If you believe you have done everything correctly, please reach out to the appropriate support channels for assistance."
                );

                //throw new MpvAPIException($"Failed to load Mpv DLL. Error Code: {errorCode}. Make sure you're loading the correct architecture DLL.");
                //throw new MpvAPIException($"Failed to load Mpv DLL. Error Code: {errorCode}. .NET apps by default are 32-bit so make sure you're loading the 32-bit DLL.");
                //throw new MpvAPIException($"Failed to load Mpv DLL. Error Code: {errorCode}. .NET apps by default are 32-bit so make sure you're loading the correct architecture DLL.");
            }
        }

        private void LoadFunctions()
        {
            ClientAPIVersion = LoadFunction<MpvClientAPIVersion>("mpv_client_api_version");
            ClientId = LoadFunction<MpvClientId>("mpv_client_id");
            ErrorString = LoadFunction<MpvErrorString>("mpv_error_string");
            Free = LoadFunction<MpvFree>("mpv_free");
            ClientName = LoadFunction<MpvClientName>("mpv_client_name");
            Create = LoadFunction<MpvCreate>("mpv_create");
            Initialise = LoadFunction<MpvInitialise>("mpv_initialize");
            Destroy = LoadFunction<MpvDestroy>("mpv_destroy");
            TerminateDestroy = LoadFunction<MpvTerminateDestroy>("mpv_terminate_destroy");
            CreateClient = LoadFunction<MpvCreateClient>("mpv_create_client");
            CreateWeakClient = LoadFunction<MpvCreateWeakClient>("mpv_create_weak_client");
            LoadConfigFile = LoadFunction<MpvLoadConfigFile>("mpv_load_config_file");
            GetTimeUs = LoadFunction<MpvGetTimeUs>("mpv_get_time_us");
            SetOption = LoadFunction<MpvSetOption>("mpv_set_option");
            SetOptionString = LoadFunction<MpvSetOptionString>("mpv_set_option_string");
            Command = LoadFunction<MpvCommand>("mpv_command");
            CommandString = LoadFunction<MpvCommandString>("mpv_command_string");
            CommandAsync = LoadFunction<MpvCommandAsync>("mpv_command_async");
            AbortAsyncCommand = LoadFunction<MpvAbortAsyncCommand>("mpv_abort_async_command");
            SetProperty = LoadFunction<MpvSetProperty>("mpv_set_property");
            SetPropertyString = LoadFunction<MpvSetPropertyString>("mpv_set_property_string");
            SetPropertyAsync = LoadFunction<MpvSetPropertyAsync>("mpv_set_property_async");
            GetProperty = LoadFunction<MpvGetProperty>("mpv_get_property");
            GetPropertyString = LoadFunction<MpvGetPropertyString>("mpv_get_property_string");
            GetPropertyOSDString = LoadFunction<MpvGetPropertyOSDString>("mpv_get_property_osd_string");
            GetPropertyAsync = LoadFunction<MpvGetPropertyAsync>("mpv_get_property_async");
            ObserveProperty = LoadFunction<MpvObserveProperty>("mpv_observe_property");
            UnobserveProperty = LoadFunction<MpvUnobserveProperty>("mpv_unobserve_property");
            EventName = LoadFunction<MpvEventName>("mpv_event_name");
            RequestEvent = LoadFunction<MpvRequestEvent>("mpv_request_event");
            RequestLogMessages = LoadFunction<MpvRequestLogMessages>("mpv_request_log_messages");
            WaitEvent = LoadFunction<MpvWaitEvent>("mpv_wait_event");
            Wakeup = LoadFunction<MpvWakeup>("mpv_wakeup");
            SetWakeupCallback = LoadFunction<MpvSetWakeupCallback>("mpv_set_wakeup_callback");
            WaitAsyncRequests = LoadFunction<MpvWaitAsyncRequests>("mpv_wait_async_requests");
            HookAdd = LoadFunction<MpvHookAdd>("mpv_hook_add");
            HookContinue = LoadFunction<MpvHookContinue>("mpv_hook_continue");

            GetPropertyDouble = LoadFunction<MpvGetPropertyDouble>("mpv_get_property");
            GetPropertyLong = LoadFunction<MpvGetPropertyLong>("mpv_get_property");
        }

        private TDelegate LoadFunction<TDelegate>(string name) where TDelegate : class
        {
            TDelegate delegateValue = MpvMarshal.LoadUnmanagedFunction<TDelegate>(dllHandle, name);
            if (delegateValue == null)
            {
                throw new MpvAPIException($"Failed to load Mpv \"{name}\" function.");
            }

            return delegateValue;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    PlatformDll.Utils.FreeLibrary(dllHandle);
                }

                disposed = true;
            }
        }
    }
}