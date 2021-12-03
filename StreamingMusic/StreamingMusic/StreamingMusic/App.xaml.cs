using StreamingMusic.Interfaces;
using StreamingMusic.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        //protected async override void OnStart()
        //protected override void OnStart()
        //{

        //}

        public DateTime LatestOpenAppAds;

        protected async override void OnStart()
        {
            base.OnStart();
            // Set LatestOpenAppAds Now
            LatestOpenAppAds = DateTime.Now;

            // Initialize MediaPlayerService
            DependencyService.Get<IMediaPlayerService>().InitMediaPlayer();

            // var CheckMyData = await DependencyService.Get<IDatabaseService>().GetData(1);

            // If Database Null
            if (await DependencyService.Get<IDatabaseService>().GetData(1) == null)
            {
                await DependencyService.Get<ILimitationInterstitialAds>().ResetLatestOpenAppAsync();
            }
            else
            {
                // If latestOpenApp Difference from now
                var LatestOpenApp = await DependencyService.Get<IDatabaseService>().GetData(1);
                DateTime latestOpenApp = LatestOpenApp.latestOpenApp;

                TimeSpan DiffereceDate = DateTime.Now.Subtract(latestOpenApp);

                if (DiffereceDate.Days >= 1)
                {
                    await DependencyService.Get<ILimitationInterstitialAds>().ResetLatestOpenAppAsync();
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            TimeSpan DiffereceTime = DateTime.Now.Subtract(LatestOpenAppAds);

            if (DiffereceTime.Hours >= 1)
            {
                //Show OpenApp Ads
                DependencyService.Get<IAdInterstitialService>().ShowAdAppOpen();
            }
        }
    }
}
