using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private  AudioSource soundPlayer; //Holds audioSorce
    public AudioClip hoverSound; //Holds the sound for hovering over the button
    public AudioClip clickSound; //Holds the sound for clicking the button

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        soundPlayer.ignoreListenerPause = true;
    }

    public void playHoverSound()
    {
        //soundPlayer.Stop(); //Stops other button sounds
        soundPlayer.pitch = 1.2f; //Sets pitch
        soundPlayer.volume = 0.5f; //Sets volume
        soundPlayer.clip = hoverSound; //Sets sound to click sound
        soundPlayer.Play(); //Plays hover sound
    }

    public void playClickSound()
    {
        //soundPlayer.Stop(); //Stops other button sounds
        soundPlayer.pitch = 1.1f; //Sets pitch
        soundPlayer.volume = 0.25f; //Sets volume
        soundPlayer.clip = clickSound; //Sets sound to click sound
        soundPlayer.Play(); //Plays sound
    }
}
