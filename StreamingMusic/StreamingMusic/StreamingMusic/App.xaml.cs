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

        //protected override void OnStart()
        //{

        //}

        protected async override void OnStart()
        {
            // Initialize MediaPlayerService
            DependencyService.Get<IMediaPlayerService>().InitMediaPlayer();

            // test data string
            //var nama = "first string on mydatabase";

            // Insert Data
            //await DependencyService.Get<IDatabaseService>().Add(nama);

            // Get Data
            //var mydata = await DependencyService.Get<IDatabaseService>().GetData();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            //Show OpenApp Ads
            DependencyService.Get<IAdInterstitialService>().ShowAdAppOpen();
        }
    }
}
