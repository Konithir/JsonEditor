using System.Windows;
using System.Windows.Controls;
using ZbigniewJson.Extensions;
using ZbigniewJson.Models;
using ZbigniewJson.Repos;

namespace ZbigniewJson.Windows
{
    public partial class VariantsNameUserControl : UserControl
    {
        public MainWindow MainWindow;
        public SpeakerModel SpeakerModel;
        public SpeakerModelUserControl SpeakerModelUserControl;
        public string VariationNameOldTextBoxValue;
        public VariantsNameUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            HelpersRepo.AddVariationName(ref SpeakerModel, SpeakerModelUserControl.VariationsWrapPanel,ref SpeakerModelUserControl);
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            HelpersRepo.DeleteVariationName(ref SpeakerModel, SpeakerModelUserControl.VariationsWrapPanel,this);
        }

        private void TextBox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if(SpeakerModel != null && SpeakerModel.Variants.Count > 0 && !string.IsNullOrEmpty(VariationNameTextBox.Text))
            {
                if(!SpeakerModel.Variants.ContainsKey(VariationNameTextBox.Text))
                {
                    SpeakerModel.Variants.UpdateKey(VariationNameOldTextBoxValue, VariationNameTextBox.Text);
                    VariationNameOldTextBoxValue = VariationNameTextBox.Text;
                }
            }
         
        }
    }
}
