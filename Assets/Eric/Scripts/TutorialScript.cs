using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject mainTutorial;



    // Start is called before the first frame update
    void Start()
    {
        mainTutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mainTutorial.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
