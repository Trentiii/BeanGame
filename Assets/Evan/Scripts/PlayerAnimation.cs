using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private float yVelocity;
    private Rigidbody2D rb;
    public GunCopy gc;
    public Animator animator;
    public CopyMove cm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        yVelocity = rb.velocity.y;

        animator.SetFloat("yv", (yVelocity));
        animator.SetFloat("speed", Mathf.Abs(cm.xInput));
        Debug.Log(animator.name);
        

        if (!gc.grappling)
        {
            animator.SetBool("tongue", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("tongue", true);
        }

        //Jump Up
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }

        if (yVelocity > 0.01)
        {
            animator.SetBool("MidJump", true);
        }

        if(yVelocity < 0.01)
        {
            animator.SetBool("Jump", false);
        }
        //Fall
        /*if (rb.velocity.y < 0.01)
        {
            animator.SetBool("Fall", true);
           // animator.SetBool("Jump", false);
        }*/

        
       /* if (rb.velocity.y < 0.01)
        {
            
            animator.SetBool("Jump", false);
        }*/

        if(cm.grounded == true)
        {
            animator.SetBool("MidJump", false);
        }

        if (cm.grounded == true)
        {
            animator.SetBool("grounded", true);
            
        }
        if (cm.grounded == false)
        {
            animator.SetBool("grounded", false);
        }
    }
}
