using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace MemoryBox.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ViewModels.DiaryItemViewModel ViewModel { get; set; }


        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = ViewModels.DiaryItemViewModel.getViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            myFrame.Navigate(typeof(CoverPage));
        }

        private void hamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            splitView.IsPaneOpen = false;
            if (home.IsSelected)
            {
                changeToIndexPageStyle();
                myFrame.Padding = new Thickness(50, 0, 0, 0);
                myFrame.Navigate(typeof(CoverPage));
            }
            else if (creation.IsSelected)
            {
                changeToCreationPageStyle();
                myFrame.Padding = new Thickness(50, 0, 0, 0);
                myFrame.Navigate(typeof(EditPage));
            }
            else if (display.IsSelected)
            {
                changeToDisplayPageStyle();
                myFrame.Padding = new Thickness(0);
                myFrame.Navigate(typeof(DisplayPage));
            }
            else
            {
                changeToIndexPageStyle();
                myFrame.Padding = new Thickness(50, 0, 0, 0);
                myFrame.Navigate(typeof(IndexPage));
            }
        }

        private void changeToCreationPageStyle()
        {
            rootGrid.Background = null;
            hamburgerButton.Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
            hamburgerButton.Foreground = new SolidColorBrush(Colors.White);
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = null;
            viewTitleBar.ButtonBackgroundColor = null;
            splitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
        }

        private void changeToDisplayPageStyle()
        {
            rootGrid.Background = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));
            hamburgerButton.Background = new SolidColorBrush(Color.FromArgb(255, 242, 242, 242));
            hamburgerButton.Foreground = new SolidColorBrush(Colors.Black);
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Color.FromArgb(255, 242, 242, 242);
            viewTitleBar.ButtonBackgroundColor = Color.FromArgb(255, 242, 242, 242);
            splitView.DisplayMode = SplitViewDisplayMode.Overlay;
        }

        private void changeToIndexPageStyle()
        {
            rootGrid.Background = null;
            hamburgerButton.Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
            hamburgerButton.Foreground = new SolidColorBrush(Colors.White);
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = null;
            viewTitleBar.ButtonBackgroundColor = null;
            splitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
        }
 
    }
}
