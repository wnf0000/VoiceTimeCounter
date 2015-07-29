using System;
using Android.App;
using Android.Content;
using Android.Media;
using Java.IO;

namespace FormsApp.Droid
{
    public class AudioPlayer
    {
        private Context context;
        private MediaPlayer player;

        public AudioPlayer(Context context = null)
        {
            this.context = context;
        }

        public void Play(string filePath)
        {
            try
            {
                if (player == null)
                {
                    player = new MediaPlayer();
                }
                else
                {
                    player.Reset();
                }

                // This method works better than setting the file path in SetDataSource. Don't know why.
                var file = new File(filePath);
                var fis = new FileInputStream(file);

                player.SetDataSource(fis.FD);

                //player.SetDataSource(filePath);
                player.Prepare();
                player.Start();
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.StackTrace);
            }
        }

        public void Play(int resId)
        {
            try
            {
                if (player != null)
                {
                    //if (player.IsPlaying)
                    //{
                    //    player.Stop();
                    //    player.Reset();
                    //}
                    player.Reset();
                }
                player = MediaPlayer.Create(Application.Context, resid: resId);
                player.Stop();
                player.Prepare();
                player.Start();
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.StackTrace);
            }
        }

        public void StopPlayer()
        {
            if ((player != null))
            {
                if (player.IsPlaying)
                {
                    player.Stop();
                }
                player.Release();
                player = null;
            }
        }

        public void Stop()
        {
            StopPlayer();
        }
    }
}