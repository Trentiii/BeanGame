using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour
{
    public float speed;
    public float lineOfSight;
    public float retreatDistance;
    private float playerDistance;
    private float waitTime;
    public float startWaitTime;

    public Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Transform[] moveSpots;
    public LayerMask ground;
    private int nextSpot = 0;


    public enum State
    {
        idle,
        patrol,
        retreat
    }
    public State currentState = State.patrol;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


        playerDistance = Vector2.Distance(player.position, transform.position);

        if (playerDistance <= retreatDistance)
        {
            currentState = State.retreat;
        }
        else if (playerDistance > lineOfSight)
        {
            currentState = State.patrol;

        }

        switch (currentState)
        {
            case State.idle:
                Idling();
                break;

            case State.patrol:
                Patrolling();
                break;

            case State.retreat:
                Retreat();
                break;

            default:
                Debug.Log("State defaulted");
                break;
        }

    }

    private void Idling()
    {
        //Tell animator to idle
        // ani.SetTrigger("Idling");
        speed = 0;
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void Patrolling()
    {
        facer(moveSpots[nextSpot].position - transform.position);
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[nextSpot].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[nextSpot].position) < 0.2f)
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

    }

    private void Retreat()
    {
        Vector2 dir = -(player.position - transform.position);

        if (groundChecker(dir))
        {
            speed = 3;

            facer(dir);

            if (groundChecker(dir))
            {
                transform.position = new Vector2(Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime).x, transform.position.y);
            }
        }
           
      
        //Runs away when player gets into range
        //ani.SetTrigger("Retreating");

        /* speed = 2.5f;
         facer(player.position - transform.position);
         transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        */
        //Also attack while flee-ing

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

    private bool groundChecker(Vector2 direction)
    {
        RaycastHit2D hit;

        if (direction.x < 0)
        {
            Debug.DrawRay(transform.position + new Vector3(-0.3f, 0.5f, 0), -(transform.up + transform.right).normalized * 1.5f);
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0.5f, 0), -(transform.up + transform.right).normalized * 1.5f, 1.5f, ground);
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-0.3f, 0.5f, 0), -(transform.up - transform.right).normalized * 1.5f);
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0.5f, 0), -(transform.up - transform.right).normalized * 1.5f, 1.5f, ground);
        }

        return hit.transform != null;
    }


    private void OnDrawGizmosSelected()
    {
        //Radius for sight
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        //Radius for attacking
        
        //Radius for retreating
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
        //Raycast ground detection
        Gizmos.color = Color.yellow;
        if (player != null) Gizmos.DrawLine(transform.position, player.position);

    }
}
