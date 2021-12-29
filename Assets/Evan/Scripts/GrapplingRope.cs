using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingRope : MonoBehaviour
{
    #region Variables

    //--Editable varibles--
    [Header("General Settings:")]
    [Tooltip("The number of points the line renderer uses to draw the wave")]
    public int precision = 80;
    [Tooltip("The speed that the line's wave straightens out")]
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 12;

    [Header("Rope Animation Settings:")]
    [Tooltip("Holds the shape of the wave animation")]
    public AnimationCurve ropeAnimationCurve;
    [Tooltip("Holds how large the wave animation is before it starts shrinking")]
    [Range(0.01f, 10)] [SerializeField] private float startWaveSize = 1;
    [Tooltip("Holds what the max size for the wave will be")]
    [Range(0.01f, 10)] [SerializeField] private float maxWaveSize = 9;

    [Header("Rope Progression:")]
    [Tooltip("Holds how fast the rope visually moves in a curve")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(0.05f, 5)] private float ropeProgressionSpeed = 0.2f;

    //--Public varibles
    [HideInInspector] public bool retracted = true; //Holds if rope is fully retracted
    [HideInInspector] public bool grappleEnded = false; //Holds if grapple has ended
    [HideInInspector] public bool grappleFailed = false; //Holds if grapple has failed
    [HideInInspector] public bool isGrappling = true; //Holds if the player is currently grappling
    [HideInInspector] public bool straightLine = true; //Holds if the line is currently straight

    //--Private varibles--
    private float waveSize = 0; //Holds current wave size
    private float moveTime = 0; //Holds the time spent moving during the grapple
    private AnimationCurve currentRopeCurve = new AnimationCurve(); //Holds rope curve for this grapple

    //--Private references--
    private GrapplingGun grapplingGun; //Holds grapplingGunScript
    private LineRenderer lineRenderer; //Holds line renderer
    private Transform gunHolder; //Holds top parent
    private PlayerMovement pm; //Holds player movement script
    private GrappleAttacking ga; //Holds player attacking script

    private LineRenderer lineRenderer2;

    #endregion

    private void OnEnable()
    {
        //Gets references if not already gotten
        if (grapplingGun == null)
        {
            grapplingGun = transform.parent.GetComponent<GrapplingGun>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer2 = transform.GetChild(0).GetComponent<LineRenderer>();
            gunHolder = transform.parent.parent;
            pm = gunHolder.GetComponent<PlayerMovement>();
            ga = gunHolder.GetComponent<GrappleAttacking>();
        }

        moveTime = 0; //Resets move time
        lineRenderer.positionCount = precision; //Sets number of line renderer point to precision
        lineRenderer2.positionCount = precision / 6; //Sets reduced precision
        waveSize = startWaveSize; //Sets starting wave size
        straightLine = false; //Resets straightLine

        //Sets currentRopeCurve to ropeAnimationCurve
        currentRopeCurve.keys = ropeAnimationCurve.keys;

        //----- Random curve start------
        //Sets flipper to either -1 or 1
        float flipper;
        if (Random.Range(-1, 1) == 0)
        {
            flipper = 1;
        }
        else
        {
            flipper = -1;
        }

        //Gets a random float
        float globalRandom = Random.Range(0.75f, 1.75f);

        //Get all the animation curve key frames
        Keyframe[] keys = currentRopeCurve.keys;


        //Goes through all the keyframes but the last few
        for (int i = 0; i <= currentRopeCurve.length - (1 + (int)(currentRopeCurve.length / 5)); i++)
        {
            Keyframe keyframe = keys[i]; //Get keyframe i
            keyframe.value *= (flipper * globalRandom * Random.Range(0.9f, 1.1f)); //Scale it based on flipper, globalRandom, and a new random
            keyframe.value = Mathf.Clamp(keyframe.value, -0.35f, 0.35f); //Clamp this new value
            keys[i] = keyframe; //Set key i to this edited keyframe
        }

        //Set currentRopeCurve to this new list of keys
        currentRopeCurve.keys = keys;
        //------ Random curve end-------

        //Start LinePointsToFirePoint
        linePointsToFirePoint();

        //Turns on line renderers
        lineRenderer.enabled = true;
        lineRenderer2.enabled = true;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false; //Turns off line renderer
        lineRenderer2.enabled = false;
        isGrappling = false; //Resets is grappling
    }

    //Resets all line renderer points
    private void linePointsToFirePoint()
    {
        //Goes through all points
        for (int i = 0; i < precision; i++)
        {
            //Set to defualt position
            lineRenderer.SetPosition(i, grapplingGun.transform.position);
        }
    }

    private void Update()
    {
        //If the grapple has not ended and not stuck
        if ((!grappleEnded && !grapplingGun.stuckOnWall))
        {
            //If not at the end of the grapple
            if(!((grapplingGun.grapplePoint - (Vector2)gunHolder.position).magnitude < grapplingGun.finalDistance))
            {
                //Adds time to movetime
                moveTime += Time.deltaTime;
            }
        }
        else if(grappleEnded) //If grapple has ended
        {   //Adds time to movetime
            moveTime -= Time.deltaTime;
        }

        //If straightLine is on, grapple has not ended and not eating
        if (straightLine && !grappleEnded && !ga.eating)
        {
            //Turn of playerMovement
            pm.enabled = false;
        }

        //Starts drawRope
        drawRope();
    }

    //Updates rope points
    private void drawRope()
    {
        //If grapple is not ended
        if (!grappleEnded)
        {
            //If line is not needed to be straight yet
            if (!straightLine)
            {
                //If last point it at grapplePoint
                if (lineRenderer.GetPosition(precision - 1).x == grapplingGun.ropeGrapplePoint.x)
                {
                    if (grappleFailed)
                    {
                        //Set grapple to ended
                        grappleEnded = true;
                    }
                    else
                    {
                        //Set straightLine to true
                        straightLine = true;
                    }
                }
                else
                {
                    //Start drawRopeWaves
                    drawRopeWaves();
                }
            }
            else //If line needs to be straight
            {
                //If not already graplling
                if (!isGrappling)
                {
                    //Start grapple
                    grapplingGun.grapple();

                    //Set isGrappling to true
                    isGrappling = true;
                }

                //Start drawRopeWaves
                drawRopeWaves();

                if (waveSize > 0) //If wavesize is greater than 0
                {
                    //Shrink wave size by time times shrink speed
                    waveSize -= Time.deltaTime * straightenLineSpeed;
                }
                else //If grappling and wave size is <= 0
                {
                    //Set waveSize to exactly 0
                    waveSize = 0;
                }
            }
        }
        else
        {
            //Start retractRope
            retractRope();
        }
    }

    //Draws rope with waves
    private void drawRopeWaves()
    {
        //For all line renderer points
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float delta = (float)i / ((float)precision - 1f);  //Converts current position point to a precentage amount (i/max)

            //Gets current point offset from being straight (change perpendicular to the line)
            Vector2 offset = Vector2.Perpendicular(grapplingGun.grappleDirection).normalized * currentRopeCurve.Evaluate(delta) * Mathf.Clamp(waveSize * (grapplingGun.grappleDirection.magnitude/ 5), 0, maxWaveSize);

            //Gets target position that is delta precent along the line between the player and the grapple point (with offset added to make wave)
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.transform.position, grapplingGun.ropeGrapplePoint, delta) + offset;

            //Pushes postition back towards player by the rope progression curve / ropeProgressionSpeed
            Vector2 currentPosition = Vector2.Lerp(grapplingGun.transform.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime / ropeProgressionSpeed));

            //Sets position in the line renderers
            lineRenderer.SetPosition(i, currentPosition);

            //If within smaller precision
            if(i < precision / 6)
            lineRenderer2.SetPosition(i, currentPosition);
        }
    }

    //Retracks the rope
    private void retractRope()
    {
        //Starts drawRope waves
        drawRopeWaves();

        //Check if points are back at player
        if (((Vector2)lineRenderer.GetPosition(lineRenderer.positionCount - 1) - (Vector2)gunHolder.position).magnitude < 0.5)
        {
            //Reset retracted and grapple ended
            retracted = true;
            grappleEnded = false;

            //Start resetGrapple
            grapplingGun.resetGrapple();
        }
    }

    public void fullReset()
    {
        //Resets all varibles
        moveTime = 0;
        lineRenderer.positionCount = precision;
        lineRenderer2.positionCount = precision / 6;
        waveSize = startWaveSize; 
        straightLine = false;
        retracted = true;
        grappleEnded = false;
        grappleFailed = false;
        lineRenderer.enabled = false; 
        lineRenderer2.enabled = false;
        isGrappling = false;
    }
}
