using StreamingMusic.Interfaces;
using Xamarin.Forms;

namespace StreamingMusic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            DependencyService.Get<IMediaPlayerService>().InitMediaPlayer();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            //DependencyService.Get<IAdInterstitial>().ShowAdVideoAppOpen();
        }
    }
}
