using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class MusicPlayer : MonoBehaviour, IObserverMusic
    {
        public List<Sound> clips;


        public AudioSource source;

        public void Play(string name)
        {
            source.Stop();
            int lenght = clips.Count;
            for (int i = 0; i < lenght; i ++)
            {
                if (clips[i].name == name)
                {
                    source.clip = clips[i].clip;
                    source.Play();
                    return;
                }
            }


        }

        public void Stop()
        {
            source.Stop();
        }

        public void Pause()
        {
            source.Pause();
        }
        public void UnPause()
        {
            source.UnPause();
        }

        #region observer

        public void UpdatePlay(string name)
        {
            source.Stop();
            int lenght = clips.Count;
            for(int i = 0; i < lenght; i++)
            {
                if (clips[i].name == name)
                {
                    source.clip = clips[i].clip;
                    source.Play();
                    return;
                }
            }
        }

        public void UpdateStop()
        {       
                Stop();            
        }

        public void UpdatePause()
        {        
                Pause();          
        }

        public void UpdateUnPause()
        {        
               UnPause();
        }
        #endregion 
    }

}