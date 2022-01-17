using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionScript : MonoBehaviour
{

    public Faucet Faucet;

    private IntroComic ic;

    private void Start()
    {
        ic = GetComponent<IntroComic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            reloadThisScene();
    }

    public void reloadScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Faucet.transitionToIndex(SceneManager.GetActiveScene().buildIndex);
    }
    public void nextScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Faucet.transitionToIndex(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void reloadThisScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Faucet.transitionToIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void startLevel()
    {
        ic.startComic();
    }

    public void quitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }

    public void creditScene()
    {
        //SceneManager.LoadScene("CreditScene");
        Faucet.transitionToIndex(2);
    }

    public void mainMenuScene()
    {
        //SceneManager.LoadScene("MainMenuScene");
        Faucet.transitionToIndex(0);
    }
}

