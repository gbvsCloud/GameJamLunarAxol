using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public List<AudioClip> audioClips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayRandomSound()
    {
        
        audioSource.volume = 1;
        audioSource.pitch = 1;
        audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.Play();
    }

    public void PlayRandomSoundWithVariation()
    {
        audioSource.volume = Random.Range(0.95f, 1.15f);
        audioSource.pitch = Random.Range(0.80f, 1.15f);
        audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.Play();
    }

}
