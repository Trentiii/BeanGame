using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScript : MonoBehaviour
{
    public GameObject mainTutorial;
    private TMP_Text tmpt;

    bool on = false;

    // Start is called before the first frame update
    void Start()
    {
        mainTutorial.SetActive(false);
        tmpt = mainTutorial.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            tmpt.color += new Color(1, 1, 1, 0.05f);

            if (tmpt.color.a >= 1)
            {
                this.gameObject.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            on = true;
            mainTutorial.SetActive(true);
        }
    }
}
