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
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (Paused)
        {
            //play fading away animation
            PausePanel.SetActive(false);
            GV.SetActive(false);
        }
        else
        {
            //play sliding in animation
            PausePanel.SetActive(true);
            GV.SetActive(true);
        }

        Paused = !Paused;
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
        PauseGame();
    }
}
