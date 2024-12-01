using SMHC = Sucrose.Manager.Helper.Cleaner;
using SMHU = Sucrose.Manager.Helper.Unique;

namespace Sucrose.Manager.Helper
{
    internal static class Reader
    {
        public static string Read(string filePath)
        {
            using Mutex Mutex = new(false, SMHU.GenerateText(filePath));

            try
            {
                try
                {
                    Mutex.WaitOne();
                }
                catch { }

                return SMHC.Clean(File.ReadAllText(filePath));
            }
            finally
            {
                Mutex.ReleaseMutex();
            }
        }

        public static string ReadStream(string filePath)
        {
            using Mutex Mutex = new(false, SMHU.GenerateText(filePath));

            try
            {
                try
                {
                    Mutex.WaitOne();
                }
                catch { }

                using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using StreamReader reader = new(fileStream);

                return SMHC.Clean(reader.ReadToEnd());
            }
            finally
            {
                Mutex.ReleaseMutex();
            }
        }
    }
}