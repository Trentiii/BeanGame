using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float damage = 1;
    public GameObject deatheffects;

    bool remove = false;
    private Transform cloneHolder;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            cloneHolder = GameObject.Find("CloneHolder").transform;
        }
        catch
        {
            Debug.LogError("Cannot find clone holder for bullet particles, please add/activate an empty gameobject titled \"CloneHolder\"");
        }
    }

    private void Update()
    {
        if (PlayerHealth.dying)
        {
            remove = true;
        }

        if (remove && Time.timeScale > 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {

            DestroyProjectile();

            //Damage
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().damage(damage);
        }

        if (other.CompareTag("Ground") || other.CompareTag("Ungrapplable"))
        {
            DestroyProjectile();
        }

    }

    void DestroyProjectile()
    {
        Destroy(Instantiate(deatheffects, transform.position, Quaternion.identity, cloneHolder), 0.5f);
        Destroy(gameObject);
    }

    
}
