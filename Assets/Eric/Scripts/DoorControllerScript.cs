using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControllerScript : MonoBehaviour
{
    public Animator ani;
    private MainUIScript mus;
    [HideInInspector] public bool arena1 = false;
    [HideInInspector] public bool open = false;

    private void Awake()
    {
        if (transform.position.x < 26.45f)
        {
            arena1 = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mus = GameObject.Find("MainUIPanel").GetComponent<MainUIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (arena1 && mus.TotalEnemies == 10 && mus.EnemiesLeft == 0)
        {
            OpenDoor();
        }
        else if (!arena1 && mus.TotalEnemies == 25 && mus.EnemiesLeft == 0)
        {
            OpenDoor();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        open = true;
        StartCoroutine(RealOpenDoor());
    }

    IEnumerator RealOpenDoor()
    {
        ani.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(1.1f);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
