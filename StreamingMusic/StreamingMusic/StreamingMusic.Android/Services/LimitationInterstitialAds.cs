using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StreamingMusic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(StreamingMusic.Droid.Services.LimitationInterstitialAds))]
namespace StreamingMusic.Droid.Services
{
    public class LimitationInterstitialAds : ILimitationInterstitialAds
    {
        public int limitInterstitialAds = 3;
        public int countNpClick = 0;
        DateTime latestOpenApp;

        public async Task ResetLatestOpenAppAsync()
        {
            countNpClick = 0;
            latestOpenApp = DateTime.Now;
            await DependencyService.Get<IDatabaseService>().Remove(1);
            await DependencyService.Get<IDatabaseService>().Add(latestOpenApp);
        }

        public void SetcountNpClick()
        {
            countNpClick ++;
        }

        public void SetlimitInterstitialAds(int SetlimitInterstitialAds)
        {
            limitInterstitialAds = SetlimitInterstitialAds;
        }

        public int GetcountNpClick()
        {
            return countNpClick;
        }

        public int GetlimitInterstitialAds()
        {
            return limitInterstitialAds;
        }
    }
}