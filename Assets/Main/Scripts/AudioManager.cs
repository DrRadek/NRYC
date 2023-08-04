using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource purrSound;
    [SerializeField] AudioSource waveSound;

    public static AudioManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
            

        instance = this;
    }


    public void PlayPurr()
    {
        if(waveSound.isPlaying)
            waveSound.Pause();

        if(!purrSound.isPlaying)
            purrSound.Play();
    }

    public void StopPurr()
    {
        if (!waveSound.isPlaying)
            waveSound.UnPause();

        if (purrSound.isPlaying)
            purrSound.Stop();
    }

    public void PlayWave()
    {
        if (!waveSound.isPlaying)
            waveSound.Play();
    }

    public void StopWave()
    {
        if (waveSound.isPlaying)
            waveSound.Stop();
    }
}
