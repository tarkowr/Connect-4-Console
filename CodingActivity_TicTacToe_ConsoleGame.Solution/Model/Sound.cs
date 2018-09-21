using System;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class Sound
    {
        private string _audioFile;
        private bool _playing;
        private System.Media.SoundPlayer _soundPlayer;

        public string AudioFile
        {
            get { return _audioFile; }
            set { _audioFile = value; }
        }
        public bool Playing
        {
            get { return _playing; }
            set { _playing = value; }
        }
        public System.Media.SoundPlayer SoundPlayer
        {
            get { return _soundPlayer; }
            set { _soundPlayer = value; }
        }

        /// <summary>
        /// Sound Constructor
        /// </summary>
        /// <param name="audioFile"></param>
        public Sound(string audioFile, System.Media.SoundPlayer soundPlayer)
        {
            _audioFile = audioFile;
            _soundPlayer = soundPlayer;

            InitializeSound();
        }

        /// <summary>
        /// Attach the audio file to the sound player and load it
        /// </summary>
        private void InitializeSound()
        {
            _soundPlayer.SoundLocation = _audioFile;
            _soundPlayer.LoadAsync();
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="audioPath"></param>
        public void playSound(bool loop = false)
        {
            try
            {
                _soundPlayer.Play();
                if (loop == true)
                {
                    _soundPlayer.PlayLooping();
                    _playing = true;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Stop a Sound and Dispose of it
        /// </summary>
        public void stopSound(bool dispose = false)
        {
            _soundPlayer.Stop();
            _playing = false;
            if (dispose == true) _soundPlayer.Dispose();
        }
    }
}
