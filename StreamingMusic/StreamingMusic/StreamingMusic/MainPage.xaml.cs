using Android.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            localNotificationsService.ShowNotification("Relax Music", "Now Playing", new Dictionary<string, string>());
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
                    string path = "https://drive.google.com/uc?id=1Un3lFeU9G-pCiWuniD3T8FtmD2IpEXNT&export=download";

                    // For Testing Looping
                    // string path = "https://drive.google.com/uc?id=1DG6JwEJ3s9rolWwKAV8j8qEoyNtJFJ0G&export=download";

                    player = new MediaPlayer();
                    player.Reset();
                    player.SetDataSource(path);
                    player.Prepare();
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
            var urlSourceImage = new Uri("https://unsplash.com/photos/gd3ysFyrsTQ");
            var urlSourceMusic = new Uri("https://soundcloud.com/ashamaluevmusic2/sets/relaxing-music");

            string[] Credits = {
                "Relax Music",
                "Version : 1.0.0\n© 2021 Kolam Kode",
                $"Source Image:\n- Unsplash Cosmin Georgian\n- {urlSourceImage}",
                $"Source Music :\n- AShamaluevMusic - Music For Videos\n- {urlSourceMusic}",
                "Ok",
            };

            var result = await DisplayActionSheet(Credits[0], Credits[4], null, Credits[1], Credits[2], Credits[3]);

            if (result == Credits[2])
            {
                await OpenBrowser(urlSourceImage);
            }
            else if (result == Credits[3])
            {
                await OpenBrowser(urlSourceMusic);
            }
        }

        public async Task OpenBrowser(Uri uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                await DisplayAlert("An unexpected error occured", "No browser may be installed on the device", "OK");
            }
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
