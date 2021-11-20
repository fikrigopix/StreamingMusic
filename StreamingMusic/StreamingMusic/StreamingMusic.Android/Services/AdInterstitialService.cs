using Xamarin.Forms;
using StreamingMusic.Interfaces;
using Android.Gms.Ads;
using Android.Gms.Ads.Interstitial;

[assembly: Dependency(typeof(StreamingMusic.Droid.Services.AdInterstitialService))]
namespace StreamingMusic.Droid.Services
{
    public class AdInterstitialService : IAdInterstitialService
    {
        public void ShowAd()
        {
            // Admob Test AdunitID Interstitial
            string AdUnitId = "ca-app-pub-3940256099942544/1033173712";

            // Admob Real AdunitID Interstitial
            // string AdUnitId = "ca-app-pub-3940256099942544/1033173712";

            InterstitialAd.Load(Android.App.Application.Context, AdUnitId, new AdRequest.Builder().Build(), new InterstitialCallbackinherit());
        }

        public void ShowAdAppOpen()
        {
            // Admob Test AdunitID Interstitial Video
            string AdUnitId = "ca-app-pub-3940256099942544/3419835294";

            // Admob Real AdunitID Interstitial Video
            // string AdUnitId = "ca-app-pub-3940256099942544/3419835294";

            InterstitialAd.Load(Android.App.Application.Context, AdUnitId, new AdRequest.Builder().Build(), new InterstitialCallbackinherit());
        }
    }
}