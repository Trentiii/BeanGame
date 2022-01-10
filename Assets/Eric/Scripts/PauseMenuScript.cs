using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject PausePanel;
    private Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        ani = PausePanel.GetComponent<Animator>();
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
