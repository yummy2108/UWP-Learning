using MemoryBox.Models;
using MemoryBox.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.ViewModels
{
    class DiaryItemViewModel
    {
        private ObservableCollection<DiaryItem> allItems = new ObservableCollection<DiaryItem>();
        public ObservableCollection<DiaryItem> AllItems
        {
            get
            {
                return this.allItems;
            }
        }
        private static DiaryItemViewModel _ViewModel = new DiaryItemViewModel();
        public static DiaryItemViewModel getViewModel()
        {
            return _ViewModel;
        }
        private void ResumeFromDataBase()
        {
            //var items = DataBaseService.GetAllItems();
            //if (items == null)
            //{
            //    return;
            //}
            //foreach (DiaryItem x in items)
            //{
            //    allItems.Add(x);
            //}
            allItems = DataBaseService.GetAllItems();
            sortByDate();
            
        }
        private void sortByDate()
        {
            for(int i = allItems.Count() - 1; i > 0; i--)
            {
                for(int j = 0; j < i; j++)
                {
                    if (allItems[j].Date < allItems[j + 1].Date)
                    {
                        var temp = allItems[j];
                        allItems[j] = allItems[j + 1];
                        allItems[j + 1] = temp;
                    }
                }
            }
        }
        private DiaryItemViewModel()
        {
            ResumeFromDataBase();
        }

        public void AddDiaryItem(DiaryItem item)
        {
            this.allItems.Insert(0, item);
            DataBaseService.InsertDiaryItem(item);
        }
        public void RemoveDiaryItem(string DiaryItemID)
        {
            DiaryItem temp = null;
            foreach (DiaryItem x in allItems)
            {
                if (x.DiaryItemID == DiaryItemID)
                {
                    temp = x;
                    allItems.Remove(x);
                    break;
                }
            }
            DataBaseService.RemoveDiaryItem(temp.DiaryItemID);

        }
        public void ClearDiaryItem()
        {
            DataBaseService.ClearAll();
            for (int i = this.allItems.Count - 1; i >= 0; i--)
            {
                this.allItems.Remove(allItems[i]);
            }
        }
    }
}
