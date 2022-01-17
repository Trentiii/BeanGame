﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAi : MonoBehaviour
{
    //Lazyness intensify
    public GameObject intro;
    public GameObject warningMaker;
    public GameObject Cover;
    public GameObject Attack;
    public GameObject endCanvas;
    public Image endComic;

    [HideInInspector]public bool grappled = false;

    private Vector2 warningStartPos;

    Animator a;
    TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        tr = warningMaker.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        a.SetBool("Grappled", grappled);
    }

    public void start() 
    {
        StartCoroutine(attack());
        warningStartPos = warningMaker.transform.position;
    }

    private IEnumerator attack()
    {
        yield return new WaitForSeconds(0.5f);
        warningMaker.SetActive(true);
        yield return new WaitForSeconds(4);
        Instantiate(Attack, Attack.transform.position, Quaternion.identity).SetActive(true);
    }

    private IEnumerator fader()
    {
        //Wait
        yield return new WaitForSecondsRealtime(2);

        //Turn on end canvas
        endCanvas.SetActive(true);

        //While comic is faded out
        while (endComic.color.a < 1)
        {
            //Fade in
            endComic.color = new Color(endComic.color.r, endComic.color.g, endComic.color.b, endComic.color.a + 0.05f);
            yield return new WaitForSecondsRealtime(0.01f); //Wait
        }

        //Wait
        yield return new WaitForSecondsRealtime(5);

        GrossChanger.grossed = true;
        Time.timeScale = 1;

        Debug.Log("need to add bean transition here");
        //Load credit scene
        SceneManager.LoadScene(2);
    }

    public void end()
    {
        //Turn on cover
        Cover.SetActive(true);

        //Start fader
        StartCoroutine(fader());
    }


    public void fullReset()
    {
        //Reset intro
        intro.SetActive(true);

        //Reset varibles
        grappled = false;

        //Call delayedReset once time starts again
        Invoke("delayedReset", 0.1f);
    }

    private void delayedReset()
    {
        //Reset animator
        a.Play("Base Layer.Nothing", 0);

        //Reset warning
        warningMaker.transform.position = warningStartPos;
        tr.Clear();
        warningMaker.SetActive(false);
    }
}
