using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    #region Variables

    //--Editable varibles--
    public float maxHealth = 10; //Holds current max player health

    //--Public varibles--
    [HideInInspector] public static float playerHealth; //Holds current player health
    [HideInInspector] public Transform currentCheckPoint; //Holds respawn point
    [HideInInspector] public static bool dying = false; //Holds if dying

    //--Private references--
    private Rigidbody2D rb2;
    private GrapplingGun gg;
    private GrapplingRope gr;
    private GrappleAttacking ga;
    private Animator a;
    private Volume v1;
    private Volume v2;
    private Vignette vi;
    private Vignette vi2;
    private ColorAdjustments ca;
    private Camera main;
    private CameraMouseFollow cmf;
    private AudioSource aS;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Gets references 
        //bool healing = gameObject.GetComponent<GrappleAttacking>().eating;
        //destroyed = 
        rb2 = GetComponent<Rigidbody2D>();
        gg = transform.GetChild(0).GetComponent<GrapplingGun>();
        gr = transform.GetChild(0).GetChild(0).GetComponent<GrapplingRope>();
        ga = GetComponent<GrappleAttacking>();
        a = GetComponent<Animator>();
        v1 = GameObject.Find("DeathVolume").GetComponent<Volume>();
        v2 = GameObject.Find("DamageVolume").GetComponent<Volume>();
        v1.profile.TryGet(out vi);
        v2.profile.TryGet(out vi2);
        GameObject.Find("DeathVolume").GetComponent<Volume>().profile.TryGet(out ca);
        main = Camera.main;
        cmf = main.GetComponent<CameraMouseFollow>();
        aS = GetComponents<AudioSource>()[2];

        //Sets defualt health
        playerHealth = maxHealth;
        dying = false;
        currentCheckPoint = GameObject.Find("StartPos").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //If player health is less than or equal to 0
        if (playerHealth <= 0 && !dying)
        {
            //Start death and set dying
            death();
            dying = true;
        }

        if (dying)
        {
            //Keep Vignette center on player
            vi.center.Override(new Vector3(0.5f, 0.5f, 0) - new Vector3(main.transform.localPosition.x / 20f, main.transform.localPosition.y / 20f, 0));
        }
    }

    [ContextMenu("startDeath")]
    private void death()
    {
        //Call all full resets
        gg.fullReset();
        gr.fullReset();
        ga.fullReset();

        //Set animator to unscaled time and pause time
        a.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;

        //Start death animaton
        a.SetBool("Dead", true);

        cmf.dying = true;

        //Start to checkpoint coroutine
        StartCoroutine(toCheckpointEffects());
    }

    //Goes to checkpoint with effects
    private IEnumerator toCheckpointEffects()
    {
        //Start screenshake
        ScreenShake.TriggerShake(0.1f);

        //Plays death sound
        aS.Play();

        //Sets priority
        v1.priority = 2;

        float time = 0; //Holds time to wait
        float exposure = 0; //Holds current exposure

        //Wait
        yield return new WaitForSecondsRealtime(0.25f);

        //While exposure is greater than -7 run death effects
        while (exposure > -7)
        {
            //Scale vignette over time
            vi.intensity.Override(time * 1.5f);

            //When vignette is done
            if (time * 1.5f > 1)
            {
                //Increment exposure and apply it to color adjustments
                exposure -= 0.20f;
                ca.postExposure.Override(exposure);
            }

            //Increment time
            time += Time.unscaledDeltaTime * 8;

            //Wait
            yield return new WaitForSecondsRealtime(0.04f);
        }

        //Reset postion and velocity and dying
        rb2.velocity = Vector2.zero;
        transform.position = currentCheckPoint.transform.position;
        cmf.dying = false;

        //Reset health
        playerHealth = maxHealth;

        //Stop death animation and reset time
        a.SetBool("Dead", false);
        Time.timeScale = 1;
        a.updateMode = AnimatorUpdateMode.Normal;


        time = 2; //Set time to 2
        float intensity = 1; //Holds current intensity

        //While time is greater than 0 undo death effects
        while (intensity > 0)
        {
            //Undo color adjustments over time
            ca.postExposure.Override(Mathf.Clamp(time / 2f, -20, 0));

            ////When color adjustments are done
            if (time / 2f < 1f)
            {
                //Increment intensity and apply it to color vignette
                intensity -= 0.20f;
                vi.intensity.Override(intensity);
            }

            //Increment time
            time -= Time.unscaledDeltaTime * 8;

            //Wait
            yield return new WaitForSecondsRealtime(0.04f);
        }

        dying = false;

        //Resets priority
        v1.priority = 0;
    }

    //Handles damage
    public void damage(float amount)
    {
        //Increments health by amount
        playerHealth -= amount;

        //Starts player damage effects
        StartCoroutine(damageEffects());
    }

    public static void heal()
    {
        playerHealth += 3;                           
    }

    private void OnDisable(GameObject enemy)
    {
        if(enemy == null)
        {
            Debug.Log("Works");
            OnDisable(enemy);
        }
        //OnDisable(enemy);
       
        /*if (enemy == null)
        {
            Debug.Log("Works");
            heal();
        }*/ 

        
    }

    private IEnumerator damageEffects()
    {
        //Start screenshake
        ScreenShake.TriggerShake(0.25f);
        


        //Sets priority
        v2.priority = 1;
        
        //Holds current intenesity of the vignette
        float intensity = 0;

        //While intensity is below 0.3f
        while (intensity < 0.3f && !dying)
        {
            intensity += Time.deltaTime * 2; //Increase intensity
            vi2.intensity.Override(intensity); //Set intensity
            yield return new WaitForEndOfFrame(); //Set intensity
        }

        //Set intensity to exactly 0.3f
        vi2.intensity.Override(0.3f);

        while (intensity > 0 && !dying)
        {
            intensity -= Time.deltaTime / 5; //Decrease intensity
            vi2.intensity.Override(intensity); //Set intensity
            yield return new WaitForEndOfFrame(); //Set intensity
        }

        //Set intensity to exactly 0
        vi2.intensity.Override(0);

        //Reset priority
        v2.priority = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            playerHealth = 0;
        }
        else if (collision.tag == "Checkpoint")
        {
            currentCheckPoint = collision.transform;
            currentCheckPoint.GetComponent<Collider2D>().enabled = false;
        }
    }
}
