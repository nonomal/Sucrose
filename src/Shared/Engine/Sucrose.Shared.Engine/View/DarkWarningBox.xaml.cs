using System.Media;
using System.Windows;
using SSSHD = Sucrose.Shared.Space.Helper.Dark;
using SWHWI = Skylark.Wing.Helper.WindowInterop;

namespace Sucrose.Shared.Engine.View
{
    /// <summary>
    /// Interaction logic for DarkWarningBox.xaml
    /// </summary>
    public partial class DarkWarningBox : Window
    {
        public DarkWarningBox(string DialogTitle, string DialogMessage, string DialogInfo, string CloseText)
        {
            InitializeComponent();

            SystemSounds.Asterisk.Play();

            Title = DialogTitle;
            Dialog_Info.Text = DialogInfo;
            Dialog_Message.Text = DialogMessage;

            Close_Button.Content = CloseText;

            SourceInitialized += DarkWarningBox_SourceInitialized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DarkWarningBox_SourceInitialized(object sender, EventArgs e)
        {
            SSSHD.Apply(SWHWI.Handle(this));
        }

        private async void DarkWarningBox_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);

            ShowInTaskbar = true;
        }
    }
}