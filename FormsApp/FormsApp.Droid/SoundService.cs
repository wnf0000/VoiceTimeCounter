
using Android.App;
using FormsApp.Droid;
using FormsApp.Service;

[assembly: Xamarin.Forms.Dependency(typeof(SoundService))]
namespace FormsApp.Droid
{
    class SoundService : ISoundService
    {
        private AudioPlayer _player;

        public SoundService()
        {
            _player = new AudioPlayer(Application.Context);
        }
        public void PlayDi()
        {
            _player.Play(Resource.Raw.di);
        }

        public void PlayDong()
        {
            _player.Play(Resource.Raw.dong);
        }

        public void Dispose()
        {
            if (_player != null)
            {
                _player.StopPlayer();
                _player = null;
            }
           
        }
    }
}