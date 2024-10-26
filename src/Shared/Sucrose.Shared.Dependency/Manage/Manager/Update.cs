using SMMCU = Sucrose.Memory.Manage.Constant.Update;
using SMMI = Sucrose.Manager.Manage.Internal;
using SSDEUAT = Sucrose.Shared.Dependency.Enum.UpdateAutoType;
using SSDEUMT = Sucrose.Shared.Dependency.Enum.UpdateModuleType;
using SSDEUST = Sucrose.Shared.Dependency.Enum.UpdateServerType;

namespace Sucrose.Shared.Dependency.Manage.Manager
{
    internal static class Update
    {
        public static SSDEUMT ModuleType => SMMI.UpdateSettingManager.GetSetting(SMMCU.ModuleType, SSDEUMT.Downloader);

        public static SSDEUST ServerType => SMMI.UpdateSettingManager.GetSetting(SMMCU.ServerType, SSDEUST.Soferity);

        public static SSDEUAT AutoType => SMMI.UpdateSettingManager.GetSetting(SMMCU.AutoType, SSDEUAT.SemiSilent);
    }
}