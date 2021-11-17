using System;
namespace StreamingMusic.DependencyServices
{
    public interface IAdInterstitial
    {
        void ShowAd();
        void ShowAdVideo();
        void ShowAdVideoAppOpen();
    }
}