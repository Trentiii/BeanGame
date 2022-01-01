using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            DestroyProjectile();

            //Damage
        }

        if (other.CompareTag("Ground"))
        {
            DestroyProjectile();
        }

    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    
}
