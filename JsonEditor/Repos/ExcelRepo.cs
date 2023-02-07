using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZbigniewJson.Models;

namespace ZbigniewJson.Repos
{
    public static class ExcelRepo
    {
        public static void ExportExcel(List<SpeakerModel> data)
        {
            if(data != null && data.Count>0)
            {
                var excel = new Microsoft.Office.Interop.Excel.Application();
                var excelworkBook = excel.Workbooks.Add();
                var excelSheet = (Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = "DataSheet";

                try
                {
                    int row = 0;
                    foreach (var speakerObject in data)
                    {
                        row++;
                        int col = 1;
                        excelSheet.Cells[row, col] = speakerObject.Speaker;
                        excelSheet.Cells[row, col].Font.Bold = true;
                        col++;
                        excelSheet.Cells[row, col] = speakerObject.GUID.ToString();
                        excelSheet.Cells[row, col].Font.Bold = true;
                        col++;
                        excelSheet.Cells[row, col] = "Variants";
                        excelSheet.Cells[row, col].Font.Bold = true;
                        col++;
                        foreach (var variantName in speakerObject.Variants)
                        {
                            row++;
                            col = 4;
                            excelSheet.Cells[row, col] = variantName.Key;
                            foreach (var variantData in variantName.Value)
                            {
                                row++;
                                col = 5;
                                excelSheet.Cells[row, col] = variantData.Text;
                                row++;
                                excelSheet.Cells[row, col] = variantData.Language;
                            }
                        }
                    }

                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    saveFileDialog.Filter = "XLSX file|*.xlsx";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        excelworkBook.Close(true, saveFileDialog.FileName);
                    }

                }
                catch (Exception ex)
                {
                    excelworkBook.Close(false);
                    MessageBox.Show("Export Failed. " + ex.Message, "Error", MessageBoxButton.OK);
                }
            }else
            {
               MessageBox.Show("Export Failed. No data to export", "Error", MessageBoxButton.OK);
            }
        }
    }
}
