using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZbigniewJson.Models;
using ZbigniewJson.Resources;
using ZbigniewJson.Windows;

namespace ZbigniewJson.Repos
{
    static class HelpersRepo
    {
        public static string NameOfLastSavedFile;
        public static MainWindow MainWindow;
        public static void AddSpeaker(ref List<SpeakerModel> speakerModels, WrapPanel parent)
        {
            var speaker = new SpeakerModel
            {
                GUID = Guid.NewGuid(),
                Variants = new Dictionary<string, List<VariantsModel>>()
            };
            speakerModels.Add(speaker);
            var speakerControl = new SpeakerModelUserControl
            {
                MainWindow = MainWindow,
                SpeakerModel = speaker
            };
            speakerControl.SpeakerTextBox.Text = speaker.Speaker;
            parent.Children.Add(speakerControl);
            AddVariationName(ref speaker, speakerControl.VariationsWrapPanel,ref speakerControl);
        }
        
        public static void AddVariationName(ref SpeakerModel speakerModel, WrapPanel parent,ref  SpeakerModelUserControl speakerModelUserControl)
        {
            string dictionaryKey = Guid.NewGuid().ToString();
            speakerModel.Variants.Add(dictionaryKey, new List<VariantsModel>());
            var variationNameControl = new VariantsNameUserControl
            {
                MainWindow = MainWindow,
                SpeakerModel = speakerModel,
                SpeakerModelUserControl = speakerModelUserControl,
                VariationNameOldTextBoxValue = dictionaryKey
            };
            variationNameControl.VariationNameTextBox.Text = dictionaryKey;
            parent.Children.Add(variationNameControl);

            AddVariationData(ref speakerModel,speakerModel.Variants[dictionaryKey],variationNameControl.VariationsNameWrapPanel,variationNameControl);
        }

        public static void AddVariationData(ref SpeakerModel speakerModel,List<VariantsModel> variants, WrapPanel parent, VariantsNameUserControl variantsNameUserControl)
        {
            var variant = new VariantsModel();
            variants.Add(variant);
            var variantDataControl = new VariantsDataUserControl {
                SpeakerModel = speakerModel,
                VariantsNameUserControl = variantsNameUserControl,
                VariantsModel = variant
            };
            variantDataControl.LanguageTextBox.Text = variant.Language;
            variantDataControl.TextTextBox.Text = variant.Text;
            parent.Children.Add(variantDataControl);
        }

        public static void DeleteSpeaker(ref List<SpeakerModel> speakerModels, ref SpeakerModel speakerModel, WrapPanel parent,SpeakerModelUserControl speakerModelUserControl)
        {
            speakerModels.Remove(speakerModel);
            parent.Children.Remove(speakerModelUserControl);
        }

        public static void DeleteVariationName(ref SpeakerModel speakerModel, WrapPanel parent, VariantsNameUserControl variantsNameUserControl)
        {
            speakerModel.Variants.Remove(variantsNameUserControl.VariationNameTextBox.Text);
            parent.Children.Remove(variantsNameUserControl);
        }

        public static void DeleteVariationData(ref SpeakerModel speakerModel,ref VariantsModel variantsModel, WrapPanel parent, VariantsDataUserControl variantsDataUserControl, VariantsNameUserControl variantsNameUserControl)
        {
            speakerModel.Variants[variantsNameUserControl.VariationNameTextBox.Text].Remove(variantsModel);
            parent.Children.Remove(variantsDataUserControl);
        }

        public static void LoadManaged(TextBox ContentHolder, ref List<SpeakerModel> SpeakerList, WrapPanel parent, MainWindow mainWindow)
        {
            if (!string.IsNullOrEmpty(ContentHolder.Text))
            {
                DeserializeData(ContentHolder.Text, ref SpeakerList);
                parent.Children.Clear();
                foreach (SpeakerModel speaker in SpeakerList)
                {
                    var speakerControl = new SpeakerModelUserControl();
                    speakerControl.MainWindow = mainWindow;
                    parent.Children.Add(speakerControl);
                    speakerControl.SpeakerTextBox.Text = speaker.Speaker;
                    speakerControl.SpeakerModel = speaker;
                    foreach (var variant in speaker.Variants)
                    {
                        var variantControl = new VariantsNameUserControl();
                        variantControl.MainWindow = mainWindow;
                        speakerControl.VariationsWrapPanel.Children.Add(variantControl);
                        variantControl.VariationNameOldTextBoxValue = variant.Key;
                        variantControl.VariationNameTextBox.Text = variant.Key;
                        variantControl.SpeakerModel = speaker;
                        variantControl.SpeakerModelUserControl = speakerControl;
                        foreach (var variantData in variant.Value)
                        {
                            var variantDataControl = new VariantsDataUserControl();
                            variantControl.VariationsNameWrapPanel.Children.Add(variantDataControl);
                            variantDataControl.TextTextBox.Text = variantData.Text;
                            variantDataControl.LanguageTextBox.Text = variantData.Language;
                            variantDataControl.SpeakerModel = speaker;
                            variantDataControl.VariantsModel = variantData;
                            variantDataControl.VariantsNameUserControl = variantControl;
                        }
                    }
                }
            }
            else
            {
                SpeakerList = new List<SpeakerModel>();
                parent.Children.Clear();
                HelpersRepo.AddSpeaker(ref SpeakerList, parent);
            }
        }
        public static string OpenFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                NameOfLastSavedFile = openFileDlg.FileName;
                return System.IO.File.ReadAllText(openFileDlg.FileName);
            }
            return null;
        }

        public static void DeserializeData(string data,ref List<SpeakerModel> SpeakerList)
        {
            SpeakerList = JsonConvert.DeserializeObject<List<SpeakerModel>>(data);
        }

        public static void SerializeData(List<SpeakerModel> data, TextBox jsonHolder)
        {
            jsonHolder.Text = JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static bool CheckModel(string data,ref List<SpeakerModel> SpeakerList)
        {
            try
            {
                DeserializeData(data,ref SpeakerList);
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show(StringResources.FileIsInvalid, StringResources.File, MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        return true;
                    case MessageBoxResult.No:
                        return false;
                }
            }
            return true;
        }

        public static void ReloadApp(MainWindow oldMainWindow)
        {
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            oldMainWindow.Close();
        }
    }
}
