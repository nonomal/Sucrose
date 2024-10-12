﻿using System.IO;
using SECNT = Skylark.Enum.ClearNumericType;
using SETT = Skylark.Enum.TimeType;
using SHN = Skylark.Helper.Numeric;
using SHV = Skylark.Helper.Versionly;
using SMC = Sucrose.Memory.Constant;
using SMMI = Sucrose.Manager.Manage.Internal;
using SMMM = Sucrose.Manager.Manage.Manager;
using SMR = Sucrose.Memory.Readonly;
using SSDECT = Sucrose.Shared.Dependency.Enum.CommandType;
using SSDETCT = Sucrose.Shared.Dependency.Enum.TransitionCycleType;
using SSDMMC = Sucrose.Shared.Dependency.Manage.Manager.Cycling;
using SSETTE = Skylark.Standard.Extension.Time.TimeExtension;
using SSSHP = Sucrose.Shared.Space.Helper.Processor;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SMML = Sucrose.Manager.Manage.Library;
using SMMCL = Sucrose.Memory.Manage.Constant.Library;

namespace Sucrose.Shared.Space.Helper
{
    internal static class Cycyling
    {
        public static bool Check(bool Time = true)
        {
            if (Directory.Exists(SMML.LibraryLocation))
            {
                List<string> Themes = Directory.GetDirectories(SMML.LibraryLocation).Select(Path.GetFileName).ToList();

                if (Themes.Any())
                {
                    Themes = Themes.Except(SMMM.DisableCycyling).ToList();

                    if (SMMM.Cycyling && (Themes.Count > 1 || (Themes.Count == 1 && !Themes.Contains(SMML.LibrarySelected))) && (SMMM.PassingCycyling >= Converter(SMMM.CycylingTime) || !Time))
                    {
                        foreach (string Theme in Themes)
                        {
                            string ThemePath = Path.Combine(SMML.LibraryLocation, Theme);
                            string InfoPath = Path.Combine(ThemePath, SMR.SucroseInfo);

                            if (Directory.Exists(ThemePath) && File.Exists(InfoPath))
                            {
                                if (SSTHI.CheckJson(SSTHI.ReadInfo(InfoPath)))
                                {
                                    SSTHI Info = SSTHI.ReadJson(InfoPath);

                                    if (Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
                                    {
                                        if (Theme != SMML.LibrarySelected)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }

                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void Change()
        {
            if ((!SMMM.ClosePerformance && !SMMM.PausePerformance) || !SSSHP.Work(SSSMI.Backgroundog))
            {
                if (Directory.Exists(SMML.LibraryLocation))
                {
                    List<string> Themes = Directory.GetDirectories(SMML.LibraryLocation).Select(Path.GetFileName).ToList();

                    if (Themes.Any())
                    {
                        string LibrarySelected = SMML.LibrarySelected;

                        Themes = Themes.Where(Theme => !SMMM.DisableCycyling.Contains(Theme) || Theme == LibrarySelected).ToList();

                        if (Themes.Count > 1)
                        {
                            string Selected = string.Empty;

                            int Index = Themes.IndexOf(LibrarySelected);

                            switch (SSDMMC.TransitionCycleType)
                            {
                                case SSDETCT.Random:
                                    while (string.IsNullOrEmpty(Selected))
                                    {
                                        while (Index == Themes.IndexOf(LibrarySelected))
                                        {
                                            Index = SMR.Randomise.Next(Themes.Count);
                                        }

                                        string Current = Themes[Index];

                                        string ThemePath = Path.Combine(SMML.LibraryLocation, Current);
                                        string InfoPath = Path.Combine(ThemePath, SMR.SucroseInfo);

                                        if (Directory.Exists(ThemePath) && File.Exists(InfoPath))
                                        {
                                            if (SSTHI.CheckJson(SSTHI.ReadInfo(InfoPath)))
                                            {
                                                SSTHI Info = SSTHI.ReadJson(InfoPath);

                                                if (Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
                                                {
                                                    Selected = Current;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SSDETCT.Sequential:
                                    if (Index < 0 || Index >= Themes.Count)
                                    {
                                        Index = 0;
                                    }
                                    else
                                    {
                                        Index += 1;

                                        if (Index >= Themes.Count)
                                        {
                                            Index = 0;
                                        }
                                    }

                                    foreach (string Theme in Themes.Skip(Index))
                                    {
                                        string ThemePath = Path.Combine(SMML.LibraryLocation, Theme);
                                        string InfoPath = Path.Combine(ThemePath, SMR.SucroseInfo);

                                        if (Directory.Exists(ThemePath) && File.Exists(InfoPath))
                                        {
                                            if (SSTHI.CheckJson(SSTHI.ReadInfo(InfoPath)))
                                            {
                                                SSTHI Info = SSTHI.ReadJson(InfoPath);

                                                if (Info.AppVersion.CompareTo(SHV.Entry()) <= 0)
                                                {
                                                    Selected = Theme;

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }

                            if (!string.IsNullOrEmpty(Selected))
                            {
                                SMMI.CyclingSettingManager.SetSetting(SMC.PassingCycyling, 0);

                                SMMI.LibrarySettingManager.SetSetting(SMMCL.LibrarySelected, Selected);

                                SSSHP.Run(SSSMI.Commandog, $"{SMR.StartCommand}{SSDECT.Cycyling}{SMR.ValueSeparator}{SMR.Unknown}");
                            }
                        }
                    }
                }
            }
        }

        private static int Converter(int Time)
        {
            return Convert.ToInt32(SHN.Numeral(SSETTE.Convert(Time, SETT.Minute, SETT.Second), false, false, 0, '0', SECNT.None));
        }
    }
}