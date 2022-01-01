﻿using System.Collections;
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

        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        //Start spawn
        spawn();
    }

    private void spawn()
    {
        //Summon enemy
        GameObject clone = Instantiate(enemyPrefabs[(int)spawnedEnemy], transform.position, Quaternion.identity, enemyCloneHolder);
        if (spawnedEnemy == 0) clone.GetComponent<NewFlyingEnemy>().moveSpots = patrolPoints; //Give it patrol points
    }

    // Update is called once per frame
    void Update()
    {
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