using Android.Gms.Ads;
using Android.Gms.Ads.Interstitial;
using StreamingMusic.Droid;

public class InterstitialCallbackinherit : InterstitialCallback
{
    public override void OnAdLoaded(InterstitialAd interstitialAd)
    {
        interstitialAd.Show(MainActivity.instance);
        base.OnAdLoaded(interstitialAd);
    }

    public override void OnAdFailedToLoad(LoadAdError p0)
    {
        base.OnAdFailedToLoad(p0);
    }
}