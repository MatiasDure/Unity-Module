using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class <c>AudioManager</c> facilitates the use of sound 
    /// in the scene
    /// </summary>
    /// <remarks>
    /// Only one is needed per scene
    /// </remarks>
    public static class AudioManager 
    {

        private static Dictionary<Sound, float> soundTimer;

        public enum Sound
        {
            NoSound,
            PlayerEnter,
            PlayerStartCar,
            PlayerTurn,
            PlayerStopped,
            PlayerMoving,
            PlayerHit,
            MoneyPicked,
            KeyPicked,
            DoorOpened,
            FlagGrabbed,
            GameSoundtrack,
            EnterNpc,
            LeaveNpc,
            PlayerFalling,
            NewNpcMessage
        }

        /// <summary>
        /// This method initializes the dictionary which contains the sound that 
        /// need a timer to function correctly
        /// </summary>
        public static void Init()
        {
            soundTimer = new Dictionary<Sound, float>();
            soundTimer[Sound.PlayerTurn] = 
                soundTimer[Sound.PlayerStopped] = 
                soundTimer[Sound.PlayerMoving] = 
                soundTimer[Sound.PlayerFalling] = 
                soundTimer[Sound.FlagGrabbed] = 0;
        }

        /// <summary>
        /// This method plays the sound that is passed in
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        public static void PlaySound(Sound sound)
        {
            if (sound == Sound.NoSound) return;
            int index = GetAudioIndex(sound);
            if (!CanPlaySound(sound, index)) return;
            AudioSource audioSource = GameAssets.Assets.SoundAudioClips[index].source == null ? 
                                        CreateAudioSource(index, sound) : 
                                        GameAssets.Assets.SoundAudioClips[index].source;

            if (sound == Sound.GameSoundtrack)
            {
                audioSource.volume = 0.1f;
                audioSource.loop = true;
            }
            else audioSource.volume = 0.3f;

            audioSource.Play();
        }

        /// <summary>
        /// This method stops the sound that is passed in
        /// </summary>
        /// <param name="sound">The sound to stop.</param>
        public static void StopSound(Sound sound)
        {
            int index = GetAudioIndex(sound);
            if (GameAssets.Assets.SoundAudioClips[index].source == null) CreateAudioSource(index, sound);
            GameAssets.Assets.SoundAudioClips[index].source.Stop();
        }

        /// <summary>
        /// This method checks if the sounds that are in the timer list 
        /// are able to play
        /// </summary>
        /// <param name="sound">The sound to check.</param>
        /// <param name="index">The index of the sound in the GameAssets SoundAudioClips list.</param>
        private static bool CanPlaySound(Sound sound, int index = -1)
        {

           //If index is passed, then we avoid looping through all the sounds 
           index = index < 0 ? GetAudioIndex(sound) : index;

           //Returns true if there is no audio source
           if(GameAssets.Assets.SoundAudioClips[index].source == null) return true;

           //returs true for sounds that dont have a timer, checks for the ones that do
           switch (sound)
           {
                default:
                    return true;
                case Sound.PlayerTurn:
                    if(soundTimer.ContainsKey(sound)) return SoundTimerStatus(sound, .7f);
                    break;
                case Sound.PlayerStopped:
                    if (soundTimer.ContainsKey(sound)) return SoundTimerStatus(sound, .5f);
                    break;
                case Sound.PlayerMoving:
                    //If the sound is NOT playing we ignore the timer (this might be possible because of the toggle between stopped and moving sounds)
                    if (!GameAssets.Assets.SoundAudioClips[index].source.isPlaying) return true;
                    if (soundTimer.ContainsKey(sound)) return SoundTimerStatus(sound, 5f);
                    break;
                case Sound.PlayerFalling:
                    if (soundTimer.ContainsKey(sound)) return SoundTimerStatus(sound, 1f);
                    break;
                case Sound.FlagGrabbed:
                    if (soundTimer.ContainsKey(sound)) return SoundTimerStatus(sound, 5f);
                    break;
            }
            return true;
        }

        /// <summary>
        /// This method returns the index where the sound is located in the 
        /// GameAssets' SoundAudioClips' list
        /// </summary>
        /// <param name="sound">The sound to look for.</param>
        public static int GetAudioIndex(Sound sound)
        {
            for (int i = 0; i < GameAssets.Assets.SoundAudioClips.Length; i++)
            {
                if (GameAssets.Assets.SoundAudioClips[i].sound == sound) return i; 
            }
            return -1;
        }

        /// <summary>
        /// This method updates the last time played if necessary
        /// and returns a boolean to inform if its possible to play the sound
        /// of sounds that are included in the soundtimer list
        /// </summary>
        /// <param name="sound">The sound to look for.</param>
        /// <param name="playDelay">The cooldown for the timer</param>
        static bool SoundTimerStatus(Sound sound, float playDelay)
        {

            //get the last time played
            float lastTimePlayed = soundTimer[sound];

            //Checks whether is not the first time playing
            if (lastTimePlayed != 0)
            {
                if (lastTimePlayed + playDelay <= Time.time)
                {
                    soundTimer[sound] = Time.time;
                    return true;
                }
                return false;
            }
            soundTimer[sound] = Time.time;  
            return true;
        }

        /// <summary>
        /// This method creates a new object, along
        /// with a new audio source
        /// </summary>
        /// <param name="sound">The sound to create the audio source for</param>
        /// <param name="index">The index of th sound in the SoundAudioClips' list</param>
        static AudioSource CreateAudioSource(int index, Sound sound)
        {
            SoundAudioClip soundAudio = GameAssets.Assets.SoundAudioClips[index];
            soundAudio.soundObject = new GameObject("Sound" + sound);
            soundAudio.source = GameAssets.Assets.SoundAudioClips[index].soundObject.AddComponent<AudioSource>();
            soundAudio.source.volume = 0.5f;
            soundAudio.source.clip = GameAssets.Assets.SoundAudioClips[index].clip;
            return soundAudio.source;

        }


        /// <summary>
        /// This method creates checks if a sound is playing
        /// </summary>
        /// <param name="sound">The sound to check for</param>
        public static bool IsPlaying(Sound sound)
        {
            int index = GetAudioIndex(sound);
            if (GameAssets.Assets.SoundAudioClips[index].source == null) CreateAudioSource(index,sound);
            return GameAssets.Assets.SoundAudioClips[index].source.isPlaying;
        }
    }
}
