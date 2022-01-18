using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusControllerScript : MonoBehaviour
{
    #region Variables

    #region PauseMenuVariables
    //reference to the Pause Menu
    public GameObject PauseMenu;
    //What state the Pause Panel is in
    public bool Paused = false;
    public GameObject SceneTransitioner;
    public Canvas MainCanvas;
    public Animator ani;
    public GameObject GV;
    private GrapplingGun gg;
    #endregion

    #region DevMenuVariables
    //reference to Dev Panel
    public GameObject DevPanel;
    //What state the Dev Panel is in
    private bool DevPanelOpen = false;
    //Unknown at the moment
    private bool DevMode = false;
    //If you can use the dev menu (controlled by a button in the dev menu)
    [SerializeField][Tooltip("If the Dev Menu can be used")]
    private bool DevMenuAvailable = true;
    #endregion

    private float cooldown = 0;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //MainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        PauseMenu.SetActive(false);
        DevPanel.SetActive(false);

        //Needed to fix sound bug
        gg = GameObject.Find("GrapplingGun").GetComponent<GrapplingGun>();
    }

    // Update is called once per frame
    void Update()
    {
        //Calls function for controlling dev menu
        if (Input.GetKeyDown(KeyCode.BackQuote) && DevMenuAvailable)
            OpenCloseDevMenu();

        //Calls function for controlling pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && cooldown < Time.time)
        {
            OpenClosePauseMenu();
            cooldown = Time.time + 0.5f;
            //remember that time stops when pause menu opens
        }
        else
        {
            //I noticed the cooldown wasnted working, hope you dont mind - Evan :D
            cooldown -= Time.unscaledDeltaTime;
        }

    }

    #region PauseMenu
    /// <summary>
    /// Controls the opening and closing of the pause menu
    /// </summary>
    private void OpenClosePauseMenu()
    {
        switch (Paused)
        {
            case true:
                gg.enabled = true; //Turns grappling gun back on                
                StartCoroutine(PauseOut());
                Paused = false;
                break;
            case false:
                GV.SetActive(true);
                gg.enabled = false; //Turns off grappling gun to stop click sounds
                MainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay; //Starts as ScreenSpaceCamera for transition in
                PauseMenu.SetActive(true);
                Time.timeScale = 0f;
                Paused = true;
                break;
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
        OpenClosePauseMenu();

    }

    IEnumerator PauseOut()
    {
        ani.SetTrigger("Paused");
        yield return new WaitForSecondsRealtime(1f);
        PauseMenu.SetActive(false);
        GV.SetActive(false);
        Time.timeScale = 1f;
    }



    #endregion

    #region DevMenu

    /// <summary>
    /// Controls the opening and closing of the dev menu
    /// </summary>
    private void OpenCloseDevMenu()
    {
        switch (DevPanelOpen)
        {
            case true:
                DevPanel.SetActive(false);
                DevPanelOpen = false;
                break;
            case false:
                DevPanel.SetActive(true);
                DevPanelOpen = true;
                break;
        }
    }

    /// <summary>
    /// Disables the Dev Menu from being used (function called from button)
    /// </summary>
    public void DisableDevMenu()
    {
        DevMenuAvailable = false;
        OpenCloseDevMenu();
    }
    #endregion
}
