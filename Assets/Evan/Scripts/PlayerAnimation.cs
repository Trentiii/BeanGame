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

        a.SetFloat("yv", (yVelocity));
        a.SetFloat("speed", Mathf.Abs(pm.xInput));       

        if (!gg.grappling)
        {
            a.SetBool("tongue", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            a.SetBool("tongue", true);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            a.SetTrigger("Jump");
        }


        if (pm.grounded)
        {
            a.SetBool("MidJump", false);
            a.SetBool("grounded", true);
        }
        if (pm.grounded == false)
        {
            a.SetBool("grounded", false);
        }
    }
}
