using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float damage = 1;
    public GameObject deatheffects;

    bool remove = false;
    private Transform cloneHolder;
    private Rigidbody2D rb2;
    private AudioSource aS;
    private AudioSource aS2;

    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        aS = GetComponent<AudioSource>();
        aS2 = GetComponents<AudioSource>()[1];

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
        var dir = rb2.velocity;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 360 * Time.deltaTime);

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
        if (other.CompareTag("Player"))
        {

            DestroyProjectile(aS);

            //Damage
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().damage(damage);
        }

        if (other.CompareTag("Ground") || other.CompareTag("Ungrapplable"))
        {
            DestroyProjectile(aS2);
        }

    }

    void DestroyProjectile(AudioSource currentAs)
    {
        //Randomize pitch and play damage sound
        currentAs.pitch = Random.Range(1.2f, 1.3f);
        currentAs.Play();

        Destroy(Instantiate(deatheffects, transform.position, Quaternion.identity, cloneHolder), 0.5f);

        transform.position = new Vector3(0, -50, 0);

        Destroy(gameObject, 0.25f);
    }

    
}
