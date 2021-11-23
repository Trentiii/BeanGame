﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Tooltip("Ground layer reference")]
    [SerializeField]
    private LayerMask whatIsGround;

    //--Serialized floats
    [Tooltip("Speed of the player defualt movement")]
    [SerializeField]
    private float movementSpeed = 4;
    [Tooltip("Force of the player's jump")]
    [SerializeField]
    private float jumpForce;
    [Tooltip("How long the jump press will be saved")]
    [SerializeField]
    private float jumpInputHoldTime = 0.3f;
    [Tooltip("Radius size for ground check sphere")]
    [SerializeField]
    private float groundCheckRadius;
    [Tooltip("Time it takes for the player to slow down")]
    [SerializeField]
    private float slowDownTime;
    [Tooltip("Player fall speed multiplyer")]
    [SerializeField]
    private float fallMultiplier;

    //Holds if player is grounded
    public bool grounded;

    //Holds current facing (1 = right)
    private int facingDirection = 1;
    //Holds loction for ground check sphere
    private Transform groundCheckTrans;

    //--Private floats
    //Holds x-input
    private float xInput;
    //Holds last xInput
    private float oldxInput;
    //Holds current turn around times count for left
    private float turnAroundTimerLeft = 0.01f;
    //Holds current turn around times count for right
    private float turnAroundTimerRight = 0.01f;
    //Counts input hold time
    private float jumpInputHoldCounter = 0;

    //--Private bools
    //Holds if the player is in turn around slow left
    private bool turnAroundSlowLeft = false;
    //Holds if the player is in turn around slow right
    private bool turnAroundSlowRight = false;
    //Holds if jump is needed next fixed update
    private bool jumpNeeded = false;

    //--Private Vector2s
    //Temporally Holds new velocitys
    private Vector2 newVelocity;
    //Temporally Holds new forces
    private Vector2 newForce;

    //Component References
    private Rigidbody2D rb2;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        //Get component References
        rb2 = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        groundCheckTrans = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        //Activate input check
        CheckInput();

        //Increments timers
        if (jumpInputHoldCounter > 0)
        {
            jumpInputHoldCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //Activate the ground check, movment, slope check, and the two managers
        CheckGround();
        GetMovement();
        //DownSpeedManager();
    }

    private void Jump()
    {
        //Jumps if grounded is true
        if (grounded)
        {
            //Toggles grounded
            grounded = false;

            //Turns off counter
            jumpInputHoldCounter = 0;

            //Adds jump force
            newForce.Set(0.0f, jumpForce + -rb2.velocity.y);
            rb2.AddForce(newForce, ForceMode2D.Impulse);
        }
        else if(jumpInputHoldCounter <= 0)
        {
            //Starts input holding counter
            jumpInputHoldCounter = jumpInputHoldTime;
        }
    }

    private void GetMovement()
    {
        if (grounded)
        {
            //Gets flat movement
            newVelocity.Set(movementSpeed * xInput, rb2.velocity.y);
            //Sends newVelocity to ApplyMovement
            ApplyMovement(newVelocity);
        }
        else if (!grounded)
        {
            //Gets air movement
            newVelocity.Set(movementSpeed * xInput, rb2.velocity.y);
            //Sends newVelocity to ApplyMovement
            ApplyMovement(newVelocity);
        }
    }

    public void ApplyMovement(Vector2 velocityToUse)
    {
        //Checks for changes in xInput
        if ((oldxInput != xInput && xInput != 0.0f) && xInput == -1f)
        {
            //Turns on slow left
            turnAroundSlowLeft = true;

            //Turns off slow right
            turnAroundSlowRight = false;
            turnAroundTimerRight = 0.01f;
        }
        else if ((oldxInput != xInput && xInput != 0.0f) && xInput == 1f)
        {
            //Turns on slow right
            turnAroundSlowRight = true;

            //Turns off slow left
            turnAroundSlowLeft = false;
            turnAroundTimerLeft = 0.01f;
        }

        //Checks if slow down changes are needed
        if (turnAroundSlowLeft)
        {
            //Slows down x speed
            velocityToUse.Set(velocityToUse.x * (turnAroundTimerLeft / slowDownTime), velocityToUse.y);

            //increments turnAroundCounter
            turnAroundTimerLeft += Time.deltaTime;

            //Checks if turnAroundTime is done
            if (turnAroundTimerLeft >= slowDownTime)
            {
                //Turns off slow turn around and resets timer
                turnAroundSlowLeft = false;
                turnAroundTimerLeft = 0.01f;
            }
        }
        else if (turnAroundSlowRight)
        {
            //Slows down x speed
            velocityToUse.Set(velocityToUse.x * (turnAroundTimerRight / slowDownTime), velocityToUse.y);

            //increments turnAroundCounter
            turnAroundTimerRight += Time.deltaTime;

            //Checks if turnAroundTime is done
            if (turnAroundTimerRight >= slowDownTime)
            {
                //Turns off slow turn around and resets timer
                turnAroundSlowRight = false;
                turnAroundTimerRight = 0.01f;
            }
        }

        if ((rb2.velocity.x > movementSpeed && velocityToUse.x < 0) || (rb2.velocity.x < -movementSpeed && velocityToUse.x > 0))
        {
            rb2.velocity += velocityToUse;
        }
        else
        {
            rb2.velocity = velocityToUse;
        }

        //Gets old xInput
        oldxInput = xInput;
    }

    private void CheckGround()
    {
        //Checks for ground on LayerMask layer within a sphere
        grounded = Physics2D.OverlapCircle(groundCheckTrans.position, groundCheckRadius, whatIsGround);
    }

    //Checks for inputs
    private void CheckInput()
    {
        //Logs xInput to varible
        xInput = Input.GetAxisRaw("Horizontal");
        

        //Flips player
        if (xInput == -facingDirection)
        {
            facingDirection *= -1;
            sr.flipX = !sr.flipX;
        }

        //Calls Jump() when jump is pressed or the input is currently being held
        if (Input.GetButtonDown("Jump") || jumpInputHoldCounter > 0)
        {
            Jump();
        }
    }

    //Manages downaward speed
    private void DownSpeedManager()
    {
        //Speeds up normal fall or low jump fall
        if (rb2.velocity.y < 0 && !grounded)
        {
            rb2.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2.velocity.y > 0 && !Input.GetButton("Jump") && !grounded)
        {
            rb2.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
}
