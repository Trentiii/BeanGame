using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;

public class GrowActivation : MonoBehaviour
{
    PlayerAnimation pa;
    AudioSource aS;
    BoxCollider2D bc2d;

    public AudioMixer musicMixer;

    private SpriteRenderer sr;
    private Volume v1;
    private ColorAdjustments ca;
    private FilmGrain fg;

    bool up = false;
    float range = 1;
    float currentPitch = 1;

    bool grown = false;
    float currentReponse = 0;

    // Start is called before the first frame update
    void Start()
    {
        pa = GameObject.Find("Player").GetComponent<PlayerAnimation>();
        aS = GetComponent<AudioSource>();
        bc2d = GetComponent<BoxCollider2D>();

        sr = GameObject.Find("FakeVignette").GetComponent<SpriteRenderer>();
        v1 = GameObject.Find("ScaryVolume").GetComponent<Volume>();
        v1.profile.TryGet(out ca);
        v1.profile.TryGet(out fg);
    }

    private void Update()
    {
        if (grown)
        {
            if (currentReponse < 1)
            {
                currentReponse += 0.05f;
            }
            else
            {
                currentReponse = 0;
            }

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

            fg.response.Override(currentReponse);
            musicMixer.SetFloat("Pitch", currentPitch);
        }
    }

    [ContextMenu("AddChorus")]
    public void addChorus()
    {
        musicMixer.SetFloat("Depth", 1);
    }

    private IEnumerator volumeFadeIn()
    {
        float intensity = 0;
        float colorValue = 1;
        float alphaValue = 0;

        while (intensity < 0.759f || colorValue > 0.6650944f || alphaValue < 0.6862745f || range > 0.80f)
        {
            if (intensity < 0.759f) intensity += Time.deltaTime / 30;
            if (colorValue > 0.6650944f) colorValue -= Time.deltaTime /30;
            if (alphaValue < 0.6862745f) alphaValue += Time.deltaTime / 30;
            if (range > 0.80f) range -= Time.deltaTime / 50;

            fg.intensity.Override(intensity);
            ca.colorFilter.Override(new Color(1, colorValue, colorValue));
            sr.color = new Color(1, 1, 1, alphaValue);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit player
        if (collision.tag == "Player")
        {
            grown = true;

            //Start grow
            pa.grow();

            aS.Play();

            bc2d.enabled = false;

            StartCoroutine(volumeFadeIn());

            //Destroy self
            //Destroy(gameObject, 2.25f);
        }
    }
}
