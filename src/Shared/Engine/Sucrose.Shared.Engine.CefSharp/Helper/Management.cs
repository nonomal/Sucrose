﻿using System.Diagnostics;
using SMR = Sucrose.Memory.Readonly;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSSHM = Sucrose.Shared.Space.Helper.Management;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;

namespace Sucrose.Shared.Engine.CefSharp.Helper
{
    internal static class Management
    {
        public static void SetProcesses()
        {
            try
            {
                Process.GetProcesses()
                    .Where(Process => Process.ProcessName.Contains(SMR.CefSharpProcessName) && SSSHM.GetCommandLine(Process).Contains(SMMRG.AppName) && !SSEMI.Processes.Contains(Process.Id))
                    .ToList()
                    .ForEach(Process => SSEMI.Processes.Add(Process.Id));
            }
            catch { }
        }
    }
}