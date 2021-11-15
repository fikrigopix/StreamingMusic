using Android.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using StreamingMusic.Interfaces;

namespace StreamingMusic
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            localNotificationsService = DependencyService.Get<ILocalNotificationsService>();
        }

        public readonly ILocalNotificationsService localNotificationsService;
        protected MediaPlayer player;
        private bool IsMediaPlayerFound = false;

        private void ShowNotification()
        {
            localNotificationsService.ShowNotification("Relax Guitar Music", "Now Playing", new Dictionary<string, string>());
        }

        private void ShowErrorMediaPlayer()
        {
            DisplayAlert("Error", "Media Player Not Found \nPlease check your internet connection", "OK");
        }

        private void PlayButton(object sender, EventArgs e)
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
                InitMediaPlayer();
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
        private void InitMediaPlayer()
        {
            try
            {
                if (!IsMediaPlayerFound)
                {
                    string path = "https://drive.google.com/uc?id=1uwtTPC_oSLyDbpEJyHpGlfbt_bOxmrOW&export=download";

                    // For Testing Looping
                    // string path = "https://drive.google.com/uc?id=1DG6JwEJ3s9rolWwKAV8j8qEoyNtJFJ0G&export=download";

                    player = new MediaPlayer();
                    player.Reset();
                    player.SetDataSource(path);
                    player.Prepare();
                    player.SeekTo(4500); // starting from the first 4.5 seconds
                    IsMediaPlayerFound = true;
                }
            }
            catch (Exception)
            {
                IsMediaPlayerFound = false;
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

        private async void AboutButton(object sender, EventArgs e)
        {
            await DisplayAlert("Relax Guitar Music", "Source Image: BelleDeesse (WallpaperUP)\nSource Music: Whitesand (Youtube)\n\nVersion : 1.0.0\n© 2021 Kolam Kode", "OK");
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
