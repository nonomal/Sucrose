namespace Sucrose.Mpv.NET.API
{
    public class MpvCommandReplyEventArgs(ulong replyUserData, MpvError error) : EventArgs
    {
        public ulong ReplyUserData { get; private set; } = replyUserData;

        public MpvError Error { get; private set; } = error;
    }
}