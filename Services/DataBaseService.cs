using MemoryBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
//using SQLitePCL;
using Windows.UI.Xaml.Media.Imaging;
using SQLitePCL;

namespace MemoryBox.Services
{
    class DataBaseService
    {
        public static void InsertDiaryItem(DiaryItem item)
        {
            insertBasicInfo(item.DiaryItemID, item.Description, item.Date.ToString(), item.MoodIndex);
            insertLocation(item.DiaryItemID, item.CurLoc.Longitude.ToString(), item.CurLoc.Latitude.ToString());
            insertWeather(item.DiaryItemID, item.CurWeather.Weather, item.CurWeather.Location, item.CurWeather.Temperature);
            InsertImage(item.Images, item.DiaryItemID);
        }

        internal static ObservableCollection<DiaryItem> GetAllItems()
        {
            var IDs = getIDs();
            ObservableCollection<DiaryItem> x = new ObservableCollection<DiaryItem>();
            if (IDs == null)
            {
                return x;
            }
            foreach (string id in IDs)
            {
                var temp = FindDiaryItem(id);
                x.Add(temp);
            }
            return x;
        }
        private static List<string> getIDs()
        {
            var connitem = App.connItem;
            List<string> IDs = new List<string>();
            using (var statement = connitem.Prepare("SELECT DiaryItemID FROM MemoryBoxItemTable"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    string DiaryItemID = (string)statement[0];
                    IDs.Add(DiaryItemID);
                }
            }

            return IDs;
        }
        public static DiaryItem FindDiaryItem(string DiaryItemID)
        {
            var connitem = App.connItem;
            DiaryItem item = new DiaryItem(DateTimeOffset.Now);
            item.DiaryItemID = DiaryItemID;
            using (var statement = connitem.Prepare("SELECT  Description, Date, MoodIndex FROM MemoryBoxItemTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                SQLiteResult result = statement.Step();
                if (SQLiteResult.ROW == result)
                {
                    item.Description = (string)statement[0];
                    item.Date = DateTimeOffset.Parse(statement[1] as string);
                    item.MoodIndex = double.Parse((string)statement[2]);
                }
            }
            using (var statement = connitem.Prepare("SELECT Longitude, Latitude FROM LocationTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                SQLiteResult result = statement.Step();
                if (SQLiteResult.ROW == result)
                {
                    item.CurLoc.Longitude = double.Parse(statement[0] as string);
                    item.CurLoc.Latitude = double.Parse(statement[1] as string);
                }
            }

            using (var statement = connitem.Prepare("SELECT Weather, Temperature,Location FROM WeatherTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                SQLiteResult result = statement.Step();
                if (SQLiteResult.ROW == result)
                {
                    item.CurWeather.Weather = statement[0] as string;
                    item.CurWeather.Temperature = statement[1] as string;
                    item.CurWeather.Location = statement[2] as string;

                }
            }
            using (var statement = connitem.Prepare("SELECT Uri FROM ImageTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                while (SQLiteResult.ROW == statement.Step())
                {

                    Uri i = new Uri(statement[0] as string);
                    item.Images.Add(i);
                }
            }
            return item;
        }
        public static void RemoveDiaryItem(string DiaryItemID)
        {
            var connitem = App.connItem;
            using (var statement = connitem.Prepare("DELETE FROM MemoryBoxItemTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                statement.Step();
            }
            using (var statement = connitem.Prepare("DELETE FROM LocationTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                statement.Step();
            }
            using (var statement = connitem.Prepare("DELETE FROM WeatherTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                statement.Step();
            }
            using (var statement = connitem.Prepare("DELETE FROM ImageTable WHERE DiaryItemID=?"))
            {
                statement.Bind(1, DiaryItemID);
                statement.Step();
            }
        }

        public static void ClearAll()
        {
            var connitem = App.connItem;
            using (var statement = connitem.Prepare("DELETE FROM MemoryBoxItemTable"))
            {
                statement.Step();
            }
            using (var statement = connitem.Prepare("DELETE FROM LocationTable"))
            {
                statement.Step();
            }
            using (var statement = connitem.Prepare("DELETE FROM WeatherTable"))
            {
                statement.Step();
            }
            using (var statement = connitem.Prepare("DELETE FROM ImageTable"))
            {
                statement.Step();
            }
        }
        public static void UpdateDiaryItem(DiaryItem item)
        {
            updateBasicInfo(item.DiaryItemID, item.Description, item.Date.ToString(), item.MoodIndex);
            updateLocation(item.DiaryItemID, item.CurLoc.Longitude.ToString(), item.CurLoc.Latitude.ToString());
            updateWeather(item.DiaryItemID, item.CurWeather.Weather, item.CurWeather.Location, item.CurWeather.Temperature);
        }


        public static void insertBasicInfo(string DiaryItemID, string Description, string Date, double MoodIndex)
        {
            var connitem = App.connItem;
            try
            {
                using (var statement = connitem.Prepare("INSERT INTO MemoryBoxItemTable (DiaryItemID,Description,Date,MoodIndex) VALUES (?,?,?,?)"))
                {
                    statement.Bind(1, DiaryItemID);
                    statement.Bind(2, Description);
                    statement.Bind(3, Date);
                    statement.Bind(4, MoodIndex.ToString());
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.Message).ShowAsync();
            }

        }
        public static void insertLocation(string DiaryItemID, string Longitude, string Latitude)
        {
            var connitem = App.connItem;
            try
            {
                using (var statement = connitem.Prepare("INSERT INTO LocationTable (DiaryItemID,Longitude,Latitude) VALUES (?,?,?)"))
                {
                    statement.Bind(1, DiaryItemID);
                    statement.Bind(2, Longitude);
                    statement.Bind(3, Latitude);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.Message).ShowAsync();
            }
        }
        public static void insertWeather(string DiaryItemID, string Weather, string Location, string Temperature)
        {
            var connitem = App.connItem;
            try
            {
                using (var statement = connitem.Prepare("INSERT INTO WeatherTable (DiaryItemID,Weather,Temperature,Location) VALUES (?,?,?,?)"))
                {
                    statement.Bind(1, DiaryItemID);
                    statement.Bind(2, Weather);
                    statement.Bind(3, Temperature);
                    statement.Bind(4, Location);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.Message).ShowAsync();
            }
        }

        public static void updateBasicInfo(string DiaryItemID, string Description, string Date, double MoodIndex)
        {
            var connitem = App.connItem;
            try
            {
                using (var statement = connitem.Prepare("UPDATE MemoryBoxItemTable SET Description=?,Date=?,MoodIndex=? WHERE DiaryItemID=?"))
                {

                    statement.Bind(1, Description);
                    statement.Bind(2, Date);
                    statement.Bind(3, MoodIndex.ToString());
                    statement.Bind(4, DiaryItemID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.Message).ShowAsync();
            }
        }
        public static void updateLocation(string DiaryItemID, string Longitude, string Latitude)
        {
            var connitem = App.connItem;
            try
            {
                using (var statement = connitem.Prepare("UPDATE LocationTable SET Longitude=?,Latitude=? WHERE DiaryItemID=?"))
                {

                    statement.Bind(1, Longitude);
                    statement.Bind(2, Latitude);
                    statement.Bind(3, DiaryItemID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.Message).ShowAsync();
            }
        }
        public static void updateWeather(string DiaryItemID, string Weather, string Location, string Temperature)
        {
            var connitem = App.connItem;
            try
            {
                using (var statement = connitem.Prepare("UPDATE WeatherTable SET Weather=?,Temperature=?,Location=? WHERE DiaryItemID=?"))
                {

                    statement.Bind(1, Weather);
                    statement.Bind(2, Temperature);
                    statement.Bind(3, Location);
                    statement.Bind(4, DiaryItemID);
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.Message).ShowAsync();
            }
        }
        public static void updateImage(ObservableCollection<Uri> imgs, string DiaryItemID)
        {
            InsertImage(imgs, DiaryItemID);
        }
        private static void removeAllImage(string DiaryItemID)
        {
            var connitem = App.connItem;
            using (var statement = connitem.Prepare("DELETE FROM ImageTable WHERE DiaryItemID=? "))
            {
                statement.Bind(1, DiaryItemID);
                statement.Step();
            }
        }
        public static void removeImage(string DiaryItemID, int ImgId)
        {
            var connitem = App.connItem;
            using (var statement = connitem.Prepare("DELETE FROM ImageTable WHERE DiaryItemID=? AND ImageID=?"))
            {
                statement.Bind(1, DiaryItemID);
                statement.Bind(2, ImgId);
                statement.Step();
            }
        }
        public static void InsertImage(ObservableCollection<Uri> imgs, string DiaryItemID)
        {
            //          removeAllImage(DiaryItemID);
            int i = 0;
            foreach (Uri item in imgs)
            {
                insertImage(item, i, DiaryItemID);
                i++;
            }
        }
        private static void insertImage(Uri img, int ImageID, string DiaryItemID)
        {
            var connitem = App.connItem;
            try
            {
                using (var statement = connitem.Prepare("INSERT INTO ImageTable(DiaryItemID,ImageID,Uri) VALUES (?,?,?)"))
                {
                    statement.Bind(1, DiaryItemID);
                    statement.Bind(2, ImageID);
                    statement.Bind(3, img.ToString());
                    statement.Step();
                }
            }
            catch (Exception ex)
            {
                var i = new MessageDialog(ex.Message).ShowAsync();
            }
        }
    }
}
