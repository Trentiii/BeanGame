using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CreditMusicGross : MonoBehaviour
{
    public static bool cGrossed;
    public AudioMixer musicMixer;

    bool up = false;
    float range = 0.8f;
    float currentPitch = 1;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cGrossed);

        if (cGrossed)
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
