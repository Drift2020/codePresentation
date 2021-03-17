using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Music
{
    public class SoundPlayer : MonoBehaviour, IObserverSound
    {

        public List<Sound> clips;

        public string Name;
        public List<AudioSource> source;

        void Start()
        {
            pause = false;

        }


        bool pause;
        public void PlayOneShoot(string name, int number)
        {
            if (number < source.Count && !source[number].isPlaying && !pause)
            {
                int lenght = clips.Count;
                for(int i = 0; i < lenght; i++)
                {
                    if (clips[i].name == name)
                    {
                        source[number].clip = clips[i].clip;
                        source[number].PlayOneShot(clips[i].clip);
                        return;
                    }
                }
            }
            else
            {
                Debug.Log(string.Format("Number:{0} is empty", number));
            }


        }


        public bool isPlay(int number)
        {
            return source[number].isPlaying;
        }
        public void Play(string name,int number)
        {
            if (!source[number].isPlaying && !pause)
            {
                int lenght = clips.Count;
                for (int i = 0; i < lenght;i++)
                {
                    if (clips[i].name == name)
                    {
                        Name = name;
                        source[number].clip = clips[i].clip;
                        source[number].Play();
                        return;
                    }
                }
            }

        }

        public void PlayPrio(string name, int number)
        {
            if(source[number].isPlaying)
            {
                source[number].Stop();
            }
            if ( !pause)
            {
                int lenght = clips.Count;
                for (int i = 0; i < lenght; i++)
                {
                    if (clips[i].name == name)
                    {
                        Name = name;
                        source[number].clip = clips[i].clip;
                        source[number].Play();
                        return;
                    }
                }
            }


        }



        public void Stop(int i)
        {
            source[i].Stop();
        }

        public void Pause(int i)
        {
            pause = true;
            source[i].Pause();
        }
        public void UnPause(int i)
        {
            pause = false;
            source[i].UnPause();
        }

        #region observer

        public void UpdatePlay(string name, int number)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateStop(int number)
        {
            if (number == -1)
            {
                int lenght = source.Count;
                for (int i = 0; i < lenght; i++)
                {
                    Stop(i);
                }
            }
            else
            {
                Stop(number);
            }
        }

        public void UpdatePause(int number)
        {
            if (number == -1)
            {
                int lenght = source.Count;
                for (int i =0;i< lenght; i++)
                {
                    Pause(i);
                }
            }
            else
            {
                Pause(number);
            }
        }

        public void UpdateUnPause(int number)
        {
            if (number == -1)
            {
                int lenght = source.Count;
                for (int i = 0; i < lenght; i++)
                {
                    UnPause(i);
                }
            }
            else
            {
                UnPause(number);
            }
           
        }
        #endregion 
    }
}
