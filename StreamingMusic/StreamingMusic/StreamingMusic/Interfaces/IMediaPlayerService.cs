using Android.Media;
using System;
using System.Collections.Generic;

namespace StreamingMusic.Interfaces
{
    public interface IMediaPlayerService
    {
        void InitMediaPlayer();
        bool SetDataSource(string path);
        bool IsPlaying();
        void Start();
        void Pause();
        void Reset();
        MediaPlayer GetPlayer();
    }
}
