using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject GVolume;
    public Animator ani;
    public bool pauseOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pauseOpen)
            {
                closePauseMenu();
                
                //Mathf.Lerp()
            }
            else if (!pauseOpen)
            {
                openPauseMenu();
            }
        }
    }
    
    public void openPauseMenu()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
        pauseOpen = !pauseOpen;
        //Debug.Log("Open");
    }
    public void closePauseMenu()
    {
        StartCoroutine(closePauseMenuCoroutine());
        pauseOpen = !pauseOpen;
        Time.timeScale = 1f;
        //Debug.Log("Close");
    }

    IEnumerator closePauseMenuCoroutine()
    {
        ani.SetTrigger("PauseDone");

        yield return new WaitForSecondsRealtime(0.5f);

        PausePanel.SetActive(false);
    }
}
