﻿using System.Media;
using System.Windows;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRU = Sucrose.Memory.Manage.Readonly.Url;
using SRER = Sucrose.Resources.Extension.Resources;
using SSDECT = Sucrose.Shared.Dependency.Enum.CommandType;
using SSSHP = Sucrose.Shared.Space.Helper.Processor;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;

namespace Sucrose.Watchdog.View
{
    /// <summary>
    /// Interaction logic for LightErrorMessageBox.xaml
    /// </summary>
    public partial class LightErrorMessageBox : Window
    {
        private static bool State = true;

        private static string Path = string.Empty;
        private static string Text = string.Empty;
        private static string Source = string.Empty;
        private static string Message = string.Empty;
        private static string Exception = string.Empty;

        public LightErrorMessageBox(string RawException, string ErrorMessage, string LogPath, string HelpSource = null, string HelpText = null)
        {
            InitializeComponent();

            Path = LogPath;
            Text = HelpText;
            Source = HelpSource;
            Message = ErrorMessage;
            Exception = RawException;

            SystemSounds.Hand.Play();

            if (!string.IsNullOrEmpty(Source))
            {
                Help_Button.Content = Text;
            }

            Error_Message.Text += Environment.NewLine + Message;
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (State)
            {
                Show_Button.Content = SRER.GetValue("Watchdog", "HideButton");
                Error_Message.Text = SRER.GetValue("Watchdog", "ErrorMessage") + Environment.NewLine + Exception;
            }
            else
            {
                Show_Button.Content = SRER.GetValue("Watchdog", "ShowButton");
                Error_Message.Text = SRER.GetValue("Watchdog", "ErrorMessage") + Environment.NewLine + Message;
            }

            State = !State;

            //SSSHP.Run(SSSMI.Commandog, $"{SMMRG.StartCommand}{SSDECT.Log}{SMMRG.ValueSeparator}{Path}");
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Source))
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMMRG.StartCommand}{SSDECT.Wiki}{SMMRG.ValueSeparator}{SMMRU.GitHubSucroseWiki}");
            }
            else
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMMRG.StartCommand}{SSDECT.Wiki}{SMMRG.ValueSeparator}{Source}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void LightErrorMessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);

            ShowInTaskbar = true;
        }
    }
}