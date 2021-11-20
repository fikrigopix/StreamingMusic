using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Ads;
using Android.Content;
using StreamingMusic.Interfaces;

[assembly: ExportRenderer(typeof(IAdBannerService), typeof(StreamingMusic.Droid.Services.AdBannerService))]
namespace StreamingMusic.Droid.Services
{
    public class AdBannerService : ViewRenderer
    {
        Context context;
        public AdBannerService(Context _context) : base(_context)
        {
            context = _context;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var adView = new AdView(Context);
                switch ((Element as IAdBannerService).Size)
                {
                    case IAdBannerService.Sizes.Standardbanner:
                        adView.AdSize = AdSize.Banner;
                        break;
                    case IAdBannerService.Sizes.LargeBanner:
                        adView.AdSize = AdSize.LargeBanner;
                        break;
                    case IAdBannerService.Sizes.MediumRectangle:
                        adView.AdSize = AdSize.MediumRectangle;
                        break;
                    case IAdBannerService.Sizes.FullBanner:
                        adView.AdSize = AdSize.FullBanner;
                        break;
                    case IAdBannerService.Sizes.Leaderboard:
                        adView.AdSize = AdSize.Leaderboard;
                        break;
                    //case AdBanner.Sizes.SmartBannerPortrait:
                    //    adView.AdSize = AdSize.SmartBanner;
                    //    break;
                    default:
                        adView.AdSize = AdSize.Banner;
                        break;
                }
                // TODO: change this id to your admob id  
                adView.AdUnitId = "ca-app-pub-3940256099942544/6300978111"; // Id Test Banner
                var requestbuilder = new AdRequest.Builder();
                adView.LoadAd(requestbuilder.Build());
                SetNativeControl(adView);
            }
        }
    }
}