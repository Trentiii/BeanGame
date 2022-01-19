using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float enemyCounter;
    [Header("Enemies Needed to Unlock Door")]
    public float enemiesNeeded;
    public float HighestUnlockedDoor;
    public GameObject[] Doors;
    // Start is called before the first frame update
    void Start()
    {
        enemyCounter = 0;
        HighestUnlockedDoor = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name);

        if (enemyCounter >= enemiesNeeded)
        {
            try
            {
                Doors[(int)HighestUnlockedDoor + 1].SetActive(false);
                HighestUnlockedDoor++;
            }
            catch (System.Exception)
            {
                throw;
            }
            resetCounter();
        }
    }

    public void EnemyCounter()
    {
        enemyCounter += 1;
    }
    public void resetCounter()
    {
        enemyCounter = 0;
    }
}
