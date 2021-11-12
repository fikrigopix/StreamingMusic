using Android.Media;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StreamingMusic
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = this;
            btn_play.IsVisible = true;
            btn_pause.IsVisible = false;

            var result = Task.Run(InitMediaPlayer).Result;
            if (!result)
            {
                ShowErrorMediaPlayer();
            }
            else
            {
                IsMediaPlayerFound = true;
            }
        }

        protected MediaPlayer player;
        private bool IsMediaPlayerFound = false;

        private void ShowErrorMediaPlayer()
        {
            DisplayAlert("Error", "Media Player Not Found \nPlease check your internet connection", "OK");
        }

        private async void PlayButton(object sender, EventArgs e)
        {
            if (!IsMediaPlayerFound)
            {
                loading.IsVisible = true;

                if (!await InitMediaPlayer())
                {
                    ShowErrorMediaPlayer();
                }
                else
                {
                    IsMediaPlayerFound = true;
                }

                loading.IsVisible = false;
            }
            else
            {
                if (!player.IsPlaying)
                {
                    btn_play.IsVisible = false;
                    btn_pause.IsVisible = true;
                    player.Start();
                }
            }
        }

        private async Task<bool> InitMediaPlayer()
        {
            try
            {
                if (!IsMediaPlayerFound)
                {
                    string path = "https://drive.google.com/uc?id=1Un3lFeU9G-pCiWuniD3T8FtmD2IpEXNT&export=download";
                    player = new MediaPlayer();
                    player.Reset();
                    await player.SetDataSourceAsync(path);
                    player.Prepare();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void PauseButton(object sender, EventArgs e)
        {
            if (player.IsPlaying)
            {
                player.Pause();
                btn_play.IsVisible = true;
                btn_pause.IsVisible = false;
            }
        }

        private async void AboutButton(object sender, EventArgs e)
        {
            await DisplayAlert("Relax Music", "Version : 1.0.0\n© 2021 Kolam Kode", "OK");
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
