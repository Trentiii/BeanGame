using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    //--Editable varibles--
    public float maxHealth = 10; //Holds current max player health

    //--Public varibles--
    [HideInInspector] public static float playerHealth; //Holds current player health
    [HideInInspector] public Transform currentCheckPoint; //Holds respawn point

    //--Private varibles--
    private bool dying = false;

    //--Private references--
    private Rigidbody2D rb2;
    private GrapplingGun gg;
    private GrapplingRope gr;
    private GrappleAttacking ga;
    private Animator a;
    private Vignette v;
    private ColorAdjustments ca;
    private Camera main;
    private CameraMouseFollow cmf;

    // Start is called before the first frame update
    void Start()
    {
        //Gets references
        rb2 = GetComponent<Rigidbody2D>();
        gg = transform.GetChild(0).GetComponent<GrapplingGun>();
        gr = transform.GetChild(0).GetChild(0).GetComponent<GrapplingRope>();
        ga = GetComponent<GrappleAttacking>();
        a = GetComponent<Animator>();
        GameObject.Find("DeathVolume").GetComponent<Volume>().profile.TryGet(out v);
        GameObject.Find("DeathVolume").GetComponent<Volume>().profile.TryGet(out ca);
        main = Camera.main;
        cmf = main.GetComponent<CameraMouseFollow>();

        //Sets defualt health
        playerHealth = maxHealth;
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
            v.center.Override(new Vector3(0.5f, 0.5f, 0) - new Vector3(main.transform.localPosition.x / 20f, main.transform.localPosition.y / 20f, 0));
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
        float time = 0; //Holds time to wait
        float exposure = 0; //Holds current exposure

        //Wait
        yield return new WaitForSecondsRealtime(0.25f);

        //While time is less than 2 run death effects
        while (exposure > -7)
        {
            //Scale vignette over time
            v.intensity.Override(time * 1.5f);

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
                v.intensity.Override(intensity);
            }

            //Increment time
            time -= Time.unscaledDeltaTime * 8;

            //Wait
            yield return new WaitForSecondsRealtime(0.04f);
        }

        dying = false;
    }

    //Handles damage
    public static void damage(float amount)
    {
        //Increments health by amount
        playerHealth -= amount;
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
