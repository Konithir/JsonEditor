using System.Windows;
using System.Windows.Controls;
using ZbigniewJson.Models;
using ZbigniewJson.Repos;

namespace ZbigniewJson.Windows
{
    public partial class SpeakerModelUserControl : UserControl
    {
        public MainWindow MainWindow;
        public SpeakerModel SpeakerModel;
        public SpeakerModelUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            HelpersRepo.AddSpeaker(ref MainWindow.SpeakerList, MainWindow.ManagedParent);
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            HelpersRepo.DeleteSpeaker(ref MainWindow.SpeakerList,ref SpeakerModel,MainWindow.ManagedParent,this);
        }

        private void SpeakerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SpeakerModel != null)
                SpeakerModel.Speaker = SpeakerTextBox.Text;
        }
    }
}
