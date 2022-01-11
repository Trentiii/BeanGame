using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private float enemyCounter;
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
        if (enemyCounter == 1)
        {
            unlockDoor = true;
        }
        if (unlockDoor == true)
        {
            gameObject.SetActive(false);
            Debug.Log("DoorUnlocked!");
        }
    }

    public void EnemyCounter()
    {
        enemyCounter += 1;
        Debug.Log("+1");
        
    }

    /*private void UnlockDoor()
    {
        if (enemyCounter == 1)
        {
            unlockDoor = true;
        }
        if (unlockDoor == true)
        {
            gameObject.SetActive(false);
        }

        UnlockDoor();
    }*/
}
