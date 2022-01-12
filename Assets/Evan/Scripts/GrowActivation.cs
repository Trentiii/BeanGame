using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowActivation : MonoBehaviour
{
    PlayerAnimation pa;
    AudioSource aS;

    // Start is called before the first frame update
    void Start()
    {
        pa = GameObject.Find("Player").GetComponent<PlayerAnimation>();
        aS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit player
        if (collision.tag == "Player")
        {
            //Start grow
            pa.grow();

            aS.Play();

            //Destroy self
            Destroy(gameObject, 2.25f);
        }
    }
}
