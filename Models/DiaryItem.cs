using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace MemoryBox.Models
{
    public class DiaryItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private ObservableCollection<Uri> images = new ObservableCollection<Uri>();
        public ObservableCollection<Uri> Images
        {
            get
            {
                return this.images;
            }
            set
            {
                this.images = value;
                this.OnPropertyChanged();
            }
        }
        private DateTimeOffset date = DateTimeOffset.Now;
        public DateTimeOffset Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
                this.OnPropertyChanged();
            }
        }
        private string description = "";
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                this.OnPropertyChanged();
            }
        }
        private string DiaryItemid = "";
        public string DiaryItemID
        {
            get
            {
                return this.DiaryItemid;
            }
            set
            {
                this.DiaryItemid = value;
                OnPropertyChanged();
            }

        }
        private TodayLocation curLoc = new TodayLocation();
        public TodayLocation CurLoc
        {
            get
            {
                return this.curLoc;
            }
            set
            {
                this.curLoc.Latitude = value.Latitude;
                this.curLoc.Longitude = value.Longitude;
            }
        }
        private TodayWeather curWeather = new TodayWeather();
        public TodayWeather CurWeather
        {
            get
            {
                return this.curWeather;
            }
            set
            {
                this.curWeather.Location = value.Location;
                this.curWeather.Weather = value.Weather;
                this.curWeather.Temperature = value.Temperature;
                this.OnPropertyChanged();
            }
        }
        private double moodIndex;
        public double MoodIndex
        {
            get
            {
                return this.moodIndex;
            }
            set
            {
                this.moodIndex = value;
                this.OnPropertyChanged();
            }
        }
        public DiaryItem(DateTimeOffset date,
            double longitude = 0, double latitude = 0,
            string description = "",
            string weather = "",
            string location = "",
            string temperature = "",
            // using -1 to represent null
            double moodIndex = -1,
            ObservableCollection<Uri> images = null,
            string DiaryItemID = null)
        {
            if (DiaryItemID == null)
            {
                this.DiaryItemID = Guid.NewGuid().ToString();
            }
            else
            {
                this.DiaryItemID = DiaryItemID;
            }
            TodayLocation loc = new TodayLocation(longitude, latitude);
            TodayWeather wea = new TodayWeather(location, weather, temperature);
            this.CurLoc = loc;
            this.CurWeather = wea;
            if (images == null)
            {
                images = new ObservableCollection<Uri>();

            }
            this.images = images;
            this.Description = description;
            this.Date = date;
            this.MoodIndex = moodIndex;
        }
    }
}
