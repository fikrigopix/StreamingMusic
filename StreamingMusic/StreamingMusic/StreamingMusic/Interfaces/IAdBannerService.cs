using Xamarin.Forms;

namespace StreamingMusic.Interfaces
{
    public class IAdBannerService : View
    {
        //public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard, SmartBannerPortrait }
        public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard }
        public Sizes Size { get; set; }
        public IAdBannerService()
        {
            this.BackgroundColor = Color.Accent;
        }
    }
}