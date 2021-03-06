using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUIScript : MonoBehaviour
{
    public Slider slider;
    public Slider BeanSlider;
    [HideInInspector] public int TotalEnemies = 0;
    [HideInInspector] public int EnemiesLeft = 0;

    // Start is called before the first frame update
    void Start()
    {
        slider.GetComponent<Slider>().maxValue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //UICounter.text = "Enemies left: " + EnemiesLeft + " / " + TotalEnemies;
        BeanSlider.maxValue = TotalEnemies;
        BeanSlider.value = TotalEnemies - EnemiesLeft;
        BeanSlider.value = TotalEnemies - EnemiesLeft;
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

    public void killEnemy()
    {
        EnemiesLeft--;
    }
}
