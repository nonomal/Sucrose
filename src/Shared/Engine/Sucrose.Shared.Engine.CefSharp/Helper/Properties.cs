using System.IO;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SSECCE = Skylark.Standard.Extension.Cryptology.CryptologyExtension;
using SSECSMI = Sucrose.Shared.Engine.CefSharp.Manage.Internal;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSSHF = Sucrose.Shared.Space.Helper.Filing;

namespace Sucrose.Shared.Engine.CefSharp.Helper
{
    internal static class Properties
    {
        public static void Start()
        {
            if (!Directory.Exists(SSECSMI.CefPath))
            {
                Directory.CreateDirectory(SSECSMI.CefPath);
            }

            SSEMI.PropertiesPath = Path.Combine(SSECSMI.CefPath, SMMRC.SucroseProperties);

            SSSHF.Write(SSEMI.PropertiesPath, SSECCE.BaseToText(SSEMI.CefProperties));
        }
    }
}