using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PausePanel;
    public Animator ani;
    public bool pauseOpen = false;
    public float openCoolDown = 0f;
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
            if (pauseOpen && Time.time > openCoolDown)
            {
                StartCoroutine(closePauseMenu());
                openCoolDown = Time.time + 0.42f;
                pauseOpen = !pauseOpen;
            }
            else if(!pauseOpen && Time.time > openCoolDown)
            {
                PausePanel.SetActive(true);
                openCoolDown = Time.time + 0.42f;
                pauseOpen = !pauseOpen;
            }
        }
        
    }

    IEnumerator closePauseMenu()
    {
        ani.SetTrigger("PauseDone");

        yield return new WaitForSeconds(0.5f);

        PausePanel.SetActive(false);
    }
}
