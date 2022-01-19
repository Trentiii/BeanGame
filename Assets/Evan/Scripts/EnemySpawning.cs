using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    //Holds types of enemies
    public enum Enemy
    {
        flyer,
        farmer
    }

    [Tooltip("Holds one of each enemy prefab (In order of enemy selector below)")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Tooltip("Holds type of enemy this script will spawn")]
    [SerializeField] private Enemy spawnedEnemy;
    [Tooltip("Holds patrol points to give enemy when spawned")]
    [SerializeField] private Transform[] patrolPoints;
    [Tooltip("Holds particles to be spawned on door open")]
    [SerializeField] private GameObject enemyRemoveParticles;


    //Holds if enemy needs to be respawned
    private bool respawn = false;
    public float respawnTime = 5;
    bool once = true;
    private float timer;
    private bool arena1 = false;

    private DoorControllerScript dcs;
    private ParticleSystem ps;
    private GameObject enemy;
    private Transform enemyCloneHolder;
    private PlayerHealth ph;
    private MainUIScript mus;
    GrowActivation ga;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x < 26.45f)
        {
            arena1 = true;
        }

        mus = GameObject.Find("MainUIPanel").GetComponent<MainUIScript>();

        //Try to find clone holder and error if needed
        try
        {
            enemyCloneHolder = GameObject.Find("Enemies").transform;
        }
        catch
        {
            Debug.LogError("Cannot find clone holder for enemies, please add/activate an empty gameObject titled \"Enemies\"");
        }

        timer = respawnTime;

        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        ps = GetComponent<ParticleSystem>();
        ga = GameObject.Find("GrowActivator").GetComponent<GrowActivation>();

        if (arena1)
        {
            dcs = GameObject.Find("Door").GetComponent<DoorControllerScript>();

            mus.setTotalEnemies(10);
            mus.setEnemiesLeft(10);
        }
        else
        {
            dcs = GameObject.Find("Door1").GetComponent<DoorControllerScript>();
        }

        //Start spawn
        spawn();
    }

    private void spawn()
    {
        timer = respawnTime;

        //Summon enemy
        GameObject clone = Instantiate(enemyPrefabs[(int)spawnedEnemy], transform.position, Quaternion.identity, enemyCloneHolder);
        enemy = clone;
        if ((int)spawnedEnemy == 0) //Give flyer patrol points
        {
            clone.GetComponent<NewFlyingEnemy>().moveSpots = patrolPoints;
        } 
        else if ((int)spawnedEnemy == 1) //Give farmer patrol points
        { 
            clone.GetComponent<FarmerEnemy>().moveSpots = patrolPoints; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null && dcs.open)
        {
            Destroy(Instantiate(enemyRemoveParticles, enemy.transform.position, Quaternion.identity, enemyCloneHolder), 1.5f);
            Destroy(enemy);
        }

        if (once && ga.grown && !arena1)
        {
            once = false;
            mus.setTotalEnemies(25);
            mus.setEnemiesLeft(25);
        }

        if (enemy == null && !dcs.open)
        {
            if (timer <= 0)
            {
                spawn();
            }
            else
            {
                timer -= Time.deltaTime;
            }

            if (timer <= 2.5f)
            {
                if (!ps.isPlaying) ps.Play();
            }
        }

        if (PlayerHealth.dying)
        {
            respawn = true;
        }

        if (respawn && Time.timeScale > 0)
        {
            spawn();
            respawn = false;
        }
    }
}
