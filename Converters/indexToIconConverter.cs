using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace MemoryBox.Converters
{
    class IndexToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double moodIndex = (double)value;
            if (moodIndex == -1)
            {
                return null;
            }
            string moodIconName = "";

            if (moodIndex < 25)
            {
                // sad
                moodIconName = "sad.png";
            }
            else if (moodIndex < 50)
            {
                // smile
                moodIconName = "smile.png";
            }
            else if (moodIndex < 75)
            {
                // happy
                moodIconName = "happy.png";
            }
            else
            {
                // very happy
                moodIconName = "veryHappy.png";
            }
            return new BitmapImage(new Uri("ms-appx:///Assets/" + moodIconName));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
