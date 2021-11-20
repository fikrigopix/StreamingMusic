using Android.Gms.Ads.Interstitial;
using Android.Runtime;
using System;

public abstract class InterstitialCallback : InterstitialAdLoadCallback
{
    [Register("onAdLoaded", "(Lcom/google/android/gms/ads/interstitial/InterstitialAd;)V", "GetOnAdLoadedHandler")]

    public virtual void OnAdLoaded(InterstitialAd interstitialAd)
    {
        //Code run when Ads Loaded
    }

    private static Delegate cb_onAdLoaded;

    private static Delegate GetOnAdLoadedHandler()
    {
        if (cb_onAdLoaded is null)
            cb_onAdLoaded = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr>)n_onAdLoaded);
        return cb_onAdLoaded;
    }
    private static void n_onAdLoaded(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)
    {
        InterstitialCallback thisobject = GetObject<InterstitialCallback>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
        InterstitialAd resultobject = GetObject<InterstitialAd>(native_p0, JniHandleOwnership.DoNotTransfer);
        thisobject.OnAdLoaded(resultobject);
    }
}