using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using StreamingMusic.Interfaces;
using System.Collections.Generic;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(StreamingMusic.Droid.Services.LocalNotificationsService))]
namespace StreamingMusic.Droid.Services
{
    public class LocalNotificationsService: ILocalNotificationsService
    {

        private const string CHANNEL_ID = "local_notifications_channel";
        private const string CHANNEL_NAME = "Now Playing";
        //private const string CHANNEL_DESCRIPTION = "Now Playing";

        private int notificationId = -1;
        private const string TITLE_KEY = "title";
        private const string MESSAGE_KEY = "message";

        private bool isChannelInitialized = false;
                
        private void CreateNotificationChannel()
        {
            if(Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default)
            {
                //Description = CHANNEL_DESCRIPTION
            };

            var notificationManager = (NotificationManager) AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        [System.Obsolete]
        public void ShowNotification(string title, string message, IDictionary<string, string> data)
        {
            if (!isChannelInitialized)
            {
                CreateNotificationChannel();
            }

            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TITLE_KEY, title);
            intent.PutExtra(MESSAGE_KEY, message);
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            notificationId++;

            // When the notification is clicked only open the app once
            // var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, notificationId, intent , PendingIntentFlags.OneShot);

            // Can always open the app when the notification is clicked
            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, notificationId, intent , PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(AndroidApp.Context, CHANNEL_ID)
                                            .SetSmallIcon(Resource.Drawable.notif_nowplaying)
                                            .SetContentTitle(title)
                                            .SetContentText(message)
                                            .SetContentIntent(pendingIntent)
                                            .SetNotificationSilent()
                                            .SetOngoing(true)
                                            .SetCategory(Notification.CategoryStatus)
                                            .SetPriority((int)NotificationPriority.Max)
                                            .SetVisibility((int)NotificationVisibility.Public);

            var notificationManager = NotificationManagerCompat.From(AndroidApp.Context);
            notificationManager.Notify(notificationId, notificationBuilder.Build());
        }

        public void HideNotification()
        {
            var notificationManager = NotificationManagerCompat.From(AndroidApp.Context);
            notificationManager.CancelAll();
        }
    }
}
