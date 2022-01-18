using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BossIntro : MonoBehaviour
{
    GameObject boss;

    public AudioMixer musicMixer;

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
            musicMixer.SetFloat("Depth", 1);

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

        boss.GetComponent<BossAi>().start();
        gameObject.SetActive(false);
    }
}
