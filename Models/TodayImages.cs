using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MemoryBox.Models
{
    class TodayImages
    {
        private ObservableCollection<Uri> images;
        public ObservableCollection<Uri> Images
        {
            get
            {
                return this.images;
            }  
            set
            {
                this.images =value;
            }
        }
        public TodayImages(ObservableCollection<Uri> imgs=null)
        {
            if (imgs == null)
            {
                images = new ObservableCollection<Uri>();
            }
            else
            {
                images = imgs;
            }
        }
        public Uri removeImage(int id)
        {
            Uri temp = images[id];
            images.RemoveAt(id);
            return temp;
        }
        public void addImage(Uri item)
        {
            images.Add(item);
 
        }
    }
}
