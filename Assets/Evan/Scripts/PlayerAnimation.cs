using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //Holds current y velocity
    private float yVelocity;

    //--Private references--
    private Rigidbody2D rb2;
    private GrapplingGun gg;
    private PlayerMovement pm;
    private Animator a;

    // Start is called before the first frame update
    void Start()
    {
        //Get references
        rb2 = GetComponent<Rigidbody2D>();
        gg = transform.GetChild(0).GetComponent<GrapplingGun>();
        pm = GetComponent<PlayerMovement>();
        a = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get current yVelocity
        yVelocity = rb2.velocity.y;

        //Set yv and speed
        a.SetFloat("yv", (yVelocity));
        a.SetFloat("speed", Mathf.Abs(pm.xInput));

        //If not grappling and not attacking
        if (!gg.grappling && !gg.attacking)
        {
            //Set tonuge to false
            a.SetBool("tongue", false);
        }

        //IF mouse is pressed or player attacking
        if (Input.GetMouseButtonDown(0) || gg.attacking)
        {
            //Set tonuge to true
            a.SetBool("tongue", true);
        }

        //If space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Start jump trigger
            a.SetTrigger("Jump");
        }

        //If grounded
        if (pm.grounded && pm.enabled)
        {
            //Set grounded to true
            a.SetBool("grounded", true);
        }
        else
        {
            //Set grounded to false
            a.SetBool("grounded", false);
        }

        if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "NewPlayer_HoldFall")
        {
            a.SetBool("InLongFall", true);
        }
        else
        {
            a.SetBool("InLongFall", false);
        }
    }

    public void fullReset()
    {
        //Reset parameters
        a.SetFloat("yv", 0);
        a.SetFloat("speed", 0);
        a.SetBool("tongue", false);
        a.ResetTrigger("Jump");
        a.SetBool("grounded", false);
        a.SetBool("InLongFall", false);
    }
}
