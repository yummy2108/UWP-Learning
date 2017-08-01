using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace MemoryBox.Converters
{
    class LocFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Models.TodayLocation temp = (Models.TodayLocation)value;
            // Specify a known location.
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = temp.Latitude, Longitude = temp.Longitude };

            return new Geopoint(cityPosition);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
