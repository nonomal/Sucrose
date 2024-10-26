namespace Sucrose.Shared.Dependency.Enum
{
    internal enum UpdateAutoType
    {
        Visible,
        SemiSilent,
        UpdateSilent,
        CompleteSilent
    }

    internal enum UpdateModuleType
    {
        Native,
        Downloader
    }

    internal enum UpdateServerType
    {
        GitHub,
        Soferity
    }
}