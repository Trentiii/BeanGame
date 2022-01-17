using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUIScript : MonoBehaviour
{
    public GameObject slider;
    public TMP_Text UICounter;
    private int TotalEnemies = 0;
    private int EnemiesLeft = 0;

    // Start is called before the first frame update
    void Start()
    {
        slider.GetComponent<Slider>().maxValue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        UICounter.text = "Enemies left: " + EnemiesLeft + " / " + TotalEnemies;
        slider.GetComponent<Slider>().value = PlayerHealth.playerHealth;
    }

    public void setTotalEnemies(int enemies)
    {
        TotalEnemies = enemies;
    }

    public void setEnemiesLeft(int enemies)
    {
        EnemiesLeft = enemies;
    }
}
