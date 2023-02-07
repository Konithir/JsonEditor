using System;
using System.Windows;
using System.Windows.Controls;
using ZbigniewJson.Models;
using ZbigniewJson.Repos;

namespace ZbigniewJson.Windows
{
    public partial class VariantsDataUserControl : UserControl
    {
        public SpeakerModel SpeakerModel;
        public VariantsModel VariantsModel;
        public VariantsNameUserControl VariantsNameUserControl;
        public VariantsDataUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            HelpersRepo.AddVariationData(
                ref SpeakerModel, SpeakerModel.Variants[VariantsNameUserControl.VariationNameTextBox.Text],
                VariantsNameUserControl.VariationsNameWrapPanel,
                VariantsNameUserControl);
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            HelpersRepo.DeleteVariationData(
                ref SpeakerModel,
                ref VariantsModel,
                VariantsNameUserControl.VariationsNameWrapPanel,
                this,
                VariantsNameUserControl);
        }

        private void Button_Click_Translate(object sender, RoutedEventArgs e)
        {
            string destinationLang = "";
            if(!string.IsNullOrEmpty(LanguageTextBox.Text) && LanguageTextBox.Text.Length>2)
            {
                destinationLang = LanguageTextBox.Text.Substring(0, 2);
            }
            TextTextBox.Text = RequestsRepo.GoogleTranslate(TextTextBox.Text,"auto", destinationLang).Result;
        }

        private void TextTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (VariantsModel != null)
                VariantsModel.Text = TextTextBox.Text;
        }

        private void LanguageTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VariantsModel != null)
                VariantsModel.Language = ((ComboBoxItem)LanguageTextBox.SelectedItem).Content.ToString();
        }
    }
}
