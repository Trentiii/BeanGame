using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //--Editable varibles--
    public float maxHealth = 10; //Holds current max player health

    //--Public varibles--
    [HideInInspector] public static float playerHealth; //Holds current player health
    [HideInInspector] public Transform currentCheckPoint; //Holds respawn point

    //--Private references--
    private Rigidbody2D rb2;
    private GrapplingGun gg;
    private GrapplingRope gr;
    private GrappleAttacking ga;

    // Start is called before the first frame update
    void Start()
    {
        //Gets references
        rb2 = GetComponent<Rigidbody2D>();
        gg = transform.GetChild(0).GetComponent<GrapplingGun>();
        gr = transform.GetChild(0).GetChild(0).GetComponent<GrapplingRope>();
        ga = GetComponent<GrappleAttacking>();

        //Sets defualt health
        playerHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //If player health is less than or equal to 0
        if (playerHealth <= 0)
        {
            //Start death
            death();
        }
    }

    private void death()
    {
        //Call all full resets
        gg.fullReset();
        gr.fullReset();
        ga.fullReset();


        //Reset postion and velocity
        rb2.velocity = Vector2.zero;
        transform.position = Vector3.zero;
    }

    //Handles damage
    public static void damage(float amount)
    {
        //Increments health by amount
        playerHealth -= amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            death();
        }
    }
}
