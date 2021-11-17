using StreamingMusic.DependencyServices;
using Xamarin.Forms;

namespace StreamingMusic
{
    public partial class App : Application
    {
        public App(string app_label)
        {
            InitializeComponent();

            MainPage = new MainPage(app_label);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            //DependencyService.Get<IAdInterstitial>().ShowAdVideoAppOpen();
        }
    }
}
