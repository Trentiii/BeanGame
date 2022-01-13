using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : MonoBehaviour
{



    public enum State
    {
        idle,
        patrol,
        run,
    }
    public State currentState = State.patrol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Run()
    {

    }

    private void OnDrawGizmosSelected()
    {
        /*/Radius for sight
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
         if (player != null) Gizmos.DrawLine(transform.position, player.position);

     }*/
    }
}
