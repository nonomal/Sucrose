namespace Sucrose.Mpv.NET.API
{
    public class MpvPropertyChangeEventArgs(ulong replyUserData, MpvEventProperty eventProperty) : EventArgs
    {
        public MpvEventProperty EventProperty { get; private set; } = eventProperty;

        public ulong ReplyUserData { get; private set; } = replyUserData;
    }
}