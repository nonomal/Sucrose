namespace Sucrose.Mpv.NET.Player
{
    public class MpvPlayerPositionChangedEventArgs(double newPosition) : EventArgs
    {
        public TimeSpan NewPosition { get; private set; } = TimeSpan.FromSeconds(newPosition);
    }
}