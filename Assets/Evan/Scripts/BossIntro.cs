using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    GameObject boss;

    private void Start()
    {
        boss = GameObject.Find("Boss");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit player
        if (collision.tag == "Player")
        {
            Camera.main.GetComponent<CameraMouseFollow>().bossPoint = boss.transform.position;
            boss.GetComponent<Animator>().SetTrigger("Start");
        }
    }
}
