using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public AnimatorOverrideController animationOverride;
    public AudioClip[] grossSFX;

    [HideInInspector] public bool grown = false;

    //Holds current y velocity
    private float yVelocity;

    //Hold camera speed
    private float speed;

    //--Private references--
    private Rigidbody2D rb2;
    private GrapplingGun gg;
    private PlayerMovement pm;
    private Animator a;
    private CameraMouseFollow cMF;

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

        if (a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "NewPlayer_HoldFall" || a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "GrossPlayer_HoldFall")
        {
            a.SetBool("InLongFall", true);
        }
        else
        {
            a.SetBool("InLongFall", false);
        }
    }

    //Sets up grown state
    [ContextMenu("Grow Player")]
    public void grow()
    {
        //-----------Stop all player movment and camera-----------
        pm.stopped = true;
        gg.stopped = true;

        //Stop camera follow and save value
        cMF = Camera.main.gameObject.GetComponent<CameraMouseFollow>();
        cMF.centered = true;
        speed = cMF.speed;
        cMF.speed = 2;

        //-----------Start transformation and change animations/sfx-----------
        a.SetBool("Transforming", true); //Set tranforming parameter
        a.Play("Base Layer.Transformation", 0); //Start transformation animation

        a.runtimeAnimatorController = animationOverride; //Override new animations

        //Get jump sfx and swap out clip and volume
        AudioSource jumpAS = GetComponents<AudioSource>()[1];
        jumpAS.clip = grossSFX[0];
        jumpAS.volume = 0.75f;

        //Get death sfx and swap out clip
        AudioSource deathAS = GetComponents<AudioSource>()[2];
        deathAS.clip = grossSFX[1];
        //jumpAS.volume = 0.75f;

        grown = true; //Set grow to true

        //Call transformingOff in 2.25 seconds
        Invoke("transformingOff", 2.25f);
    }

    private void transformingOff()
    {
        //Reset transforming
        a.SetBool("Transforming", false);

        //Reset player
        pm.stopped = false;
        gg.stopped = false;

        cMF.centered = false;
        cMF.speed = speed;
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
