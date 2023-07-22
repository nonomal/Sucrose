﻿using SSDECT = Sucrose.Shared.Dependency.Enum.CommandsType;
using SMR = Sucrose.Memory.Readonly;
using SSHP = Sucrose.Space.Helper.Processor;
using SSMI = Sucrose.Space.Manage.Internal;

namespace Sucrose.Shared.Launcher.Command
{
    internal static class Report
    {
        public static void Command()
        {
            SSHP.Run(SSMI.Commandog, $"{SMR.StartCommand}{SSDECT.Report}{SMR.ValueSeparator}{SMR.ReportWebsite}");
        }
    }
}