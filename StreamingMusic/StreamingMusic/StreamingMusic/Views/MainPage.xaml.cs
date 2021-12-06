using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using StreamingMusic.Interfaces;
using Xamarin.Essentials;
using Plugin.LatestVersion;
using System.Threading;
using Android.Media;

namespace StreamingMusic
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            label_title.Text = titleSongs[0];
            current_path = path[0];
            //CheckLatestVersion();
        }

        public void CheckLatestVersion()
        {
            bool isLatest = false;
            // Start a new task (this launches a new thread)
            Task.Factory.StartNew(async () =>
            {
                // Do some work on a background thread, allowing the UI to remain responsive
                isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();
                // When the background work is done, continue with this code block
            }).ContinueWith(async task =>
            {
                if (!isLatest)
                {
                    var update = await DisplayAlert("New Version", "There is a new version of this app available. Would you like to update now?", "Update", "Exit");
                    if (update)
                    {
                        await CrossLatestVersion.Current.OpenAppInStore();
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }
                    else
                    {
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }
                }
                
                // the following forces the code in the ContinueWith block to be run on the
                // calling thread, often the Main/UI thread.
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public readonly IMediaPlayerService MediaPlayerService = DependencyService.Get<IMediaPlayerService>();

        // Current app version
        readonly string currentVersion = VersionTracking.CurrentVersion;

        private bool DataSourceEmpty = true;
        public int current_number_songs = 0;
        public string current_path;

        
        // Title Songs
        public string[] titleSongs = {
                        "1. Whitesand - Dance of Strings",
                        "2. Solo Accoustic Guitar - Jason Shaw",
                        "3. Wonderful Acoustic Guitar"};

        // Stream Songs Link
        public string[] path = {
                        "https://drive.google.com/uc?id=1uwtTPC_oSLyDbpEJyHpGlfbt_bOxmrOW&export=download",
                        "https://drive.google.com/uc?id=1zPZpaQLnePn7-KQYOrBESf9hegXsVVcF&export=download",
                        "https://drive.google.com/uc?id=1vSfMj9iEqIqMIokJfsRkUhuBHbyKkUZ0&export=download"};

        private void ShowAdsInterstitial()
        {
            DependencyService.Get<IAdInterstitialService>().ShowAd();
        }
        private void ShowAdsAppOpen()
        {
            DependencyService.Get<IAdInterstitialService>().ShowAdAppOpen();
        }

        private void ShowAdsInterstitialWhen3Clicked()
        {
            int GetcountNpClick = DependencyService.Get<ILimitationInterstitialAds>().GetcountNpClick();
            int limitInterstitialAds = DependencyService.Get<ILimitationInterstitialAds>().GetlimitInterstitialAds();

            if (GetcountNpClick <= limitInterstitialAds)
            {
                if (GetcountNpClick%3 == 0)
                {
                    ShowAdsInterstitial();
                }
            }
        }

        private void ShowLoading(bool showloading)
        {
            if (showloading)
            {
                btn_play.IsVisible = false;
                btn_pause.IsVisible = false;
                btn_next.IsVisible = false;
                btn_previous.IsVisible = false;
                loading.IsVisible = true;
            }
            else
            {
                btn_play.IsVisible = true;
                btn_pause.IsVisible = true;
                btn_next.IsVisible = true;
                btn_previous.IsVisible = true;
                loading.IsVisible = false;
            }
        }

        private void ShowButtonPlayPause()
        {
            if (!DataSourceEmpty)
            {
                if (MediaPlayerService.IsPlaying())
                {
                    btn_play.IsVisible = false;
                    btn_pause.IsVisible = true;
                }
                else
                {
                    btn_play.IsVisible = true;
                    btn_pause.IsVisible = false;
                }
            }
            else
            {
                btn_play.IsVisible = true;
                btn_pause.IsVisible = false;
            }
        }

        private void ShowErrorMediaPlayer()
        {
            DisplayAlert("Error", "Media Player Not Found \nPlease check your internet connection", "OK");
        }

        private void Play(string path)
        {
            if (DataSourceEmpty)
            {
                ShowLoading(true);
            }
            // Start a new task (this launches a new thread)
            Task.Factory.StartNew(() =>
            {
                // Do some work on a background thread, allowing the UI to remain responsive
                if (MediaPlayerService.SetDataSource(path))
                {
                    DataSourceEmpty = false;
                }
                else
                {
                    DataSourceEmpty = true;
                }
                // When the background work is done, continue with this code block
            }).ContinueWith(task =>
            {
                if (!DataSourceEmpty)
                {
                    if (!MediaPlayerService.IsPlaying())
                    {
                        MediaPlayerService.Start();
                    }
                }
                else
                {
                    ShowErrorMediaPlayer();
                }
                ShowLoading(false);
                ShowButtonPlayPause();
                // the following forces the code in the ContinueWith block to be run on the
                // calling thread, often the Main/UI thread.
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Pause()
        {
            if (MediaPlayerService.IsPlaying())
            {
                MediaPlayerService.Pause();
                ShowButtonPlayPause();
            }
        }
        private void Next()
        {
            DependencyService.Get<ILimitationInterstitialAds>().IncrementcountNpClick();
            if (!DataSourceEmpty)
            {
                MediaPlayerService.Reset();
                DataSourceEmpty = true;
            }

            ShowAdsInterstitialWhen3Clicked();

            if (current_number_songs == 2)
            {
                current_number_songs = 0;
            }
            else
            {
                current_number_songs++;
            }

            label_title.Text = titleSongs[current_number_songs];
            current_path = path[current_number_songs];
            Play(current_path);
        }

        public void Previous()
        {
            DependencyService.Get<ILimitationInterstitialAds>().IncrementcountNpClick();
            if (!DataSourceEmpty)
            {
                MediaPlayerService.Reset();
                DataSourceEmpty = true;
            }

            ShowAdsInterstitialWhen3Clicked();

            if (current_number_songs == 0)
            {
                current_number_songs = 2;
            }
            else
            {
                current_number_songs -= 1;
            }

            label_title.Text = titleSongs[current_number_songs];
            current_path = path[current_number_songs];
            Play(current_path);
        }

        private void PlayButton(object sender, EventArgs e)
        {
            Play(current_path);
        }

        private void PauseButton(object sender, EventArgs e)
        {
            Pause();
        }

        private void NextButton(object sender, EventArgs e)
        {
            Next();
        }

        private void PreviousButton(object sender, EventArgs e)
        {
            Previous();
        }

        private async void AboutButton(object sender, EventArgs e)
        {
            await DisplayAlert("Relaxing Guitar Music", $"Source Image: BelleDeesse (WallpaperUP)\nSource Music: Whitesand (Youtube)\n\nVersion : {currentVersion}\n© 2021 Kolam Kode", "OK");
        }

        private void ShowAdsInterstitialButton(object sender, EventArgs e)
        {
            ShowAdsInterstitial();
        }
        private void ShowAdsOpenAppButton(object sender, EventArgs e)
        {
            ShowAdsAppOpen();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await DisplayAlert("Alert!", "Do you really want to exit the application?", "Yes", "No");
                if (result)
                {
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                }
            });
            return true;
        }
    }
}
