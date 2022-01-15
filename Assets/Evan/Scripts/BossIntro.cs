using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    GameObject boss;

    PlayerMovement pm;
    GrapplingGun gg;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        pm = player.GetComponent<PlayerMovement>();
        gg = player.transform.GetChild(0).GetComponent<GrapplingGun>();

        boss = GameObject.Find("Boss");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit player
        if (collision.tag == "Player")
        {
            Camera.main.GetComponent<CameraMouseFollow>().bossPoint = boss.transform.position;
            boss.GetComponent<Animator>().SetTrigger("Start");

            pm.stopped = true;
            gg.stopped = true;

            //Call introOff in 1.5 seconds
            Invoke("introOff", 1.5f);
        }
    }
    private void introOff()
    {
        //Reset player
        pm.stopped = false;
        gg.stopped = false;

        Destroy(gameObject);
    }
}
