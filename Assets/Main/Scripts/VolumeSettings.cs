using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    List<string> names = new() { "Master","Music","SFX"};
    [SerializeField] List<Slider> sliders = new();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < names.Count; i++)
        {
            sliders[i].value = GetValue(i);
        }
    }

    public void UpdateMaster(float value)
    {
        ChangeValue(0, sliders[0].value);
    }
    public void UpdateMusic(float value)
    {
        ChangeValue(1, sliders[1].value);
    }

    public void UpdateSFX(float value)
    {
        ChangeValue(2, sliders[2].value);
    }

    void ChangeValue(int index, float value)
    {
        audioMixer.GetFloat(names[0], out float value2);

        audioMixer.SetFloat(names[index], Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(names[index], value);
    }

    float GetValue(int index)
    {
        return PlayerPrefs.GetFloat(names[index], 0.75f);
    }
}
