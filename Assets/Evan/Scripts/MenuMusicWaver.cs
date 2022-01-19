using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuMusicWaver : MonoBehaviour
{
    public static bool mGrossed;
    public AudioMixer musicMixer;

    bool up = false;
    float range = 0.8f;
    float currentPitch = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mGrossed)
        {
            if (up && currentPitch < 1)
            {
                currentPitch += Time.unscaledDeltaTime / 15;
            }
            else if (up && currentPitch >= 1)
            {
                up = false;
            }
            else if (!up && currentPitch > range)
            {
                currentPitch -= Time.unscaledDeltaTime / 15;
            }
            else if (!up && currentPitch <= range)
            {
                up = true;
            }

            musicMixer.SetFloat("Pitch", currentPitch);
        }
    }
}
