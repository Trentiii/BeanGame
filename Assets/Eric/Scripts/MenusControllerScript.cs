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
    private bool PausePanelOpen = false;
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

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.SetActive(false);
        DevPanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Calls function for controlling dev menu
        if (Input.GetKeyDown(KeyCode.BackQuote) && DevMenuAvailable)
            OpenCloseDevMenu();

        //Calls function for controlling pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenClosePauseMenu();
            

    }

    #region PauseMenu
    /// <summary>
    /// Controls the opening and closing of the pause menu
    /// </summary>
    private void OpenClosePauseMenu()
    {
        switch (PausePanelOpen)
        {
            case true:
                PauseMenu.SetActive(false);
                PausePanelOpen = false;
                break;
            case false:
                PauseMenu.SetActive(true);
                PausePanelOpen = true;
                break;
        }
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
