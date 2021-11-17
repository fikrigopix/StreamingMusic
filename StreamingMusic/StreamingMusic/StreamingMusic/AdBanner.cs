using System;  
using Xamarin.Forms;  
  
namespace StreamingMusic.CustomRenders
{
    public class AdBanner : View
    {
        //public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard, SmartBannerPortrait }
        public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard }
        public Sizes Size { get; set; }
        public AdBanner()
        {
            this.BackgroundColor = Color.Accent;
        }
    }
}