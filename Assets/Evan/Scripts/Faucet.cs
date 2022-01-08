using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages fluids
public class Faucet : MonoBehaviour
{
    bool running = true; //If faucet is running
    bool once = true; //Makes sure loading the next scene is only called once
    bool once2 = true; //Makes sure starting removing the liquid is only called once
    bool once3 = true; //Makes sure everything is turned on only once
    bool started = false; //Holds if transition has started
    int sceneIndex; //Holds scene to load
    float speed; //Holds the speed of CameraMouseFollow
    private GameObject floor; //Gameobject that is bottem of bucket

    public GameObject particle; //Holds particle to spawn
    public Transitioner Transitioner; //Holds Transition in scene

    //Private refrences
    private PlayerMovement pm;
    private GrapplingGun gg;
    private CameraMouseFollow cMF;

    // Update is called once per frame

    private void Start()
    {
        floor = transform.parent.GetChild(0).gameObject;

        //Turn off floor
        floor.SetActive(false);
    }

    public void transitionToIndex(int index)
    {
        //Set started to true
        started = true;
        sceneIndex = index;
    }

    public void transitionToMainMenu()
    {
        //Set started to true
        started = true;
        sceneIndex = 0;
    }

    public void transitionToCredits()
    {
        //Set started to true
        started = true;
        sceneIndex = 2;
    }

    public void transitionToLevel()
    {
        //Set started to true
        started = true;
        sceneIndex = 1;
    }


    void Update()
    {
        //If started run spawner
        if (started)
        {
            if (once3)
            {
                //flip once3
                once3 = false;

                //Turn everything on
                floor.SetActive(true);
                transform.parent.GetChild(1).gameObject.SetActive(true);
                transform.parent.GetChild(2).gameObject.SetActive(true);
                transform.parent.GetChild(3).gameObject.SetActive(true);
                transform.parent.GetChild(5).gameObject.SetActive(true);

                GetComponent<BoxCollider2D>().enabled = true;
            }

            spawner();
        }
    }

    private void spawner()
    {
        //If name has been set to transitionEnder and once2 is still true
        if (transform.parent.name == "transitionEnder" && once2)
        {
            //Flip once2
            once2 = false;

            //Find player and stop movement
            GameObject player = GameObject.Find("Player");

            try
            {
                pm = player.GetComponent<PlayerMovement>();
            }
            catch
            {
                //Nothing lol
            }

            if (pm != null)
            {
                pm = player.GetComponent<PlayerMovement>();
                pm.stopped = true;

                //Stop grappling
                gg = player.transform.GetChild(0).GetComponent<GrapplingGun>();
                gg.stopped = true;
            }

            //Stop camera follow and save value
            GameObject camera = Camera.main.gameObject;

            try
            {
                cMF = camera.GetComponent<CameraMouseFollow>();
            }
            catch
            {
                //Nothing lol
            }

            if (cMF != null)
            {
                speed = cMF.speed;
                cMF.speed = 0;
            }

            //Move parents to camera
            transform.parent.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.parent.position.z);
            Destroy(floor); //Destroy floor

            transform.parent.GetChild(6).gameObject.SetActive(false);

            //Call resetPlayer with delay
            Invoke("resetPlayer", 2);
        }

        //If faucet is running
        if (running)
        {
            //Spawn 5 particles at slightly different positions
            GameObject Clone = Instantiate(particle, transform);
            Clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

            Clone = Instantiate(particle, transform);
            Clone.transform.position += new Vector3(-0.25f, 0, 0);
            Clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));


            Clone = Instantiate(particle, transform);
            Clone.transform.position += new Vector3(0.25f, 0, 0);
            Clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

            Clone = Instantiate(particle, transform);
            Clone.transform.position += new Vector3(-0.5f, 0, 0);
            Clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));


            Clone = Instantiate(particle, transform);
            Clone.transform.position += new Vector3(0.5f, 0, 0);
            Clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));

            Clone = Instantiate(particle, transform);
            Clone.transform.position += new Vector3(-0.5f, 0, 0);
            Clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));


            Clone = Instantiate(particle, transform);
            Clone.transform.position += new Vector3(0.5f, 0, 0);
            Clone.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        }
        else if (once) //If not running and once is still true
        {
            //Flip once and start loading
            once = false;
            Transitioner.startloadScene(sceneIndex);
        }
    }

    //Resets player
    private void resetPlayer()
    {
        //Restart everything thayt was turned off
        if (pm != null)
        {
            pm.stopped = false;
            gg.stopped = false;
        }
        if(cMF != null) cMF.speed = speed;


        //Start destroyer in 2 second
        Invoke("destroyer", 2);
    }

    private void destroyer()
    {
        //Destroy all of the liquid simulation 
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If triggers are hit turn of fauset
        running = false;
    }
}
