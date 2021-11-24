using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ani.SetTrigger("Beat");
            Debug.Log("Beating...   Keycode : Space");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ani.SetTrigger("ToDisgust");
            Debug.Log("Fading to disgust...    Keycode : P");
        }
    }
    
}
