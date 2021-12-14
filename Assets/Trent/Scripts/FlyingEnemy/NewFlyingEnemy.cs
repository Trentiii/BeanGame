using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlyingEnemy : MonoBehaviour
{
    public LayerMask playerLayer;
    private Rigidbody2D rb;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;

    private bool idling;
    
    private int randomSpot;
    Vector2 hover;
    public LayerMask ground;
    public float speed;
    public float lineOfSight;
    private float playerDistance;
    private float groundDistance;
    public float retreatDistance;
    public float attackDistance;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public Transform player;
    public GameObject Ground;
    public GameObject projectile;
    public enum State
    {
        idle,
        attacking,
        patrolling,
        following,
        grappled,
        retreat

    }

    public State currentState = State.idle;
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        
        randomSpot = Random.Range(0, moveSpots.Length);
        hover = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Ground = GameObject.FindGameObjectWithTag("Ground").gameObject;
        //ATTACKING 
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
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
            case State.grappled:
                break;
            case State.retreat:
                Retreat();
                break;
            default:
                Debug.Log("State defaulted");
                break;
        }
       // groundDistance = Vector2.Distance(ground, transform.position);
        playerDistance = Vector2.Distance(player.position, transform.position);
        if (playerDistance < lineOfSight && playerDistance > attackDistance)
        {
            //transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            currentState = State.following;
        }
        else if (playerDistance <= attackDistance && playerDistance > retreatDistance)
        {
            currentState = State.attacking;
        }
        else if (playerDistance <= retreatDistance)
        {
            currentState = State.retreat;
        }
        else if(playerDistance > lineOfSight)
        {
            currentState = State.idle;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position);
        if (hit.transform.gameObject.layer == 8)
        {
            currentState = State.idle;
            
        }
        /*
        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, player.position, 10f, playerLayer);
        if (hitPlayer.collider == player)
        {
            currentState = State.following;
            Debug.Log("Start");
        }
        */

       /* RaycastHit2D hitGround = Physics2D.Raycast(transform.position, player.position, 10f);
        if (hitGround.collider == GameObject.FindGameObjectWithTag("Ground"))
        {
            currentState = State.idle;
            Debug.Log("Stop");
        }
      /*  else
        {
            currentState = State.following;
        }*/

    }
    //What happens when Idling
    private void Idling()
    {
        idling = true;
        //Tell animator to idle
        ani.SetTrigger("Idling");
        speed = 0;
        rb.velocity = new Vector2(speed, rb.velocity.y);
        
        hover = Vector2.up * Mathf.Sin(Time.time * 2) / 15;

        while (idling)
        {
            new WaitForSeconds(1.0f);
            currentState = State.patrolling;
        }
        

    }

    //What happens when following player
    private void Following()
    {
        //Play flying towards animation and get within certain distance
        ani.SetTrigger("Following");
        speed = 5;
        transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
    }
    //What happens when attacking
    private void Attacking()
    {
        //Stop and shoot projectiles at player
        //Play attacking animation
        //Instantiate projectiles
        ani.SetBool("Attacking", true);

        if (timeBtwShots <= 0)
        {
            GameObject Clone = Instantiate(projectile, transform.position, Quaternion.identity);
            //GameObject Clone = Instantiate(projectile2, transform.position, Quaternion.identity);
            Clone.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * 1000);
            Destroy(Clone, 5);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    //What happens when patrolling
    private void Patrolling()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if(waitTime <= 0)
            {
                
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        //Looks around the area for the player
        //Patroll between points
        ani.SetTrigger("Patrolling");
    }
    //What happens when retreating
    private void Retreat()
    {
        //Runs away when player gets into range
        ani.SetBool("Retreating", true);
        transform.position = (Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime));
    }

    //What happens when grappled
    private void Grappled()
    {
        //stop everything and play grappled animation
    }

    private void OnDrawGizmosSelected()
    {
        //Radius for sight
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        //Radius for attacking
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        //Radius for retreating
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
        //Raycast ground detection
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.position);
        

    }
}
