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
        //    DependencyService.Get<IMediaPlayerService>().InitMediaPlayer();
        //}

        protected async override void OnStart()
        {
            base.OnStart();
            DependencyService.Get<IMediaPlayerService>().InitMediaPlayer();

            // data string
            var nama = "first string on mydatabase";

            // Insert Data
            await DependencyService.Get<IDatabaseService>().Add(nama);

            // Get Data
            var mydata = await DependencyService.Get<IDatabaseService>().GetData();

            // Debug
            //var result = 0;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            DependencyService.Get<IAdInterstitialService>().ShowAdAppOpen();
        }
    }
}
