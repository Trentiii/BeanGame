﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerEnemy : MonoBehaviour
{
    public float startWaitTime;
    public float speed;
    public float lineOfSight;
    public float retreatDistance;
    public float attackDistance;
    public int bullets = 10;
    public float bulletSpeed;

    private Transform cloneHolder;
    private float waitTime;
    private float playerDistance;
    private int nextSpot = 0;
    private float sightAngle;

    public GameObject projectile;
    public LayerMask groundAndPlayer;
    public LayerMask ground;
    public Transform[] moveSpots;

    public enum State
    {
        idle,
        attacking,
        patrolling,
        following,
        grappled,
        retreat

    }
    public State currentState = State.patrolling;

    bool remove = false;

    private Transform player;
    private Animator ani;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            cloneHolder = GameObject.Find("CloneHolder").transform;
        }
        catch
        {
            Debug.LogError("Cannot find clone holder for bullets, please add/activate an empty gameobject titled \"CloneHolder\"");
        }

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        waitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.position - transform.position).x < 0)
        {
            sightAngle =  Vector2.Angle(new Vector2(-1, 0), player.position - transform.position);
        }
        else
        {
            sightAngle = Vector2.Angle(new Vector2(1, 0), player.position - transform.position);
        }

        if (PlayerHealth.dying)
        {
            remove = true;
        }
        if (remove && Time.timeScale > 0)
        {
            Destroy(gameObject);
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), 0);

        if (currentState != State.grappled)
        {
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !ani.GetCurrentAnimatorStateInfo(0).IsName("Idle 0"))
            {
                //Play attacking animation
                ani.SetBool("Attacking", false);
            }

            playerDistance = Vector2.Distance(player.position, transform.position);
            if (playerDistance < lineOfSight && playerDistance > attackDistance && (sightAngle > -45 && sightAngle < 45))
            {
                currentState = State.following;
            }
            else if (playerDistance <= attackDistance && playerDistance > retreatDistance && (sightAngle > -45 && sightAngle < 45))
            {
                currentState = State.attacking;
            }
            else if (playerDistance <= retreatDistance && (sightAngle > -45 && sightAngle < 45))
            {
                currentState = State.retreat;
            }
            else if (playerDistance > lineOfSight)
            {
                currentState = State.patrolling;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, 100, groundAndPlayer);
            if (hit.transform != null && hit.transform.gameObject.layer == 8)
            {
                currentState = State.patrolling;
            }

            switch (currentState)
            {
                case State.idle:
                    Idling();
                    break;

                case State.patrolling:
                    Patrolling();
                    break;

                case State.attacking:
                    Attacking();
                    break;

                case State.following:
                    Following();
                    break;

                case State.retreat:
                    Retreat();
                    break;

                default:
                    Debug.Log("State defaulted");
                    break;
            }
        }
        else
        {
            speed = 0;
            rb.velocity = new Vector2(0, 0);

            ani.SetBool("Grappled", true);
        }

    }
    //What happens when Idling
    private void Idling()
    {
        //Tell animator to idle
        ani.SetTrigger("Idling");
        speed = 0;
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    //What happens when following player
    private void Following()
    {
        //Play moving towards animation and get within certain distance
        ani.SetTrigger("Following");
        speed = 3;

        facer(player.position - transform.position);
        transform.position = new Vector2(Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime).x, transform.position.y);
    }
    //What happens when attacking
    private void Attacking()
    {
        //Stop and shoot projectiles at player
        //Play attacking animation
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !ani.GetCurrentAnimatorStateInfo(0).IsName("Idle 0"))
        {
            ani.SetTrigger("AttackStart");
        }

        ani.SetBool("Attacking", true);
    }

    [ContextMenu("test")]
    public void Shoot()
    {
        Vector2 direction = player.transform.position - transform.position;

        for (int i = 0; i < bullets; i++)
        {
            GameObject Clone = Instantiate(projectile, transform.position + new Vector3(-0.3f, 0.5f, 0), Quaternion.identity, cloneHolder);
            Clone.GetComponent<Rigidbody2D>().AddForce(direction.normalized * bulletSpeed + new Vector2(Random.Range(-1.5f, 1.0f), Random.Range(-1.5f, 1.2f)) , ForceMode2D.Impulse);
            Destroy(Clone, 5);
        }
    }

    //What happens when patrolling
    private void Patrolling()
    {
        facer(moveSpots[nextSpot].position - transform.position);
        transform.position = new Vector2(Vector2.MoveTowards(transform.position, moveSpots[nextSpot].position, speed * Time.deltaTime).x, transform.position.y);
        if (Vector2.Distance(transform.position, new Vector2(moveSpots[nextSpot].position.x, transform.position.y)) < 0.2f)
        {
            if (waitTime <= 0)
            {
                //randomSpot = Random.Range(0, moveSpots.Length);
                if (nextSpot < moveSpots.Length - 1)
                {
                    nextSpot++;
                }
                else
                {
                    nextSpot = 0;
                }

                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        //Looks around the area for the player
        //Patrol between points

        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Patrol"))
        {
            ani.SetTrigger("Patrolling");
        }
    }
    //What happens when retreating
    private void Retreat()
    {
        speed = 2.5f;
        facer(player.position - transform.position);
        transform.position = new Vector2(Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime).x, transform.position.y);

        //Also attack while flee-ing
        Attacking();
    }

    private void facer(Vector2 direction)
    {
        if (direction.x > 0)
        {
            sr.flipX = true;
        }
        else if (direction.x < 0)
        {
            sr.flipX = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Radius for sight (While lines to make the cone)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, (transform.up + transform.right).normalized * lineOfSight);
        Gizmos.DrawRay(transform.position, (transform.up - transform.right).normalized * lineOfSight);
        Gizmos.DrawRay(transform.position, -(transform.up + transform.right).normalized * lineOfSight);
        Gizmos.DrawRay(transform.position, -(transform.up - transform.right).normalized * lineOfSight);
        //Radius for attacking
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        //Radius for retreating
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
        //Raycast ground detection
        Gizmos.color = Color.yellow;
        if (player != null) Gizmos.DrawLine(transform.position, player.position);

    }
}
