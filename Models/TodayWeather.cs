using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Models
{
    public class TodayWeather : INotifyPropertyChanged
    {
        private string location;
        private string weather;
        private string temperature;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
                this.OnPropertyChanged();
            }
        }
        public string Weather
        {
            get
            {
                return this.weather;
            }
            set
            {
                this.weather = value;
                this.OnPropertyChanged();
            }
        }
        public string Temperature
        {
            get
            {
                return this.temperature;
            }
            set
            {
                this.temperature = value;
                this.OnPropertyChanged();
            }
        }
        public TodayWeather(string loc = "", string wea = "", string tem = "")
        {
            this.Location = loc;
            this.Weather = wea;
            this.Temperature = tem;
        }
    }
}
