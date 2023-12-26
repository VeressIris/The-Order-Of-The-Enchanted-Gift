using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] songs;
    
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = songs[Random.Range(0, songs.Length)];
            audioSource.Play();
        }
    }
}
