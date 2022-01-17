using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControllerScript : MonoBehaviour
{
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        StartCoroutine(RealOpenDoor());
    }

    IEnumerator RealOpenDoor()
    {
        ani.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(1.1f);
        gameObject.SetActive(false);
    }
}
