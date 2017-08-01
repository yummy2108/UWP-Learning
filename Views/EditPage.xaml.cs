using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace MemoryBox.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditPage : Page
    {
        private ObservableCollection<Uri> tempImages = new ObservableCollection<Uri>();

        ViewModels.DiaryItemViewModel ViewModel { get; set; }

        private double latitude;
        private double longitude;
        private double index = -1;
        private string curTemperature;

        //是否语音输入全局变量
        bool wetherlisten = false;

        public EditPage()
        {
            this.InitializeComponent();
            this.ViewModel = ViewModels.DiaryItemViewModel.getViewModel();
            date.Text = DateTime.Now.ToString("M月d日, ddd");
        }

        private async void locationButton_Checked(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // Get the current location.
                    Geolocator geolocator = new Geolocator();
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    Geopoint myLocation = pos.Coordinate.Point;
                    latitude = pos.Coordinate.Point.Position.Latitude;
                    longitude = pos.Coordinate.Point.Position.Longitude;
                    MapIcon mapIcon1 = new MapIcon();
                    mapIcon1.Location = myLocation;
                    mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    mapIcon1.Title = "My Location";
                    mapIcon1.ZIndex = 0;
                    map.MapElements.Add(mapIcon1);

                    // Set the map location.
                    map.Center = myLocation;
                    map.ZoomLevel = 15;
                    map.LandmarksVisible = true;
                    getWeather(latitude, longitude);
                    break;

                case GeolocationAccessStatus.Denied:
                    // Handle the case  if access to location is denied.
                    break;

                case GeolocationAccessStatus.Unspecified:
                    // Handle the case if  an unspecified error occurs.
                    break;
            }
        }

        //获取天气
        public async void getWeather(double lat, double lon)
        {
            string url = "https://free-api.heweather.com/v5/now?city=" + lon + "," + lat + "&key=b8ab3754c6cb49aa845817097ac4e095";
            HttpClient client = new HttpClient();
            // 异步请求
            string result = await client.GetStringAsync(url);
            // Json解析
            JObject jo = (JObject)JsonConvert.DeserializeObject(result);
            // 异常判断
            if (jo["HeWeather5"][0]["status"].ToString() != "ok")
            {
                this.locationText.Text = "未知城市";
                return;
            }
            //string updateTime = jo["HeWeather5"][0]["basic"]["update"]["loc"].ToString();
            string weatherCond = jo["HeWeather5"][0]["now"]["cond"]["txt"].ToString();
            string condCode = jo["HeWeather5"][0]["now"]["cond"]["code"].ToString();
            string temperature = jo["HeWeather5"][0]["now"]["tmp"].ToString();
            string city = jo["HeWeather5"][0]["basic"]["city"].ToString();
            curTemperature = temperature + "℃";
            string weatherIconUrl = "http://files.heweather.com/cond_icon/" + condCode + ".png";
            this.locationText.Text = city;
            this.weatherIcon.Source = new BitmapImage(new Uri(weatherIconUrl));
            this.weather.Text = weatherCond;
            ToolTipService.SetToolTip(weatherPanel, curTemperature);
        }

        // show or hide the map
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

        private async void cameraButton_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();

            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);


            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                return;
            }

            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
                                                                       BitmapPixelFormat.Bgra8,
                                                                       BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            myPhoto.Source = bitmapSource;
            string picture = await StorageFileToBase64(photo);
            getmotion(picture);
        }

        private async void getmotion(string picture)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                        {
                        {"api_key","bgQzh1aJ17uBma0I9b5aTNOZUfe6-Xth"},
                        {"api_secret","id3-SWeI33QC8LseISq_15x3AzgZgNQB"},
                        {"image_base64", picture},
                        {"return_landmark","1"},
                        {"return_attributes","smiling" },
                        });
                var response = await client.PostAsync("https://api-cn.faceplusplus.com/facepp/v3/detect", content);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync(); // 返回结果

                    JsonObject res = JsonObject.Parse(result);
                    JsonArray face = res.GetNamedArray("faces");
                    if (face.Count == 0)
                    {
                        await new MessageDialog("照片识别出错，请重新上传 ").ShowAsync();
                    }
                    else
                    {
                        int attr = result.LastIndexOf("value");
                        int end = result.IndexOf("face_rectangle");
                        string temp = result.Substring(attr, end - attr);
                        int index1 = temp.IndexOf(":");
                        int index2 = temp.IndexOf("}");
                        string sm = temp.Substring(index1 + 1, index2 - index1 - 1);
                        index = double.Parse(sm);
                        string moodIconName = getMoodIconByIndex(index);
                        moodIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + moodIconName));
                        moodIndex.Text = "Mood Index : " + sm;
                        moodButton.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    await new MessageDialog("照片识别出错，请重新上传 " + response.StatusCode).ShowAsync();
                }
            };
        }

        private string getMoodIconByIndex(double moodIndex)
        {
            if (moodIndex < 25)
            {
                // sad
                return "sad.png";
            }
            else if (moodIndex < 50)
            {
                // smile
                return "smile.png";
            }
            else if (moodIndex < 75)
            {
                // happy
                return "happy.png";
            }
            else
            {
                // very happy
                return "veryHappy.png";
            }
        }

        private async Task<string> StorageFileToBase64(StorageFile file)
        {
            string Base64String = "";

            if (file != null)
            {
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                var reader = new DataReader(fileStream.GetInputStreamAt(0));
                await reader.LoadAsync((uint)fileStream.Size);
                byte[] byteArray = new byte[fileStream.Size];
                reader.ReadBytes(byteArray);
                Base64String = Convert.ToBase64String(byteArray);
            }

            return Base64String;
        }

        private async void ASRButton_Checked(object sender, RoutedEventArgs e)
        {
            wetherlisten = true;
            var speechRecognizer = new SpeechRecognizer();
            var constraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
            speechRecognizer.Constraints.Add(constraint);
            await speechRecognizer.CompileConstraintsAsync();
            while (wetherlisten)
            {
                waitingRing.IsActive = true;
                var result = await speechRecognizer.RecognizeAsync();
                if (result != null)
                    mainBody.Text += result.Text;
                waitingRing.IsActive = false;
            }
        }

        private void ASRButton_Unchecked(object sender, RoutedEventArgs e)
        {
            wetherlisten = false;
        }

        private async void imageButton_Click(object sender, RoutedEventArgs e)
        {
            var image = new Image();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(file.Name) == null)
                {
                    await file.CopyAsync(ApplicationData.Current.LocalFolder, file.Name, NameCollisionOption.ReplaceExisting);
                }
                if (!tempImages.Contains(new Uri("ms-appdata:///local/" + file.Name)))
                {
                    tempImages.Add(new Uri("ms-appdata:///local/" + file.Name)); ;
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // clear content
            mainBody.Text = "";
            tempImages.Clear();
            moodButton.Visibility = Visibility.Collapsed;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // add new diary item
            ViewModel.AddDiaryItem(new Models.DiaryItem(DateTimeOffset.Now,
                                                        longitude, latitude,
                                                        mainBody.Text,
                                                        weather.Text,
                                                        locationText.Text,
                                                        curTemperature,
                                                        index,
                                                        tempImages));
            // Navigate to the CoverPage
            var myFrame = this.Parent as Frame;
            myFrame.Navigate(typeof(CoverPage));
        }
    }
}
