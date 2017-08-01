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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MemoryBox.Templates
{
    public sealed partial class ThumbnailTemplate : UserControl
    {
        public Models.DiaryItem Diary { get { return this.DataContext as Models.DiaryItem; } }

        public ThumbnailTemplate()
        {
            this.InitializeComponent();
            ExitStoryboard.Begin();
            //this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private void thumbnail_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            EnterStoryboard.Begin();
        }

        private void thumbnail_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ExitStoryboard.Begin();
        }
    }
}
