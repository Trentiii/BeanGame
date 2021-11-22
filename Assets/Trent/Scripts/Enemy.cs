using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;
    public GameObject projectile2;
    public Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }


    
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = new Vector2(Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime).x , transform.position.y);
        } else if(Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        } else if(Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = new Vector2(Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime).x , transform.position.y);
        }


        if(timeBtwShots <= 0)
        {
            GameObject Clone = Instantiate(projectile, transform.position, Quaternion.identity);
            GameObject Clone = Instantiate(projectile2, transform.position, Quaternion.identity);
            Clone.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * 1000);
            Destroy(Clone, 5);
            timeBtwShots = startTimeBtwShots;
        } else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
