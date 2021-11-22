using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [Header("General Settings:")]
    [Tooltip("The number of points the line renderer uses to draw the wave")]
    public int precision = 40;
    [Tooltip("The speed that the line's wave straightens out")]
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 12;

    [Header("Rope Animation Settings:")]
    [Tooltip("Holds the shape of the wave animation")]
    public AnimationCurve ropeAnimationCurve;
    [Tooltip("Holds how large the wave animation is before it starts shrinking")]
    [Range(0.01f, 10)] [SerializeField] private float StartWaveSize = 1;

    [Header("Rope Progression:")]
   // [Tooltip("Holds how r")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 7;

    //--Private varibles--
    private float waveSize = 0; //Holds current wave size
    private float moveTime = 0; //Holds the time spent moving during the grapple
    private bool isGrappling = true; //Holds if the player is currently grappling
    private bool straightLine = true; //Holds if the line is currently straight

    //--Private references--
    private GrapplingGun grapplingGun; //Holds grapplingGunScript
    private LineRenderer lineRenderer; //Holds line renderer
    private Transform gunHolder; //Holds parent

    private void OnEnable()
    {
        //Gets references if not already gotten
        if (grapplingGun == null)
        {
            grapplingGun = transform.parent.GetComponent<GrapplingGun>();
            lineRenderer = GetComponent<LineRenderer>();
            gunHolder = transform.parent.transform.parent;
        }

        moveTime = 0; //Resets move time
        lineRenderer.positionCount = precision; //Sets number of line renderer point to precision
        waveSize = StartWaveSize; //Sets starting wave size
        straightLine = false; //Resets straightLine

        //Start LinePointsToFirePoint
        linePointsToFirePoint();

        //Turns on line renderer
        lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false; //Turns off line renderer
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
        //Adds time to movetime
        moveTime += Time.deltaTime;

        //Starts drawRope
        drawRope();
    }

    //Updates rope points
    void drawRope()
    {
        //If line is not needed to be straight yet
        if (!straightLine)
        {
            //If last point it at grapplePoint
            if (lineRenderer.GetPosition(precision - 1).x == grapplingGun.grapplePoint.x)
            {
                //Set straightLine to true
                straightLine = true;
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
            if (waveSize > 0) //If wavesize is greater than 0
            {
                //Shrink wave size by time times shrink speed
                waveSize -= Time.deltaTime * straightenLineSpeed;

                //Start drawRopeWaves
                drawRopeWaves();
            }
            else //If grappling and wave size is <= 0
            {
                //Set waveSize to exactly 0
                waveSize = 0;

                //Set the rope position count to 2 if not already
                if (lineRenderer.positionCount != 2)
                {
                    lineRenderer.positionCount = 2;
                }

                //Start drawRopeNoWaves
                drawRopeNoWaves();
            }
        }
    }

    //Draws rope with waves
    void drawRopeWaves()
    {
        //For all line renderer points
        for (int i = 0; i < precision; i++)
        {
            float delta = (float)i / ((float)precision - 1f);  //Converts current position point to a precentage amount (i/max)

            //Gets current point offset from being straight (change perpendicular to the line)
            Vector2 offset = Vector2.Perpendicular(grapplingGun.grappleDirection).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;

            //Gets target position that is delta precent along the line between the player and the grapple point (with offset added to make wave)
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.transform.position, grapplingGun.grapplePoint, delta) + offset; 

            //Pushes postition back towards player by the rope progression curve and progression speed
            Vector2 currentPosition = Vector2.Lerp(grapplingGun.transform.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            //Sets position in the line renderer
            lineRenderer.SetPosition(i, currentPosition);
        }
    }

    //Draws the rope with no waves
    void drawRopeNoWaves()
    {
        lineRenderer.SetPosition(0, grapplingGun.transform.position); //Sets rope position 0 to the player
        lineRenderer.SetPosition(1, grapplingGun.grapplePoint); //Sets rope position 1 to the grapplePoint
    }
}
