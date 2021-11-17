using Android.Gms.Ads;
using StreamingMusic.Droid;

public class InterstitialCallbackinherit : InterstitialCallback
{

    public override void OnAdLoaded(Android.Gms.Ads.Interstitial.InterstitialAd interstitialAd)
    {
        //MainActivity.interstitialAd = interstitialAd;
        interstitialAd.Show(MainActivity.instance);
        base.OnAdLoaded(interstitialAd);
    }
    public override void OnAdFailedToLoad(LoadAdError p0)
    {
        base.OnAdFailedToLoad(p0);
    }
}