using Xamarin.Forms;
using StreamingMusic.Interfaces;
using Android.Media;
using System;
using System.Collections.Generic;

[assembly: Dependency(typeof(StreamingMusic.Droid.Services.MediaPlayerService))]
namespace StreamingMusic.Droid.Services
{
    public class MediaPlayerService : IMediaPlayerService
    {
        public MediaPlayer player;
        private bool DataSourceEmpty = true;

        private void ShowNotification()
        {
            DependencyService.Get<ILocalNotificationsService>().ShowNotification("Relaxing Guitar Music", "Now Playing", new Dictionary<string, string>());
        }

        private void HideNotification()
        {
            DependencyService.Get<ILocalNotificationsService>().HideNotification();
        }

        public void InitMediaPlayer()
        {
            player = new MediaPlayer();
            player.Reset();
        }

        public bool SetDataSource(string path)
        {
            try
            {
                if (DataSourceEmpty)
                {
                    player.SetDataSource(path);
                    player.Prepare();
                }
                DataSourceEmpty = false;
                return true;
            }
            catch (Exception)
            {
                DataSourceEmpty = true;
                return false;
            }
        }

        public bool IsPlaying()
        {
            if (player.IsPlaying)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Start()
        {
            player.Looping = true;
            player.Start();
            ShowNotification();
        }

        public void Pause()
        {
            player.Pause();
            HideNotification();
        }

        public void Reset()
        {
            HideNotification();
            player.Release();
            InitMediaPlayer();
            DataSourceEmpty = true;
        }
    }
}