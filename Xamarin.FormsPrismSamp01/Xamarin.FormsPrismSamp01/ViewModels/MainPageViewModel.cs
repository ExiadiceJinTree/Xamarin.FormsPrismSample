using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.FormsPrismSamp01.Conditions;
using Xamarin.FormsPrismSamp01.Objects;
using Xamarin.FormsPrismSamp01.Views;

namespace Xamarin.FormsPrismSamp01.ViewModels
{
    //====================================================================================================
    // [MVVM手法] ViewのCodeBehind側でなくViewModel側でロジックを実装する:
    //====================================================================================================
    public class MainPageViewModel : ViewModelBase
    {
        private IPageDialogService _pageDialogService;
        private IDependencyService _dependencyService;

        private string _bLabel;
        public string BLabel
        {
            get { return _bLabel; }
            set { SetProperty(ref _bLabel, value); }  // データバインディング値が変わった通知がされViewに反映される。
        }

        private Xamarin.Forms.ImageSource _cameraImageSource;
        public Xamarin.Forms.ImageSource CameraImageSource
        {
            get { return _cameraImageSource; }
            set { SetProperty(ref _cameraImageSource, value); }
        }

        public DelegateCommand BButtonClickCmd { get; set; }
        public DelegateCommand NavigateToBPageCmd { get; set; }
        public DelegateCommand MessageCmd { get; set; }
        public DelegateCommand InitCameraCmd { get; set; }

        // メッセージ表示機能を使うために、既定のコンストラクタを変更し、引数でIPageDialogServiceを受け取りプライベート変数に設定するよう変更。
        // Device毎の処理切り替えをするために、既定のコンストラクタを変更し、引数でIDependencyServiceを受け取りプライベート変数に設定するよう変更。
        //public MainPageViewModel(INavigationService navigationService)
        //    : base(navigationService)
        public MainPageViewModel(INavigationService navigationService,
                                 IPageDialogService pageDialogService,
                                 IDependencyService dependencyService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _dependencyService = dependencyService;

            BButtonClickCmd = new DelegateCommand(SetControlText);
            NavigateToBPageCmd = new DelegateCommand(ShowBPage);
            MessageCmd = new DelegateCommand(ShowMessageAsync);
            InitCameraCmd = new DelegateCommand(InitCameraAsync);

            Title = "Main Page";
            BLabel = "BBB";
        }

        private void SetControlText()
        {
            this.BLabel = "BButton was clicked ! - MVVM手法";
        }

        // MVVM手法での画面遷移
        private void ShowBPage()
        {
            // 引数なし画面遷移
            //this.NavigationService.NavigateAsync(nameof(BContentPage));

            // 引数あり画面遷移
            var naviParams = new NavigationParameters();
            //naviParams.Add("title", "XXXX");  // パラメータkeyが文字列リテラルなのがよくない
            naviParams.Add(nameof(BContentPageCondition), new BContentPageCondition("XXXX"));  // パラメータkeyが文字列でない、かつ条件用クラスオブジェクトを渡す方法。
            this.NavigationService.NavigateAsync(nameof(BContentPage), naviParams);
        }

        private async void ShowMessageAsync()
        {
            await _pageDialogService.DisplayAlertAsync("Title", "Message: specify only accept button string.", "OK");
            await _pageDialogService.DisplayAlertAsync("title", "Message: specify accept button and cancel button string.", "OK", "Cancel");

            // Device毎に処理切り替え実装
            string deviceName = _dependencyService.Get<IDevice>().GetDeviceName();
            await _pageDialogService.DisplayAlertAsync("Device Name", $"This device is {deviceName}.", "OK");
        }

        private async void InitCameraAsync()
        {
            // Plugin の初期化。おまじない
            await CrossMedia.Current.Initialize();

            // カメラが使用可能で、写真が撮影可能かを判定
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await _pageDialogService.DisplayAlertAsync("No Camera", ":( No camera available.", "OK");
                return;
            }

            // カメラが起動し写真を撮影する。撮影した写真はストレージに保存され、ファイルの情報が return される
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                //====================================================================================================
                // [Directories and File Names]
                // Any illegal characters will be removed and if the name of the file is a duplicate then a number will be appended to the end.
                // The default implementation is to specify a unique time code to each value.
                //====================================================================================================
                Directory = Assembly.GetExecutingAssembly().GetName().Name,
                Name = string.Format("{0}_{1}.jpg", Assembly.GetExecutingAssembly().GetName().Name, DateTime.Now.ToString("yyyyMMdd-HHmmssff")),

                //====================================================================================================
                // [Resize Photo Size]
                // By default the photo that is taken/picked is the maxiumum size and quality available. For most applications this is not needed and can be Resized.
                // This can be accomplished by adjusting the PhotoSize property on the options.
                // The easiest is to adjust it to Small, Medium, or Large, which is 25%, 50%, or 75% or the original.
                // This is only supported in Android & iOS. On UWP there is a different scale that is used based on these numbers to the respected resolutions UWP supports.
                // Or you can set to a custom percentage.
                //====================================================================================================
                PhotoSize = PhotoSize.Medium,
                //PhotoSize = PhotoSize.Custom,
                //CustomPhotoSize = 90, //Resize to 90% of original

                //====================================================================================================
                // [Photo Quality]
                // Set the CompressionQuality, which is a value from 0 the most compressed all the way to 100, which is no compression.
                // A good setting from testing is around 92. This is only supported in Android & iOS
                //====================================================================================================
                CompressionQuality = 92,

                //====================================================================================================
                // [Saving Photo / Video to Camera Roll / Gallery]
                // You can now save a photo or video to the camera roll/gallery.
                // When your user takes a photo it will still store temporary data, but also if needed make a copy to the public gallery (based on platform).
                // In the MediaFile you will now see a AlbumPath that you can query as well.
                // 
                // This will restult in 2 photos being saved for the photo. One in your private folder and one in a public directory that is shown.
                // - Get the public album path
                //      var aPpath = file.AlbumPath;
                // - Get private path
                //      var path = file.Path;
                // 
                // Android: When you set SaveToAlbum this will make it so your photos are public in the Pictures/YourDirectory or Movies/YourDirectory.
                // This is the only way Android can detect the photos.
                //====================================================================================================
                SaveToAlbum = true,

                //====================================================================================================
                // [Allow Cropping(トリミングを許可)]
                // Both iOS and UWP have crop controls built into the the camera control when taking a photo.
                // On iOS the default is false and UWP the default is true.
                //====================================================================================================
                AllowCropping = false,

                //====================================================================================================
                // [Default Camera]
                // By default when you take a photo or video the default system camera will be selected.
                // This option does not guarantee that the actual camera will be selected because each platform is different.
                // It seems to work extremely well on iOS, but not so much on Android. Your mileage may vary.
                //====================================================================================================
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,  // Front facing of device
                //DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,  // Back of device
            });

            // 端末のカメラ機能でユーザーが「キャンセル」した場合は、file が null となる
            if (file == null)
                return;

            await _pageDialogService.DisplayAlertAsync("File Location", file.Path, "OK");

            // ファイルを消せるようにするために、ファイル内容をbyte配列に格納
            byte[] fileBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                await file.GetStream().CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            // ファイル不要か確認し、不要なら削除
            var doDeletePhotoFile = await _pageDialogService.DisplayAlertAsync("Media file", "Do you delete this photo file ?", "Yes", "No");
            if (doDeletePhotoFile)
            {
                System.IO.File.Delete(file.Path);
                System.IO.File.Delete(file.AlbumPath);
                file.Dispose();
            }

            // 写真を画面上の image 要素に表示する
            CameraImageSource = Forms.ImageSource.FromStream(() =>
            {
                //var stream = file.GetStream();
                //return stream;
                return new MemoryStream(fileBytes);
            });
        }
    }
}
