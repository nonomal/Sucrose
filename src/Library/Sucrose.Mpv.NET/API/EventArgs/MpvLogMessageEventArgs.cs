namespace Sucrose.Mpv.NET.API
{
    public class MpvLogMessageEventArgs(MpvLogMessage message) : EventArgs
    {
        public MpvLogMessage Message { get; private set; } = message;
    }
}