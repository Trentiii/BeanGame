using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroComic : MonoBehaviour
{
    public Faucet Faucet;

    public Image[] mainMenu;
    public  GameObject mainPanel;
    private Image comicFrame1;
    private Image comicFrame2;
    private Image comicFrame3;

    private bool running = false;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        comicFrame1 = GameObject.Find("Comic frame 1").GetComponent<Image>();
        comicFrame2 = GameObject.Find("Comic frame 2").GetComponent<Image>();
        comicFrame3 = GameObject.Find("Comic frame 3").GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (running && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)))
        {
            timer = 4;
        }
    }

    public void startComic()
    {
        StartCoroutine(comicRunner());
    }

    private IEnumerator comicRunner()
    {
        running = true;

        //Fade out of mainMenu
        //While menu is fading out
        while (mainMenu[0].color.a > 0)
        {
            //Fade out
            for (int i = 0; i < mainMenu.Length; i++)
            {
                mainMenu[i].color = new Color(mainMenu[i].color.r, mainMenu[i].color.g, mainMenu[i].color.b, mainMenu[i].color.a - 0.025f);
            }
            yield return new WaitForSecondsRealtime(0.01f); //Wait
        }

        mainPanel.SetActive(false);

        //-----------------Comic 1
        while (timer < 4)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;

        //While comic is fading out
        while (comicFrame1.color.a > 0)
        {
            //Fade out
            comicFrame1.color = new Color(comicFrame1.color.r, comicFrame1.color.g, comicFrame1.color.b, comicFrame1.color.a - 0.025f);
            yield return new WaitForSecondsRealtime(0.01f); //Wait
        }

        //-----------------Comic 2
        while (timer < 4)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;

        //While comic is fading out
        while (comicFrame2.color.a > 0)
        {
            //Fade out
            comicFrame2.color = new Color(comicFrame2.color.r, comicFrame2.color.g, comicFrame2.color.b, comicFrame2.color.a - 0.025f);
            yield return new WaitForSecondsRealtime(0.01f); //Wait
        }

        //-----------------Comic 3
        while (timer < 4)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;

        //While comic is fading out
        while (comicFrame3.color.a > 0)
        {
            //Fade out
            comicFrame3.color = new Color(comicFrame3.color.r, comicFrame3.color.g, comicFrame3.color.b, comicFrame3.color.a - 0.025f);
            yield return new WaitForSecondsRealtime(0.01f); //Wait
        }

        //-----------------Comic 4
        while (timer < 4)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Faucet.transitionToIndex(1);
    }
}
