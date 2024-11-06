namespace Sucrose.Mpv.NET.API
{
    public class MpvStartFileEventArgs(MpvEventStartFile eventStartFile) : EventArgs
    {
        public MpvEventStartFile EventStartFile { get; private set; } = eventStartFile;
    }
}