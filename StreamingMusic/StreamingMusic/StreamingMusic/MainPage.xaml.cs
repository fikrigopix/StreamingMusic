using Android.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using StreamingMusic.Interfaces;
using Xamarin.Essentials;
using StreamingMusic.DependencyServices;
using System.Linq;

namespace StreamingMusic
{
    public partial class MainPage : ContentPage
    {
        public MainPage(string app_label)
        {
            InitializeComponent();
            localNotificationsService = DependencyService.Get<ILocalNotificationsService>();
            app_name = app_label;
            current_path = path[0];
        }

        // Aplication Name
        string app_name;
        // Current app version
        string currentVersion = VersionTracking.CurrentVersion;

        public readonly ILocalNotificationsService localNotificationsService;
        protected MediaPlayer player;
        private bool IsMediaPlayerFound = false;
        // Stream Songs Link
        public string[] path = {
                        "https://drive.google.com/uc?id=1uwtTPC_oSLyDbpEJyHpGlfbt_bOxmrOW&export=download",
                        "https://drive.google.com/uc?id=1zPZpaQLnePn7-KQYOrBESf9hegXsVVcF&export=download",
                        "https://drive.google.com/uc?id=1vSfMj9iEqIqMIokJfsRkUhuBHbyKkUZ0&export=download"};
        public int next_count = 0;
        public int current_number_songs = 0;
        public string current_path;

        private void ShowNotification()
        {
            localNotificationsService.ShowNotification($"{app_name}", "Now Playing", new Dictionary<string, string>());
        }

        private void ShowErrorMediaPlayer()
        {
            DisplayAlert("Error", "Media Player Not Found \nPlease check your internet connection", "OK");
        }

        private void InterstitialButton(object sender, EventArgs e)
        {
            DependencyService.Get<IAdInterstitial>().ShowAd();
        }
        private void InterstitialVideoButton(object sender, EventArgs e)
        {
            DependencyService.Get<IAdInterstitial>().ShowAdVideo();
        }

        private void PlayButton(object sender, EventArgs e)
        {
            Play(current_path);
        }

        private void Play(string path)
        {
            if (!IsMediaPlayerFound)
            {
                btn_play.IsVisible = false;
                btn_pause.IsVisible = false;
                loading.IsVisible = true;
            }
            // Start a new task (this launches a new thread)
            Task.Factory.StartNew(() =>
            {
                // Do some work on a background thread, allowing the UI to remain responsive
                InitMediaPlayer(path);
                // When the background work is done, continue with this code block
            }).ContinueWith(task =>
            {
                loading.IsVisible = false;

                if (IsMediaPlayerFound)
                {
                    if (!player.IsPlaying)
                    {
                        btn_play.IsVisible = false;
                        btn_pause.IsVisible = true;
                        player.Looping = true;
                        player.Start();
                        ShowNotification();
                    }
                }
                else
                {
                    ShowErrorMediaPlayer();
                    btn_play.IsVisible = true;
                    btn_pause.IsVisible = false;
                }
                // the following forces the code in the ContinueWith block to be run on the
                // calling thread, often the Main/UI thread.
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void InitMediaPlayer(string path)
        {
            try
            {
                if (!IsMediaPlayerFound)
                {
                    // For Testing Looping
                    // string path = "https://drive.google.com/uc?id=1DG6JwEJ3s9rolWwKAV8j8qEoyNtJFJ0G&export=download";

                    player = new MediaPlayer();
                    player.Reset();
                    player.SetDataSource(path);
                    player.Prepare();
                    //player.SeekTo(4500); // starting from the first 4.5 seconds
                    IsMediaPlayerFound = true;
                }
            }
            catch (Exception)
            {
                IsMediaPlayerFound = false;
            }
        }

        public void PauseWhenShowAds()
        {
            if (IsMediaPlayerFound)
            {
                if (player.IsPlaying)
                {
                    player.Pause();
                    btn_play.IsVisible = true;
                    btn_pause.IsVisible = false;
                    localNotificationsService.HideNotification();
                }
            }
        }


        private void PauseButton(object sender, EventArgs e)
        {
            if (player.IsPlaying)
            {
                player.Pause();
                btn_play.IsVisible = true;
                btn_pause.IsVisible = false;
                localNotificationsService.HideNotification();
            }
        }

        private void NextButton(object sender, EventArgs e)
        {
            if (IsMediaPlayerFound)
            {
                if (player.IsPlaying)
                {
                    player.Release();
                    localNotificationsService.HideNotification();
                }
            }

            current_number_songs++;
            IsMediaPlayerFound = false;
            if (current_number_songs == path.Count())
            {
                current_number_songs = 0;
            }
            if (next_count == 2)
            {
                PauseWhenShowAds();
                DependencyService.Get<IAdInterstitial>().ShowAd();
                next_count = 0;
            }
            current_path = path[current_number_songs];
            Play(current_path);
            next_count++;
        }

        private async void AboutButton(object sender, EventArgs e)
        {
            await DisplayAlert($"{app_name}", $"Source Image: BelleDeesse (WallpaperUP)\nSource Music: Whitesand (Youtube)\n\nVersion : {currentVersion}\n© 2021 Kolam Kode", "OK");
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

        private void ads_banner_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}
