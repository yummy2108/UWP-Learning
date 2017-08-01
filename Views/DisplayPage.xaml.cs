using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MemoryBox.Models;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace MemoryBox.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DisplayPage : Page
    {
        ViewModels.DiaryItemViewModel ViewModel { get; set; }
        public DisplayPage()
        {
            this.InitializeComponent();
            this.ViewModel = ViewModels.DiaryItemViewModel.getViewModel();
           
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    ViewerExitStoryboard.Begin();
        //}

        private void diaryViewer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ViewerEnterStoryboard.Begin();
        }

        private void diaryViewer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ViewerExitStoryboard.Begin();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem selectedItem = sender as MenuFlyoutItem;

            if (selectedItem == null)
            {
                return;
            }

            if (selectedItem.Tag.ToString() == "deleteCur")
            {
                // delete current diary
                DiaryItem x = (DiaryItem)diaryList.SelectedItem;
                ViewModel.RemoveDiaryItem(x.DiaryItemID);
                Frame.Navigate(typeof(MainPage), ViewModel);

            }
            else
            {
                ViewModel.ClearDiaryItem();
                // Navigate to the CoverPage
                Frame.Navigate(typeof(MainPage), ViewModel);

            }
        }
    }
}
