namespace Sucrose.Mpv.NET.API
{
    public class MpvGetPropertyReplyEventArgs(ulong replyUserData, MpvError error, MpvEventProperty eventProperty) : EventArgs
    {
        public MpvEventProperty EventProperty { get; private set; } = eventProperty;

        public ulong ReplyUserData { get; private set; } = replyUserData;

        public MpvError Error { get; private set; } = error;
    }
}