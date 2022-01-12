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
        soundPlayer.Stop(); //Stops other button sounds
        soundPlayer.clip = hoverSound; //Sets sound to click sound
        soundPlayer.Play(); //Plays hover sound
    }

    public void playClickSound()
    {
        soundPlayer.Stop(); //Stops other button sounds
        soundPlayer.clip = clickSound; //Sets sound to click sound
        soundPlayer.Play(); //Plays sound
    }
}
