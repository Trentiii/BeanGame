﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            reloadThisScene();
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void nextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void reloadThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void quitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }

    public void creditScene()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void mainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}

