using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZbigniewJson.Repos
{
    public static class HistoryRepo
    {
        private static List<string> HistoryList = new List<string>();
        private const int MaxChanges = 10;
        private static int CurrentIndex = -1;
        private static HistoryActionEnum LastAction;

        public static void AddChangeToList(string change)
        {
            if (LastAction == HistoryActionEnum.Redo)
            {
                for(int i=HistoryList.Count-1;i>CurrentIndex;i--)
                {
                    HistoryList.RemoveAt(i);
                }
            }
            LastAction = HistoryActionEnum.Insert;
            HistoryList.Add(change);
            CurrentIndex = HistoryList.Count - 1;
            if (HistoryList.Count > MaxChanges)
            {
                HistoryList.RemoveAt(0);
            }
        }

        public static string GetUndoText()
        {
            LastAction = HistoryActionEnum.Undo;
            if (CurrentIndex > 0)
                CurrentIndex--;
            return HistoryList[CurrentIndex];
        }

        public static string GetRedoText()
        {
            LastAction = HistoryActionEnum.Redo;
            if (CurrentIndex < MaxChanges && CurrentIndex < HistoryList.Count-1)
                CurrentIndex++;
            return HistoryList[CurrentIndex];
        }

    }
}
