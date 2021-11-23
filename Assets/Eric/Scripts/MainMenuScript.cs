using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    Color Discusting = new Color(0.69f,0.1f,0.1f,1);
    Color Normal = new Color(1, 1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().color = Normal;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Image>().color = Color.Lerp(Normal, Discusting, 2 * Time.deltaTime);
    }
    
}
