using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using StreamingMusic.Interfaces;
using Xamarin.Essentials;

namespace StreamingMusic
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            label_title.Text = "Anjay";
            current_path = path[0];
        }

        public readonly IMediaPlayerService MediaPlayerService = DependencyService.Get<IMediaPlayerService>();

        // Current app version
        readonly string currentVersion = VersionTracking.CurrentVersion;

        private bool DataSourceEmpty = true;
        public int next_count = 0;
        public int current_number_songs = 0;
        public string current_path;

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
            if (!DataSourceEmpty)
            {
                MediaPlayerService.Reset();
                DataSourceEmpty = true;
            }
            
            if (current_number_songs == 2)
            {
                current_number_songs = 0;
            }
            else
            {
                current_number_songs++;
            }

            if (next_count == 2)
            {
                ShowAdsInterstitial();
                next_count = 0;
            }
            else
            {
                next_count++;
            }

            current_path = path[current_number_songs];
            Play(current_path);
        }

        public void Previous()
        {

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
