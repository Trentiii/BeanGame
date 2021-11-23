using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    private LayerMask Ground;
    private GameObject ground;
    public float speed;
    public float lineOfSight;
    public float shootingRange;
    public float fireRate = 1f;
    private float nextFireTime;
    private Transform player;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject bullet;
    public GameObject bulletParent;
    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.FindGameObjectWithTag("Ground");
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer>shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingRange )
        {
            if (timeBtwShots <= 0)
            {
                GameObject Clone = Instantiate(bullet, transform.position, Quaternion.identity);
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position, 10f, Ground);
        if (hit.collider != ground)
        {
            Debug.Log("Stop");
        }
      
      
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        
    }
}
