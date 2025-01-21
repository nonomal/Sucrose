using SELLT = Skylark.Enum.LevelLogType;
using SELT = Skylark.Enum.LogType;
using SMHW = Sucrose.Manager.Helper.Writer;
using SMMRF = Sucrose.Memory.Manage.Readonly.Folder;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SMMVL = Sucrose.Memory.Manage.Valuable.Log;

namespace Sucrose.Manager
{
    public class LogManager
    {
        private int threadId;
        private SELT logType;
        private string logFilePath;

        public LogManager(string logFileName, SELT logType = SELT.All)
        {
            this.logType = logType;

            threadId = SMMRG.Randomise.Next(1000, 9999);

            logFilePath = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Log, string.Format(logFileName, SMMVL.FileNameDate));

            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
        }

        public void Log(SELLT level, string message)
        {
            if (logType == SELT.None)
            {
                return;
            }

            try
            {
                SMHW.WriteAppend(logFilePath, $"[{SMMVL.FileTimeLine}] ~ [{SMMVL.FileDescriptionLine}-{threadId}/{level}] ~ [{message}]");
            }
            catch { }
        }

        public void Log(SELLT level, params string[] messages)
        {
            if (logType == SELT.None)
            {
                return;
            }

            try
            {
                foreach (string message in messages)
                {
                    SMHW.WriteAppend(logFilePath, $"[{SMMVL.FileTimeLine}] ~ [{SMMVL.FileDescriptionLine}-{threadId}/{level}] ~ [{message}]");
                }
            }
            catch { }
        }

        public bool CheckFile()
        {
            return File.Exists(logFilePath);
        }

        public string LogFile()
        {
            return logFilePath;
        }
    }
}