using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MemoryBox.Models
{
    public class TodayLocation : INotifyPropertyChanged
    {
        private double longitude;
        private double latitude;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                this.longitude = value;
                this.OnPropertyChanged();
            }

        }
        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                this.latitude = value;
                this.OnPropertyChanged();
            }
        }
        public TodayLocation(double lgt = 0.0, double lat = 0.0)
        {
            this.Latitude = lat;
            this.Longitude = lgt;
        }

    }
}
