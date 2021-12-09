using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlyingEnemy : MonoBehaviour
{
    public enum State
    {
        idle,
        attacking,
        patrolling,
        following,
        grappled,
        retreat

    }

    public State currentState = State.idle;
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.idle:
                Idling();

                break;
            case State.patrolling:
                break;
            case State.attacking:
                break;
            case State.following:
                break;
            case State.grappled:
                break;
            case State.retreat:
                break;
            default:
                Debug.Log("State defaulted");
                break;
        }

    }
    //What happens when Idling
    private void Idling()
    {
        //Tell animator to idle
        ani.SetTrigger("Idling");
    }

    //What happens when following player
    private void Following()
    {
        //Play flying towards animation and get within certain distance
        ani.SetTrigger("Following");
    }
    //What happens when attacking
    private void Attacking()
    {
        //Stop and shoot projectiles at player
        //Play attacking animation
        //Instantiate projectiles
        ani.SetBool("Attacking", true);
    }

    //What happens when patrolling
    private void Patrolling()
    {
        //Looks around the area for the player
        //Patroll between points
        ani.SetTrigger("Patrolling");
    }
    //What happens when retreating
    private void Retreat()
    {
        //Runs away when player gets into range
        ani.SetBool("Retreating", true);
    }

    //What happens when grappled
    private void Grappled()
    {
        //stop everything and play grappled animation
    }
}
