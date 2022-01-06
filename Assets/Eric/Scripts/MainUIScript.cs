using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIScript : MonoBehaviour
{
    public GameObject slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.GetComponent<Slider>().maxValue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
        slider.GetComponent<Slider>().value = PlayerHealth.playerHealth;
    }
}
