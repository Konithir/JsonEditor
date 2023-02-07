using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZbigniewJson.Models;
using ZbigniewJson.Repos;

namespace ZbigniewJson
{
    public partial class MainWindow : Window
    {
        public string NameOfLastSavedFile;
        public List<SpeakerModel> SpeakerList;
        public ModeEnum ModeEnum;
        public bool BlockOnChangeEvent = false;
        public MainWindow()
        {
            InitializeComponent();
            HelpersRepo.MainWindow = this;
            MainTextBlock.IsUndoEnabled = false;
        }

        #region menu buttons

        private void MenuItem_Click_Open(object sender, RoutedEventArgs e)
        {
            MainTextBlock.Text = HelpersRepo.OpenFile();
            if(ModeEnum == ModeEnum.Managed)
            {
                HelpersRepo.LoadManaged(MainTextBlock, ref SpeakerList, ManagedParent, this);
            }
        }

        private void MenuItem_Click_SaveAs(object sender, RoutedEventArgs e)
        {
            if (ModeEnum == ModeEnum.Managed)
            {
                HelpersRepo.SerializeData(SpeakerList, MainTextBlock);
            }
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "JSON file|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                if (HelpersRepo.CheckModel(MainTextBlock.Text,ref SpeakerList))
                {
                    NameOfLastSavedFile = saveFileDialog.FileName;
                    System.IO.File.WriteAllText(NameOfLastSavedFile, MainTextBlock.Text);
                }

            }
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            if(ModeEnum == ModeEnum.Managed)
            {
                HelpersRepo.SerializeData(SpeakerList, MainTextBlock);
            }
            if(string.IsNullOrEmpty(NameOfLastSavedFile))
            {
                MenuItem_Click_SaveAs(null,null);
            }else
            {            
                if(HelpersRepo.CheckModel(MainTextBlock.Text,ref SpeakerList))
                {
                    System.IO.File.WriteAllText(NameOfLastSavedFile, MainTextBlock.Text);
                }
            }
        }

        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            MainTextBlock.Text = "";
            ManagedParent.Children.Clear();
            SpeakerList = new List<SpeakerModel>();
            if(ModeEnum == ModeEnum.Managed)
            {
                HelpersRepo.LoadManaged(MainTextBlock, ref SpeakerList, ManagedParent, this);
            }
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_Click_Undo(object sender, RoutedEventArgs e)
        {
            MainTextBlock.TextChanged -= MainTextBlock_TextChanged;
            MainTextBlock.Text = HistoryRepo.GetUndoText();
            if (ModeEnum == ModeEnum.Managed)
            {
                HelpersRepo.LoadManaged(MainTextBlock, ref SpeakerList, ManagedParent, this);
            }
            Task.Delay(1000).ContinueWith(t => ReAddEvent());//doesnt work correctly anyway, not always at least
            //MainTextBlock.TextChanged += MainTextBlock_TextChanged; //event is still being fired without delay....?
        }

        private void MenuItem_Click_Redo(object sender, RoutedEventArgs e)
        {
            MainTextBlock.TextChanged -= MainTextBlock_TextChanged;
            MainTextBlock.Text = HistoryRepo.GetRedoText();
            if (ModeEnum == ModeEnum.Managed)
            {
                HelpersRepo.LoadManaged(MainTextBlock, ref SpeakerList, ManagedParent, this);
            }
            Task.Delay(1000).ContinueWith(t => ReAddEvent());
            //MainTextBlock.TextChanged += MainTextBlock_TextChanged; //event is still being fired without delay....?
        }

        private void MenuItem_Click_Polish(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            HelpersRepo.ReloadApp(this);
        }

        private void MenuItem_Click_English(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            HelpersRepo.ReloadApp(this);
        }

        private void Button_Click_Raw(object sender, RoutedEventArgs e)
        {
            if (ModeEnum == ModeEnum.Managed)
            {
                ModeEnum = ModeEnum.Raw;
                if(SpeakerList.Count > 0)
                {
                    HelpersRepo.SerializeData(SpeakerList, MainTextBlock);
                }
            }
        }

        private void Button_Click_Managed(object sender, RoutedEventArgs e)
        {
            if(ModeEnum == ModeEnum.Raw)
            {
                ModeEnum = ModeEnum.Managed;
                HelpersRepo.LoadManaged(MainTextBlock, ref SpeakerList, ManagedParent, this);               
            }
        }
        private void MenuItem_Click_ToExcel(object sender, RoutedEventArgs e)
        {
            ExcelRepo.ExportExcel(SpeakerList);
        }

        private void MainTextBlock_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
                HistoryRepo.AddChangeToList(MainTextBlock.Text);
        }
        #endregion

        private void ReAddEvent()
        {
            MainTextBlock.TextChanged += MainTextBlock_TextChanged;
        }

    }
}
