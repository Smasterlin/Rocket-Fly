using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public static AudioSourceManager instance;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource[] soundAudioSource;
    private void Awake()
    {
        instance = this;
    }
    public void PlayMusic(AudioClip audioClip,bool loop=true)
    {
        musicAudioSource.clip = audioClip;
        musicAudioSource.loop = loop;
        musicAudioSource.volume = GlobalValue.volumn;
        musicAudioSource.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (soundAudioSource[0].isPlaying)
        {
            soundAudioSource[1].PlayOneShot(audioClip,GlobalValue.volumn);
        }
        else
        {
            soundAudioSource[0].PlayOneShot(audioClip,GlobalValue.volumn);
        }
    }

    public void StopPlayMusic ()
    {
        musicAudioSource.Stop();
    }
}
