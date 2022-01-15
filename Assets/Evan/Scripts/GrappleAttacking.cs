using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GrappleAttacking : MonoBehaviour
{
    #region Variables

    //--Editable varibles--
    public GameObject template;
    [Tooltip("Number of frames to wait during eating")]
    [SerializeField] int eatWaitTime = 1;

    //--Public varibles--
    [HideInInspector] public bool eating;
    [HideInInspector] public GameObject clone;

    //--Private varibles--
    bool pulling = false;
    bool corRunning = false;

    //--Private references--
    GrapplingGun gg;
    AudioSource aS;
    GrapplingRope gr;
    GameObject cloneHolder;

    #endregion
    public bool destroyed;
    private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        //Gets references
        door = GameObject.FindGameObjectWithTag("Door");
        gg = transform.GetChild(0).GetComponent<GrapplingGun>();
        aS = GetComponent<AudioSource>();
        gr = transform.GetChild(0).GetChild(0).GetComponent<GrapplingRope>();
        cloneHolder = transform.GetChild(3).gameObject;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //If collided with enemy (and not already running/ending)
        if (collision.gameObject.layer == 11 && !corRunning && !gr.grappleEnded && !gg.grappling)
        {
            //Starts eating
            startEating(collision.gameObject);
        }
    }

    //Starts the enemy eating
    public void startEating(GameObject enemy)
    {
        
        

        //Randomize pitch and play eating sound
        aS.pitch = Random.Range(0.95f, 1.1f);
        aS.Play();

        if (enemy.name != "Boss")
        {
            //Definitly the intended use of a try catch lol
            try
            {
                //Set flying enemy state
                enemy.GetComponent<NewFlyingEnemy>().currentState = NewFlyingEnemy.State.grappled;
            }
            catch //If flying enemy setting errored
            {
                //Set farmer enemy state
                enemy.GetComponent<FarmerEnemy>().currentState = FarmerEnemy.State.grappled;
            }

            eating = true; //Sets eating to true
            gg.attacking = true; //Set attacking to true
            gg.setAttackPoint(enemy.transform.position); //Starts setAttackPoint
            StartCoroutine(doEating(enemy)); //Start doEating coroutine and passes it enemy
        }
        else
        {
            //Start End scene (Just stops game for now)
            Time.timeScale = 0;
            Debug.Log("Ending scene not made yet :(");
        }
    }

    private void Update()
    {
        //If pulling
        if (pulling)
        {
            //Lerps enemy towards player
            clone.transform.position = new Vector2(Mathf.Lerp(clone.transform.position.x, transform.position.x, Time.deltaTime * 30), Mathf.Lerp(clone.transform.position.y, transform.position.y, Time.deltaTime * 30));
            gg.setAttackPoint(clone.transform.position); //Recalls setAttackPoint for new enemy position
        }
    }

    private IEnumerator doEating(GameObject enemy)
    {
        //Sets corRunning to true
        corRunning = true;

        //Holds if enemy copy has been spawned
        bool spawned = false;
        clone = null; //Resets clone holder

        //Extend tounge to enemy
        while (true)
        {
            //If straightline and not spawned yet
            if (gr.straightLine && !spawned)
            {
                //Sets spawned to true
                spawned = true;

                //Spawns clone and sets sprite
                clone = Instantiate(template, enemy.transform.position, enemy.transform.localRotation, cloneHolder.transform);
                clone.GetComponent<SpriteRenderer>().sprite = enemy.GetComponent<SpriteRenderer>().sprite;

                //Once again definitly the intended use of a try catch lol
                try
                {
                    //Sets up scream sfx clone
                    enemy.GetComponent<NewFlyingEnemy>().cloneSFXSetup();
                }
                catch //If flying enemy setting errored
                {
                    //Sets up scream sfx clone
                    enemy.GetComponent<FarmerEnemy>().cloneSFXSetup();
                }

                //In fi statement so test scenes still work
                if(door != null) door.GetComponent<Door>().EnemyCounter();
                
                //Destroy original enemy
                Destroy(enemy);

                //Heals player
                PlayerHealth.heal();
            }

            //If player is spawned
            if (spawned)
            {
                //Leave loop
                break;
            }

            //Wait
            yield return new WaitForEndOfFrame();
        }

        //Sets pulling to true
        pulling = true;

        //Get sprite renderer of clone
        SpriteRenderer cSR = clone.GetComponent<SpriteRenderer>();

        //While not shrunk
        while (clone.transform.localScale.x > 0.05f)
        {
            //If close enough
            if ((clone.transform.position - transform.position).magnitude < 10.04f)
            {
                //Shrink clone
                clone.transform.localScale -= new Vector3(0.05f, 0.05f, 0);

                //Fade to mouth background
                cSR.color -= new Color(0.06f, 0.12f, 0.06f);
                cSR.color = new Color(Mathf.Clamp(cSR.color.r, 0.53f, 1), Mathf.Clamp(cSR.color.g, 0.26f, 1), Mathf.Clamp(cSR.color.b, 0.435f, 1));

                //Wait for frames = eatWaitTime
                for (int i = 0; i < eatWaitTime; i++)
                {
                    yield return new WaitForEndOfFrame();
                }

            }
            else
            {
                //Wait for one frame
                yield return new WaitForEndOfFrame();
            }
        }

        //Set grapple ended to true
        gr.grappleEnded = true;

        //Turn off pulling
        pulling = false;

        //Destroy clone
        Destroy(clone);

        //Turns off corRunning
        corRunning = false;
    }

    public void fullReset()
    {
        //Reset all varibles
        eating = false;
        clone = null;
        pulling = false;
        corRunning = false;
    }
}
