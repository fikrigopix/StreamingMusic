using StreamingMusic.DependencyServices;
using StreamingMusic.Droid.DependencyServices;
using Android.Gms.Ads;
using Xamarin.Forms;
using Android.Gms.Ads.Interstitial;

[assembly: Dependency(typeof(AdInterstitial_Droid))]
namespace StreamingMusic.Droid.DependencyServices
{
    public class AdInterstitial_Droid : IAdInterstitial
    {
        public void ShowAd()
        {
            // Admob Test AdunitID Interstitial
            string AdUnitId = "ca-app-pub-3940256099942544/1033173712";

            // Admob Real AdunitID Interstitial
            // string AdUnitId = "ca-app-pub-3940256099942544/1033173712";

            InterstitialAd.Load(Android.App.Application.Context, AdUnitId, new AdRequest.Builder().Build(), new InterstitialCallbackinherit());
        }

        public void ShowAdVideo()
        {
            // Admob Test AdunitID Interstitial Video
            string AdUnitId = "ca-app-pub-3940256099942544/8691691433";

            // Admob Real AdunitID Interstitial Video
            // string AdUnitId = "ca-app-pub-3940256099942544/8691691433";

            InterstitialAd.Load(Android.App.Application.Context, AdUnitId, new AdRequest.Builder().Build(), new InterstitialCallbackinherit());
        }

        public void ShowAdVideoAppOpen()
        {
            // Admob Test AdunitID Interstitial Video
            string AdUnitId = "ca-app-pub-3940256099942544/3419835294";

            // Admob Real AdunitID Interstitial Video
            // string AdUnitId = "ca-app-pub-3940256099942544/3419835294";

            InterstitialAd.Load(Android.App.Application.Context, AdUnitId, new AdRequest.Builder().Build(), new InterstitialCallbackinherit());
        }
    }
}