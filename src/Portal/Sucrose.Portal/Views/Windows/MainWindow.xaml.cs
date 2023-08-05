﻿using Sucrose.Portal.Services.Contracts;
using Sucrose.Portal.ViewModels;
using System.Windows;
using SEWTT = Skylark.Enum.WindowsThemeType;
using SMC = Sucrose.Memory.Constant;
using SMMI = Sucrose.Manager.Manage.Internal;
using SWHWT = Skylark.Wing.Helper.WindowsTheme;
using WUAT = Wpf.Ui.Appearance.Theme;
using WUATT = Wpf.Ui.Appearance.ThemeType;

namespace Sucrose.Portal.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IWindow
    {
        private static SEWTT Theme => SMMI.GeneralSettingManager.GetSetting(SMC.ThemeType, SWHWT.GetTheme());

        public MainWindowViewModel ViewModel { get; }

        public MainWindow(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            if (Theme == SEWTT.Dark)
            {
                WUAT.Apply(WUATT.Dark);
            }
            else
            {
                WUAT.Apply(WUATT.Light);
            }

            InitializeComponent();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double WindowWidth = e.NewSize.Width;
            double SearchWidth = SearchBox.RenderSize.Width;

            SearchBox.Margin = new Thickness(0, 0, ((WindowWidth - SearchWidth) / 2) - 160, 0);
        }
    }
}