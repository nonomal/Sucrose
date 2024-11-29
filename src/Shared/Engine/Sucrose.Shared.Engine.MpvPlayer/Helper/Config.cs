using System.IO;
using System.Text.RegularExpressions;
using SMME = Sucrose.Manager.Manage.Engine;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSEMPMI = Sucrose.Shared.Engine.MpvPlayer.Manage.Internal;
using SSSHF = Sucrose.Shared.Space.Helper.Filing;
using SSSHR = Sucrose.Shared.Space.Helper.Regexer;

namespace Sucrose.Shared.Engine.MpvPlayer.Helper
{
    internal static class Config
    {
        public static void Start()
        {
            if (!Directory.Exists(SSEMPMI.MpvPath))
            {
                Directory.CreateDirectory(SSEMPMI.MpvPath);
            }

            SSEMPMI.MpvConfig = Path.Combine(SSEMPMI.MpvPath, SMMRC.uMpvPlayerConfig);

            if (!File.Exists(SSEMPMI.MpvConfig))
            {
                SSEMPMI.MpvConfig = Path.Combine(SSEMPMI.MpvPath, SMMRC.MpvPlayerConfig);

                string Content = string.Join(Environment.NewLine, SSEMI.MpvConfig);

                if (SMME.StayAwake)
                {
                    Content = SSSHR.Replace(Content, @"^stop-screensaver=.*$", "stop-screensaver=always", RegexOptions.Multiline);
                }
                else
                {
                    Content = SSSHR.Replace(Content, @"^stop-screensaver=.*$", "stop-screensaver=no", RegexOptions.Multiline);
                }

                if (SMME.HardwareAcceleration)
                {
                    Content = SSSHR.Replace(Content, @"^hwdec=.*$", "hwdec=auto-safe", RegexOptions.Multiline);
                }
                else
                {
                    Content = SSSHR.Replace(Content, @"^hwdec=.*$", "hwdec=no", RegexOptions.Multiline);
                }

                SSSHF.Write(SSEMPMI.MpvConfig, Content);
            }
        }
    }
}