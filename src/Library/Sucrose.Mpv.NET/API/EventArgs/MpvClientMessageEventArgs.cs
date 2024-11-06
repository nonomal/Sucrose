namespace Sucrose.Mpv.NET.API
{
    public class MpvClientMessageEventArgs(MpvEventClientMessage eventClientMessage) : EventArgs
    {
        public MpvEventClientMessage EventClientMessage { get; private set; } = eventClientMessage;
    }
}