namespace Sucrose.Mpv.NET.API
{
    public class MpvEventHookEventArgs(MpvEventHook eventHook) : EventArgs
    {
        public MpvEventHook EventHook { get; private set; } = eventHook;
    }
}