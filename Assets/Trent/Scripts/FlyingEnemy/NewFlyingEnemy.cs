using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlyingEnemy : MonoBehaviour
{
    public float startWaitTime;
    public float speed;
    public float lineOfSight;
    public float retreatDistance;
    public float attackDistance;
    public float LaunchAngle = 70;
    //public GameObject Ground;

    private Transform cloneHolder;
    private float waitTime;
    private float playerDistance;
    //private int randomSpot;
    private int nextSpot = 0;
    //private bool idling;
    //private float groundDistance;

    public GameObject projectile;
    public LayerMask groundAndPlayer;
    public LayerMask ground;
    public Transform[] moveSpots;
    //public LayerMask playerLayer;

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
    private PolygonCollider2D pc2d;

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
        pc2d = GetComponent<PolygonCollider2D>();
        //Ground = GameObject.FindGameObjectWithTag("Ground").gameObject;

        waitTime = startWaitTime;
        //randomSpot = Random.Range(0, moveSpots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.dying)
        {
            remove = true;
        }
        if (remove && Time.timeScale > 0)
        {
            Destroy(gameObject);
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), Mathf.Clamp(rb.velocity.y, -speed, speed));

        if (currentState != State.grappled)
        {
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                //Play attacking animation
                ani.SetBool("Attacking", false);
            }

            //groundDistance = Vector2.Distance(ground, transform.position);
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
            else if (playerDistance > lineOfSight)
            {
                currentState = State.patrolling;
                pc2d.isTrigger = true;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, 100, groundAndPlayer);
            if (hit.transform != null && hit.transform.gameObject.layer == 8)
            {
                currentState = State.patrolling;
                pc2d.isTrigger = true;
            }
            else if (playerDistance <= lineOfSight)
            {
                pc2d.isTrigger = false;
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

                case State.grappled:
                    Grappled();
                    break;

                case State.retreat:
                    Retreat();
                    break;

                default:
                    Debug.Log("State defaulted");
                    break;
            }

            #region oldCode
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
            #endregion
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
        //Play flying towards animation and get within certain distance
        ani.SetTrigger("Following");
        speed = 3;

        facer(player.position - transform.position);
        transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
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

    public void Shoot()
    {
        shootMath(LaunchAngle);
    }

    private void shootMath(float mathLaunchAngle)
    {
        if (Vector2.Distance(player.position, transform.position) < attackDistance)
        {
            for (int i = 0; i < 3; i++)
            {
                //Instantiate projectiles
                GameObject Clone = Instantiate(projectile, transform.position + new Vector3(-0.25f, 0.7f, 0), Quaternion.identity, cloneHolder);
                //GameObject Clone = Instantiate(projectile2, transform.position, Quaternion.identity);

                Vector3 projectileXPos = new Vector3(Clone.transform.position.x, 0, 0);
                Vector3 targetXxPos = new Vector3(player.transform.position.x, 0, 0);

                //shorthands for the formula
                float R = Vector3.Distance(projectileXPos, targetXxPos);
                float G = Physics.gravity.y;
                float tanAlpha = Mathf.Tan(mathLaunchAngle * Mathf.Deg2Rad);
                float H = player.transform.position.y - Clone.transform.position.y;

                //calculate the local space components of the velocity 
                // required to land the projectile on the target object 
                float Vx = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
                float Vy = tanAlpha * Vx;

                //create the velocity vector in local space and get it in global space
                Vector3 localVelocity = new Vector3(Vx, Vy, 0);
                Vector3 globalVelocity = transform.TransformDirection(localVelocity);
                //---------------

                if (i == 1)
                {
                    globalVelocity += new Vector3(0.15f * globalVelocity.magnitude, 0, 0);
                }
                else if (i == 2)
                {
                    globalVelocity += new Vector3(-0.15f * globalVelocity.magnitude, 0, 0);
                }

                if (player.transform.position.x < Clone.transform.position.x)
                {
                    globalVelocity = new Vector3(-globalVelocity.x, globalVelocity.y, 0);
                }

                if (i == 0)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Clone.transform.position, globalVelocity, globalVelocity.magnitude / 3, ground);
                    Debug.DrawRay(Clone.transform.position, globalVelocity / 3, Color.black, 1);
                    if (hit.transform != null && mathLaunchAngle > 0)
                    {
                        Destroy(Clone);
                        shootMath(mathLaunchAngle - 10);
                        break;
                    }
                    else
                    {
                        if (globalVelocity.magnitude > 11)
                        {
                            Destroy(Clone);
                            break;
                        }
                        else
                        {
                            Clone.GetComponent<Rigidbody2D>().AddForce(globalVelocity, ForceMode2D.Impulse);
                            Destroy(Clone, 5);
                        }
                    }
                }
                else
                {
                    Clone.GetComponent<Rigidbody2D>().AddForce(globalVelocity, ForceMode2D.Impulse);
                    Destroy(Clone, 5);
                }
            }
        }
    }

    //What happens when patrolling
    private void Patrolling()
    {
        facer(moveSpots[nextSpot].position - transform.position);
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[nextSpot].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[nextSpot].position) < 0.2f)
        {
            if(waitTime <= 0)
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
        //Patroll between points

        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Patrol"))
        {
            ani.SetTrigger("Patrolling");
        }
    }
    //What happens when retreating
    private void Retreat()
    {
        //Runs away when player gets into range
        //ani.SetTrigger("Retreating");

        speed = 2.5f;
        facer(player.position - transform.position);
        transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);

        //Also attack while flee-ing
        Attacking();
    }

    //What happens when grappled
    private void Grappled()
    {
        //stop everything and play grappled animation
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
        if(player != null) Gizmos.DrawLine(transform.position, player.position);
       
    }
}
