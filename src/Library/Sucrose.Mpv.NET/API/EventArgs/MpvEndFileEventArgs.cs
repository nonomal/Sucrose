namespace Sucrose.Mpv.NET.API
{
    public class MpvEndFileEventArgs(MpvEventEndFile eventEndFile) : EventArgs
    {
        public MpvEventEndFile EventEndFile { get; private set; } = eventEndFile;
    }
}