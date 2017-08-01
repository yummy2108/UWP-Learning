using MemoryBox.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MemoryBox.Templates
{
    public sealed partial class DiaryTemplate : UserControl
    {
        public Models.DiaryItem Diary { get { return this.DataContext as Models.DiaryItem; } }

        public DiaryTemplate()
        {
            this.InitializeComponent();
            //location();

            //this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private async void location()
        {

                    // Get the current location.
                    Geolocator geolocator = new Geolocator();
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    Geopoint myLocation = pos.Coordinate.Point;

                    // Set the map location.
                    map.Center = myLocation;
                    map.ZoomLevel = 15;
                    map.LandmarksVisible = true;
        }

        private void diary_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            StackPanelEnterStoryboard.Begin();
        }

        private void diary_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Window.Current.Bounds.Width > 600)
            {
                StackPanelExitStoryboard.Begin();
            }
        }

        private void dropDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (map.Visibility == Visibility.Collapsed)
            {
                map.Visibility = Visibility.Visible;
            }
            else
            {
                map.Visibility = Visibility.Collapsed;
            }
        }
    }
}
