using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSetter : MonoBehaviour
{
    public AudioMixer musicMixer;
    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        musicMixer.SetFloat("MasterVolume", masterVolume);
        musicMixer.SetFloat("MusicVolume", musicVolume);
        musicMixer.SetFloat("SfxVolume", sfxVolume);
    }


    public void setVolume(float newVolume)
    {
        if (newVolume >= 0)
        {
            masterVolume = newVolume;
        }
        else
        { 
            masterVolume = newVolume * 4;
        }
    }

    public void setSfxVolume(float newVolume)
    {
        if (newVolume >= 0)
        {
            sfxVolume = newVolume;
        }
        else
        {
            sfxVolume = newVolume * 4;
        }
    }

    public void setMusicVolume(float newVolume)
    {
        if (newVolume >= 0)
        {
            musicVolume = newVolume;
        }
        else
        {
            musicVolume = newVolume * 4;
        }
    }
}
