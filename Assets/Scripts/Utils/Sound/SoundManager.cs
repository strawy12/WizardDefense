using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource bgmAudio;
    private List<AudioSource> soundEffectAudios;

    private void Awake()
    {
        SoundManager[] smanagers = FindObjectsOfType<SoundManager>();
        if (smanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        InitAudioSource();
    }

    public void InitAudioSource()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        soundEffectAudios = new List<AudioSource>();

        foreach (AudioSource audio in audioSources)
        {
            if (audio.transform.CompareTag("BGM"))
            {
                bgmAudio = audio;
            }

            else
            {
                soundEffectAudios.Add(audio);
            }
        }
    }

    public void BGMVolume(float value)
    {
        if (bgmAudio == null) return;
        bgmAudio.volume = value;

        DataManager.Instance.PlayerData.bgmSoundVolume = value;
    }

    public void EffectVolume(float value)
    {
        foreach (AudioSource audio in soundEffectAudios)
        {
            audio.volume = value;
        }

        DataManager.Instance.PlayerData.effectSoundVolume = value;
    }

    public void SetBGM(AudioClip clip)
    {
        bgmAudio.Stop();
        bgmAudio.clip = clip;
        bgmAudio.Play();
    }

    public void StopBGM()
    {
        bgmAudio.Stop();
    }
}