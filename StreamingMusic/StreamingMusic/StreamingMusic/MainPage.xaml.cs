using Android.Media;
using System;
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
            if (player == null)
            {
                player = new MediaPlayer();
            }
        }

        protected MediaPlayer player;
        public void StartPlayer(String filePath)
        {
            player.Reset();
            player.SetDataSource(filePath);
            player.Prepare();
            player.Start();
        }

        private void PlayButton(object sender, EventArgs e)
        {
            //tampilkan loading bar
            ShowLoadingBar(true);

            if (PlayMusic())
            {
                ShowLoadingBar(false);
            }
            else
            {
                ShowLoadingBar(false);
                //ShowErrorPlayMusic();
            }
        }

        private bool ShowLoadingBar(bool showloadingbar)
        {
            if (showloadingbar == true)
            {
                loading.IsVisible = true;
                btn_play.IsVisible = false;
                btn_pause.IsVisible = false;
                return true;
            }
            else
            {
                loading.IsVisible = false;
                btn_pause.IsVisible = true;
                return false;
            }
        }

        private bool PlayMusic()
        {
            if (!player.IsPlaying)
            {
                try
                {
                    string path = "https://drive.google.com/uc?id=1Un3lFeU9G-pCiWuniD3T8FtmD2IpEXNT&export=download";
                    StartPlayer(path);

                }
                catch (Exception)
                {
                    return false;
                }

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
