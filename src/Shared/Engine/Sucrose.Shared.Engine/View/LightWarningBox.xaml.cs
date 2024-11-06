using System.Media;
using System.Windows;

namespace Sucrose.Shared.Engine.View
{
    /// <summary>
    /// Interaction logic for LightWarningBox.xaml
    /// </summary>
    public partial class LightWarningBox : Window
    {
        public LightWarningBox(string DialogTitle, string DialogMessage, string DialogInfo, string CloseText)
        {
            InitializeComponent();

            SystemSounds.Asterisk.Play();

            Title = DialogTitle;
            Dialog_Info.Text = DialogInfo;
            Dialog_Message.Text = DialogMessage;

            Close_Button.Content = CloseText;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void LightWarningBox_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);

            ShowInTaskbar = true;
        }
    }
}