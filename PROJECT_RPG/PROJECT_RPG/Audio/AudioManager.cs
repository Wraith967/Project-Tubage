using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace PROJECT_RPG
{
    /// <summary>
    /// Manages playback of sounds and music.
    /// </summary>
    /// 
    // Inspired from the XNA wiki.
    // ... and then totally torn apart and rebuilt because it totally didn't work as intended.
    // :(
    //
    public class AudioManager : GameComponent
    {

        private static AudioManager audioManager;

        public static AudioManager Instance
        { get { return audioManager; } }

        public static void Initialize(Game game)
        {
            audioManager = new AudioManager(game);
            game.Components.Add(audioManager);
        }

        #region Private fields
        private ContentManager _content;

        private Dictionary<string, Song> _songs = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();

        private Song _currentSong = null;

        private SoundEffectInstance[] _playingSounds = new SoundEffectInstance[MaxSounds];

        private bool _isMusicPaused = false;

        private bool _isFading = false;
        private MusicFadeEffect _fadeEffect;
        #endregion

        // Change MaxSounds to set the maximum number of simultaneous sounds that can be playing.
        private const int MaxSounds = 100;

        /// <summary>
        /// Gets the name of the currently playing song, or null if no song is playing.
        /// </summary>
        public string CurrentSong { get; private set; }

        /// <summary>
        /// Gets or sets the volume to play songs. 1.0f is max volume.
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        /// <summary>
        /// Gets or sets the master volume for all sounds. 1.0f is max volume.
        /// </summary>
        public float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }

        /// <summary>
        /// Gets whether a song is playing or paused (i.e. not stopped).
        /// </summary>
        public bool IsSongActive { get { return _currentSong != null && MediaPlayer.State != MediaState.Stopped; } }

        /// <summary>
        /// Gets whether the current song is paused.
        /// </summary>
        public bool IsSongPaused { get { return _currentSong != null && _isMusicPaused; } }

        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        private AudioManager(Game game)
            : base(game)
        {
            MusicVolume = 0.05f;
            SoundVolume = 0.05f;
            _content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);
        }

        /// <summary>
        /// Creates a new Audio Manager. Add this to the Components collection of your Game.
        /// </summary>
        /// <param name="game">The Game</param>
        /// <param name="contentFolder">Root folder to load audio content from</param>
        private AudioManager(Game game, string contentFolder)
            : base(game)
        {
            audioManager._content = new ContentManager(game.Content.ServiceProvider, contentFolder);
        }

        /// <summary>
        /// Loads a Song into the AudioManager.
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        public static void LoadSong(string songName)
        {
            LoadSong(songName, songName);
        }

        /// <summary>
        /// Loads a Song into the AudioManager.
        /// </summary>
        /// <param name="songName">Name of the song to load</param>
        /// <param name="songPath">Path to the song asset file</param>
        public static void LoadSong(string songName, string songPath)
        {
            if (audioManager._songs.ContainsKey(songName))
            {
                throw new InvalidOperationException(string.Format("Song '{0}' has already been loaded", songName));
            }
            else 
                audioManager._songs.Add(songName, audioManager._content.Load<Song>(songPath));
        }

        public static bool IsSongLoaded(string songName)
        {
            if (audioManager._songs.ContainsKey(songName))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Loads a SoundEffect into the AudioManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        public static void LoadSound(string soundName)
        {
            LoadSound(soundName, soundName);
        }

        /// <summary>
        /// Loads a SoundEffect into the AudioManager.
        /// </summary>
        /// <param name="soundName">Name of the sound to load</param>
        /// <param name="soundPath">Path to the song asset file</param>
        public static void LoadSound(string soundName, string soundPath)
        {
            if (audioManager._sounds.ContainsKey(soundName))
            {
                throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
            }

            audioManager._sounds.Add(soundName, audioManager._content.Load<SoundEffect>(soundPath));
        }

        public static bool IsSoundLoaded(string soundName)
        {
            if (audioManager._sounds.ContainsKey(soundName))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Unloads all loaded songs and sounds.
        /// </summary>
        public static void UnloadContent()
        {
            audioManager._content.Unload();
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public static void PlaySong(string songName)
        {
            PlaySong(songName, false);
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public static void PlaySong(string songName, bool loop)
        {
            if (audioManager.CurrentSong != songName)
            {
                if (audioManager._currentSong != null)
                {
                    MediaPlayer.Stop();
                }

                if (!audioManager._songs.TryGetValue(songName, out audioManager._currentSong))
                {
                    throw new ArgumentException(string.Format("Song '{0}' not found", songName));
                }

                audioManager.CurrentSong = songName;

                audioManager._isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(audioManager._currentSong);

                if (!audioManager.Enabled)
                {
                    MediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// Pauses the currently playing song. This is a no-op if the song is already paused,
        /// or if no song is currently playing.
        /// </summary>
        public static void PauseSong()
        {
            if (audioManager._currentSong != null && !audioManager._isMusicPaused)
            {
                if (audioManager.Enabled) MediaPlayer.Pause();
                audioManager._isMusicPaused = true;
            }
        }

        /// <summary>
        /// Resumes the currently paused song. This is a no-op if the song is not paused,
        /// or if no song is currently playing.
        /// </summary>
        public static void ResumeSong()
        {
            if (audioManager._currentSong != null && audioManager._isMusicPaused)
            {
                if (audioManager.Enabled) MediaPlayer.Resume();
                audioManager._isMusicPaused = false;
            }
        }

        /// <summary>
        /// Stops the currently playing song. This is a no-op if no song is currently playing.
        /// </summary>
        public static void StopSong()
        {
            if (audioManager._currentSong != null && MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                audioManager._isMusicPaused = false;
            }
        }

        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public static void FadeSong(float targetVolume, TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration must be a positive value");
            }

            audioManager._fadeEffect = new MusicFadeEffect(MediaPlayer.Volume, targetVolume, duration);
            audioManager._isFading = true;
        }

        /// <summary>
        /// Stop the current fade.
        /// </summary>
        /// <param name="option">Options for setting the music volume</param>
        public static void CancelFade(FadeCancelOptions option)
        {
            if (audioManager._isFading)
            {
                switch (option)
                {
                    case FadeCancelOptions.Source: MediaPlayer.Volume = audioManager._fadeEffect.SourceVolume; break;
                    case FadeCancelOptions.Target: MediaPlayer.Volume = audioManager._fadeEffect.TargetVolume; break;
                }

                audioManager._isFading = false;
            }
        }

        /// <summary>
        /// Plays the sound of the given name.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        public static void PlaySound(string soundName)
        {
            PlaySound(soundName, 1.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Plays the sound of the given name at the given volume.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        public static void PlaySound(string soundName, float volume)
        {
            PlaySound(soundName, volume, 0.0f, 0.0f);
        }

        /// <summary>
        /// Plays the sound of the given name with the given parameters.
        /// </summary>
        /// <param name="soundName">Name of the sound</param>
        /// <param name="volume">Volume, 0.0f to 1.0f</param>
        /// <param name="pitch">Pitch, -1.0f (down one octave) to 1.0f (up one octave)</param>
        /// <param name="pan">Pan, -1.0f (full left) to 1.0f (full right)</param>
        public static void PlaySound(string soundName, float volume, float pitch, float pan)
        {
            SoundEffect sound;

            if (!audioManager._sounds.TryGetValue(soundName, out sound))
            {
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));
            }

            int index = GetAvailableSoundIndex();

            if (index != -1)
            {
                audioManager._playingSounds[index] = sound.CreateInstance();
                audioManager._playingSounds[index].Volume = volume;
                audioManager._playingSounds[index].Pitch = pitch;
                audioManager._playingSounds[index].Pan = pan;
                audioManager._playingSounds[index].Play();

                if (!audioManager.Enabled)
                {
                    audioManager._playingSounds[index].Pause();
                }
            }
        }

        /// <summary>
        /// Stops all currently playing sounds.
        /// </summary>
        public static void StopAllSounds()
        {
            for (int i = 0; i < audioManager._playingSounds.Length; ++i)
            {
                if (audioManager._playingSounds[i] != null)
                {
                    audioManager._playingSounds[i].Stop();
                    audioManager._playingSounds[i].Dispose();
                    audioManager._playingSounds[i] = null;
                }
            }
        }

        /// <summary>
        /// Called per loop unless Enabled is set to false.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < audioManager._playingSounds.Length; ++i)
            {
                if (audioManager._playingSounds[i] != null && audioManager._playingSounds[i].State == SoundState.Stopped)
                {
                    audioManager._playingSounds[i].Dispose();
                    audioManager._playingSounds[i] = null;
                }
            }

            if (audioManager._currentSong != null && MediaPlayer.State == MediaState.Stopped)
            {
                audioManager._currentSong = null;
                audioManager.CurrentSong = null;
                audioManager._isMusicPaused = false;
            }

            if (audioManager._isFading && !audioManager._isMusicPaused)
            {
                if (audioManager._currentSong != null && MediaPlayer.State == MediaState.Playing)
                {
                    if (audioManager._fadeEffect.Update(gameTime.ElapsedGameTime))
                    {
                        audioManager._isFading = false;
                    }

                    MediaPlayer.Volume = audioManager._fadeEffect.GetVolume();
                }
                else
                {
                    audioManager._isFading = false;
                }
            }

            base.Update(gameTime);
        }

        // Pauses all music and sound if disabled, resumes if enabled.
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Paused)
                    {
                        _playingSounds[i].Resume();
                    }
                }

                if (!_isMusicPaused)
                {
                    MediaPlayer.Resume();
                }
            }
            else
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Playing)
                    {
                        _playingSounds[i].Pause();
                    }
                }

                MediaPlayer.Pause();
            }

            base.OnEnabledChanged(sender, args);
        }

        // Acquires an open sound slot.
        private static int GetAvailableSoundIndex()
        {
            for (int i = 0; i < audioManager._playingSounds.Length; ++i)
            {
                if (audioManager._playingSounds[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        #region MusicFadeEffect
        private struct MusicFadeEffect
        {
            public float SourceVolume;
            public float TargetVolume;

            private TimeSpan _time;
            private TimeSpan _duration;

            public MusicFadeEffect(float sourceVolume, float targetVolume, TimeSpan duration)
            {
                SourceVolume = sourceVolume;
                TargetVolume = targetVolume;
                _time = TimeSpan.Zero;
                _duration = duration;
            }

            public bool Update(TimeSpan time)
            {
                _time += time;

                if (_time >= _duration)
                {
                    _time = _duration;
                    return true;
                }

                return false;
            }

            public float GetVolume()
            {
                return MathHelper.Lerp(SourceVolume, TargetVolume, (float)_time.Ticks / _duration.Ticks);
            }
        }
        #endregion
    }

    /// <summary>
    /// Options for AudioManager.CancelFade
    /// </summary>
    public enum FadeCancelOptions
    {
        /// <summary>
        /// Return to pre-fade volume
        /// </summary>
        Source,
        /// <summary>
        /// Snap to fade target volume
        /// </summary>
        Target,
        /// <summary>
        /// Keep current volume
        /// </summary>
        Current
    }
}
