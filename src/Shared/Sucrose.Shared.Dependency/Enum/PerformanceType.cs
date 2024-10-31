namespace Sucrose.Shared.Dependency.Enum
{
    internal enum PerformanceType
    {
        Close,
        Pause,
        Resume
    }

    internal enum PausePerformanceType
    {
        Heavy,
        Light
    }

    internal enum NetworkPerformanceType
    {
        Not,
        Ping,
        Upload,
        Download
    }

    internal enum CategoryPerformanceType
    {
        Not,
        Cpu,
        Gpu,
        Lock,
        Focus,
        Sleep,
        Memory,
        Remote,
        Battery,
        Console,
        Network,
        Session,
        Virtual,
        FullScreen,
        ScreenSaver,
        BatterySaver
    }
}