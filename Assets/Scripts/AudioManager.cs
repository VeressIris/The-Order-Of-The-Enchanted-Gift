using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string volumeToChange;

    void Start()
    {
        mixer.SetFloat("Volume", PlayerPrefs.GetFloat(volumeToChange));
    }

    public void SetVolume(float sliderVal)
    {
        mixer.SetFloat("Volume", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat(volumeToChange, sliderVal);
    }
}
