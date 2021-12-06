using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Rigidbody2D rb;
    GunCopy gc;
    Animator animator;
    CopyMove cm;

    // Start is called before the first frame update
    void Start()
    {
        gc = transform.GetChild(0).GetComponent<GunCopy>();
        cm = GetComponent<CopyMove>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", Mathf.Abs(cm.xInput));
        Debug.Log(gc.grappling);

        if (!gc.grappling)
        {
            animator.SetBool("tongue", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("tongue", true);
        }

        //Jump Up
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
        }
        //Fall
        if (Input.GetButtonDown("Jump") && rb.velocity.y < 0.01)
        {
            animator.SetBool("Fall", true);
        }

        if(cm.grounded == true)
        {
            animator.SetBool("Fall", false);
        }
    }
}
