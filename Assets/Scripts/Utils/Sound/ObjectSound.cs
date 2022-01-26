using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    public AudioSource audioSource;
    public float initVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        initVolume = audioSource.volume;
    }

    public void PlaySound(int index)
    {
        audioSource.Stop();
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public bool IsPlaySound()
    {
        return audioSource.isPlaying;
    }
}
