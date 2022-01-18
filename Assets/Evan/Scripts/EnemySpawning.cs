using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [Tooltip("Holds one of each enemy prefab (In order of enemy selector below)")]
    [SerializeField] private GameObject[] enemyPrefabs;

    //Holds types of enemies
    public enum Enemy
    {
        flyer,
        farmer
    }

    [Tooltip("Holds type of enemy this script will spawn")]
    [SerializeField] private Enemy spawnedEnemy;
    [Tooltip("Holds patrol points to give enemy when spawned")]
    [SerializeField] private Transform[] patrolPoints;

    //Holds if enemy needs to be respawned
    private bool respawn = false;
    public float respawnTime = 5;

    private float timer;

    private ParticleSystem ps;
    private GameObject enemy;
    private Transform enemyCloneHolder;
    private PlayerHealth ph;

    // Start is called before the first frame update
    void Start()
    {
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
        if (enemy == null)
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
