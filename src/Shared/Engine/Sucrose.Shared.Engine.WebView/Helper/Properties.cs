using System.IO;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SSECCE = Skylark.Standard.Extension.Cryptology.CryptologyExtension;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSEWVMI = Sucrose.Shared.Engine.WebView.Manage.Internal;
using SSSHF = Sucrose.Shared.Space.Helper.Filing;

namespace Sucrose.Shared.Engine.WebView.Helper
{
    internal static class Properties
    {
        public static void Start()
        {
            if (!Directory.Exists(SSEWVMI.WebPath))
            {
                Directory.CreateDirectory(SSEWVMI.WebPath);
            }

            SSEMI.PropertiesPath = Path.Combine(SSEWVMI.WebPath, SMMRC.SucroseProperties);

            SSSHF.Write(SSEMI.PropertiesPath, SSECCE.BaseToText(SSEMI.WebProperties));
        }
    }
}