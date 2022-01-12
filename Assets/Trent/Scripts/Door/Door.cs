using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private float enemyCounter;
    [Header("Enemies Needed to Unlock Door")] 
    public float enemiesNeeded;
    private bool unlockDoor;
    // Start is called before the first frame update
    void Start()
    {
        enemyCounter = 0;
        unlockDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCounter == enemiesNeeded)
        {
            unlockDoor = true;
        }
        if (unlockDoor == true)
        {
            gameObject.SetActive(false);
            DoorReset();
            
            
        }
    }

    public void EnemyCounter()
    {
        enemyCounter += 1;
        
        
    }


    private void DoorReset()
    {
        enemyCounter = 0;
        Debug.Log("Reset!");
    }
    
}
