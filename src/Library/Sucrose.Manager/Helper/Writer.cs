using SMHC = Sucrose.Manager.Helper.Cleaner;
using SMHU = Sucrose.Manager.Helper.Unique;

namespace Sucrose.Manager.Helper
{
    internal static class Writer
    {
        public static void Write(string filePath, string fileContent)
        {
            using Mutex Mutex = new(false, SMHU.GenerateText(filePath));

            try
            {
                try
                {
                    Mutex.WaitOne();
                }
                catch { }

                File.WriteAllText(filePath, SMHC.Clean(fileContent));
            }
            finally
            {
                Mutex.ReleaseMutex();
            }
        }

        public static void WriteAppend(string filePath, string fileContent)
        {
            using Mutex Mutex = new(false, SMHU.GenerateText(filePath));

            try
            {
                try
                {
                    Mutex.WaitOne();
                }
                catch { }

                using StreamWriter writer = File.AppendText(filePath);

                writer.WriteLine(SMHC.Clean(fileContent));
            }
            finally
            {
                Mutex.ReleaseMutex();
            }
        }

        public static void WriteStream(string filePath, string fileContent)
        {
            using Mutex Mutex = new(false, SMHU.GenerateText(filePath));

            try
            {
                try
                {
                    Mutex.WaitOne();
                }
                catch { }

                FileMode fileMode = File.Exists(filePath) ? FileMode.Truncate : FileMode.CreateNew;

                using FileStream fileStream = new(filePath, fileMode, FileAccess.Write, FileShare.None);
                using StreamWriter writer = new(fileStream);

                writer.Write(SMHC.Clean(fileContent));
            }
            finally
            {
                Mutex.ReleaseMutex();
            }
        }
    }
}