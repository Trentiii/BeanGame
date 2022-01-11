using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject SceneTransitioner;
    public Canvas MainCanvas;
    public GameObject PausePanel;
    private Animator ani;
    public GameObject GV;
    public bool Paused = false;
    // Start is called before the first frame update
    void Start()
    {
        MainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        ani = PausePanel.GetComponent<Animator>();
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
        }
        if (!Paused)
        {
            //play fading away animation
            ani.SetBool("Paused", false);
            PausePanel.SetActive(false);
            GV.SetActive(false);
            Time.timeScale = 1f;
        }
        if (Paused)
        {
            //play sliding in animation
            ani.SetBool("Paused", true);
            PausePanel.SetActive(true);
            GV.SetActive(true);
            Time.timeScale = 0f;
        }
    }
        
    public void creditsClicked()// if menu or credits clicked
    {
        MainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        //call transition to credit scene
        SceneTransitioner.GetComponent<SceneTransitionScript>().creditScene();
    }

    public void MenuClicked()// if menu or credits clicked
    {
        MainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        //call transition to main menu
        SceneTransitioner.GetComponent<SceneTransitionScript>().mainMenuScene();
    }

    public void xButtonClicked() // might not be working
    {
        Paused = false;
    }
}
