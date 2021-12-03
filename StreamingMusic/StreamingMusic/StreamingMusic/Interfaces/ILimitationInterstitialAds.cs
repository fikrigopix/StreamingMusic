using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMusic.Interfaces
{
    public interface ILimitationInterstitialAds
    {
        void IncrementcountNpClick();
        void SetlimitInterstitialAds(int SetlimitInterstitialAds);
        int GetcountNpClick();
        int GetlimitInterstitialAds();
        Task ResetLatestOpenAppAsync();
    }
}
