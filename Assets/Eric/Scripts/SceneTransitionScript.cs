using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionScript : MonoBehaviour
{

    Faucet f;

    // Start is called before the first frame update
    void Start()
    {
        f = GameObject.Find("Faucet").GetComponent<Faucet>();
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
        f.transitionToIndex(SceneManager.GetActiveScene().buildIndex);
    }
    public void nextScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        f.transitionToIndex(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void reloadThisScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        f.transitionToIndex(SceneManager.GetActiveScene().buildIndex);
    }
    public void quitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }

    public void creditScene()
    {
        //SceneManager.LoadScene("CreditScene");
        f.transitionToIndex(2);
    }

    public void mainMenuScene()
    {
        //SceneManager.LoadScene("MainMenuScene");
        f.transitionToIndex(0);
    }
}

