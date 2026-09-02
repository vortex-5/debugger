using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace AnyKey
{

    //Enumeration to store song names
    public enum song{
        SONG1 = 1,
        BOSS,
        VICTORY,
        MENU,
        FAILURE
    }

    //Enumeration to store sound clip names
    public enum soundClip
    {
        OUCH = 1,
        SQUISH,
        THUD,
        TTC
    }

    /// <summary>
    /// Class to handle music and sound effects
    /// </summary>
    public static class Sound
    {

        //Ojbects to handle sounds
        private static AudioEngine audioEngine;
        private static SoundBank soundBank;
        private static WaveBank waveBank;

        private static Cue musicCue;

        public static void Initialize(string rootDir)
        {
            //Set up the audio engine and prepare sounds to be played
            //audioEngine = new AudioEngine("Content\\Sound\\Win\\GameSound.xgs");
            //soundBank = new SoundBank(audioEngine, "Content\\Sound\\Win\\Sound Bank.xsb");
            //waveBank = new WaveBank(audioEngine, "Content\\Sound\\win\\Wave Bank.xwb");

            audioEngine = new AudioEngine("Content\\GameSound.xgs");
            soundBank = new SoundBank(audioEngine, "Content\\Sound Bank.xsb");
            waveBank = new WaveBank(audioEngine, "Content\\Wave Bank.xwb");
        }

        /// <summary>
        /// Start a song playing for background music
        /// </summary>
        /// <param name="songName">The name of the song to play from the enumerated type song</param>
        public static void PlaySong(song songName){
            if (musicCue == null || !musicCue.IsPlaying)
            {
                switch (songName)
                {
                    case song.SONG1:
                        musicCue = soundBank.GetCue("AntsMarch");
                        musicCue.Play();
                        break;
                    case song.VICTORY:
                        musicCue = soundBank.GetCue("Victory");
                        musicCue.Play();
                        break;
                    case song.FAILURE:
                        musicCue = soundBank.GetCue("Failure");
                        musicCue.Play();
                        break;
                    case song.BOSS:
                        musicCue = soundBank.GetCue("Boss");
                        musicCue.Play();
                        break;
                    case song.MENU:
                        musicCue = soundBank.GetCue("Menu");
                        musicCue.Play();
                        break;
                    default:
                        break;
                }
            }

        }

        /// <summary>
        /// Stop a song which is currently playing
        /// </summary>
        public static void StopSong()
        {
            if (musicCue != null)
            {
                musicCue.Stop(AudioStopOptions.Immediate);
            }
            
        }

        //Returns whether the music is playing or not
        public static bool isSongPlaying()
        {
            if (musicCue == null)
                return false;
            else
                return musicCue.IsPlaying;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Update()
        {
            audioEngine.Update();
        }

        public static void PlaySoundClip(soundClip clipName){
            switch(clipName){
                case soundClip.OUCH:
                    soundBank.PlayCue("Ouch");
                    break;
                case soundClip.SQUISH:
                    soundBank.PlayCue("Squish");
                    break;
                case soundClip.THUD:
                    soundBank.PlayCue("Thud");
                    break;
                case soundClip.TTC:
                    soundBank.PlayCue("ttc");
                    break;
                default:
                    break;
            }
        }
    }
}
