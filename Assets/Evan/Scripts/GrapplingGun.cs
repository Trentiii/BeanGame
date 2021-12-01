using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    //--Editable varibles--
    [Header("Layers Settings:")]
    [Tooltip("If you are able to grapple to all layers")]
    [SerializeField] private bool grappleToAll = true;
    [Tooltip("Holds grapple layer number if needed")]
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Distance:")]
    [Tooltip("If grapple has a max distance value")]
    [SerializeField] private bool hasMaxDistance = true;
    [Tooltip("Max shoot distance for the grapple")]
    [SerializeField] private float maxDistance = 9;

    [Header("Launching")]
    //[Tooltip("If letting go of right click will cancel the grapple early")]
    //[SerializeField] private bool cancelable = false;
    [Tooltip("Distance from grapple point to be considered done")]
    public float finalDistance = 0.1f;
    [Tooltip("Holds start speed of grapple")]
    [SerializeField] private float startLaunchSpeed = 1f;
    [Tooltip("Holds ending speed of grapple")]
    [SerializeField] private float endLaunchSpeed = 8;
    [Tooltip("Hold max speed for all movement")]
    [SerializeField] private float maxSpeed = 10;

    enum stuckSolvers // your custom enumeration
    {
        clipSolver,
        stopSolver,
        anyTimeStopSolver
    };

    [Header("Stuck Solver")]
    [Tooltip("Holds which method of fixing softlocks to use")]
    [SerializeField] stuckSolvers solverType;


    //--Public varibles--
    [HideInInspector] public Vector2 ropeGrapplePoint; //Holds point for the rope to grapple too
    [HideInInspector] public Vector2 grapplePoint; //Holds point to grapple too
    [HideInInspector] public Vector2 grappleDirection; //Holds vector towards grapple point
    [HideInInspector] public float currentLaunchSpeed; //Holds currentLaunchSpeed
    [HideInInspector] public bool stuckOnWall = false; //Holds if stuck on wall

    //--Private varibles--
    private bool grappling = false;
    private bool noBoost = false;
    private Vector2 grappleNormal; //Holds the normal of the grappled surface

    //--Private references--
    private Camera mCamera; //Holds main camera 
    private Transform gunHolder; //Holds parent
    private Transform grappleFinder; //Holds GrappleFinder
    private GrapplingRope grappleRope; //Holds grappleRope script
    private SpringJoint2D springJoint2D; //Holds springJoint
    private Rigidbody2D rb2; //Holds rigibody2d
    private PlayerMovement pm; //Holds playerMovement script
    private Collider2D c2d;

    private void Start()
    {
        //Gets references
        gunHolder = transform.parent;
        grappleRope = transform.GetChild(0).GetComponent<GrapplingRope>();
        springJoint2D = gunHolder.GetComponent<SpringJoint2D>();
        rb2 = gunHolder.GetComponent<Rigidbody2D>();
        mCamera = Camera.main;
        pm = gunHolder.GetComponent<PlayerMovement>();
        grappleFinder = gunHolder.parent.GetChild(1);
        c2d = gunHolder.GetComponent<Collider2D>();

        //Sets rope, spring joint, and grappleFinder to off by default
        grappleRope.enabled = false;
        springJoint2D.enabled = false;
        grappleFinder.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Clamp velocity to maxSpeed
        rb2.velocity = new Vector2(Mathf.Clamp(rb2.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb2.velocity.y, -maxSpeed, maxSpeed));

        //If right click detected
        if (Input.GetKeyDown(KeyCode.Mouse0) && !grappling && grappleRope.retracted)
        {
            //Flips grappling and retracted
            grappling = true;
            grappleRope.retracted = false;

            //Sets start launch speed
            currentLaunchSpeed = startLaunchSpeed;

            //Start setGrapplePoint
            setGrapplePoint();
        }
        else if (solverType != stuckSolvers.anyTimeStopSolver && grappling && !Input.GetKey(KeyCode.Mouse0) && ((grapplePoint - (Vector2)gunHolder.position).magnitude < finalDistance)) //If right click was let go and grapple is done
        {
            //Sets grapple to ended
            grappleRope.grappleEnded = true;
        }
        else if (solverType == stuckSolvers.anyTimeStopSolver && grappling && !Input.GetKey(KeyCode.Mouse0)) ///If right click was let go and anytime stop is active
        {
            //Sets grapple to ended
            grappleRope.grappleEnded = true;

            if (!grappleRope.isGrappling)
            {
                noBoost = true;
            }
        }
        else if (solverType != stuckSolvers.anyTimeStopSolver && grappling && Input.GetKey(KeyCode.Mouse0) && ((grapplePoint - (Vector2)gunHolder.position).magnitude < finalDistance)) //If right click is held and grapple is done
        {
            //Set noBoost to true
            noBoost = true;
        }

        //If grappling
        if (grappling)
        {
            //Starts speedUp
            speedUp();

            //Sets grappleFinder to end of the grapple
            grappleFinder.transform.position = grapplePoint;

            //Does raycast to grapplePoint
            RaycastHit2D hit = Physics2D.Raycast(transform.position, grapplePoint - (Vector2)transform.position);

            //Checks if path to grappleFinder is blocked
            if (hit.transform != null && springJoint2D.enabled && hit.transform.name != "GrappleFinder" && (grapplePoint - (Vector2)transform.position).magnitude > 1.05f && hit.distance < 1.05f)
            {
                //Sets stuckOnWall to truw
                stuckOnWall = true;

                if (solverType == stuckSolvers.clipSolver) //If using clipsolver
                {
                    //Turn off collider
                    c2d.enabled = false;
                }
                else if (solverType == stuckSolvers.stopSolver) //If using stopsolver
                {
                    //Set grapple to ended
                    grappleRope.grappleEnded = true;
                }
            }
            else if(solverType == stuckSolvers.clipSolver) //If using clipsolver
            {
                //Turn on collider
                c2d.enabled = true;
            }
        }
    }


    //Gets point to grapple too
    void setGrapplePoint()
    {
        //Finds distance from lanch point to mouse
        Vector2 distanceVector = mCamera.ScreenToWorldPoint(Input.mousePosition) - gunHolder.position;

        //If raycast from grapple stop towards grapple end hits something
        if (Physics2D.Raycast(transform.position, distanceVector.normalized))
        {
            //Get that hit
            RaycastHit2D hit = Physics2D.Raycast(transform.position, distanceVector.normalized);

            //If that hit is on a grapplable layer and if distance is less than max
            if ((hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll) && (Vector2.Distance(hit.point, transform.position) <= maxDistance || !hasMaxDistance))
            {
                if (hit.transform.gameObject.layer == 11)
                {
                    Debug.Log("hi");
                }

                grappleNormal = hit.normal; //Gets grapple normal
                ropeGrapplePoint = hit.point; //Gets grapple point for rope to go to
                grapplePoint = hit.point + (grappleNormal * 0.4f); //Set grapple point to raycast hit point + normal + x offset
                grappleDirection = grapplePoint - (Vector2)gunHolder.position; //Get grapple distance vector
                grappleRope.enabled = true; //Starts grappleRope script

                //Turns on grappleFinder
                grappleFinder.gameObject.SetActive(true);
            }
            else
            {
                //Starts grappleFailed and passes it distanceVector
                grappleFailed(distanceVector);
            }
        }
        else
        {
            //Starts grappleFailed and passes it distanceVector
            grappleFailed(distanceVector);
        }
    }

    //If grapple failed to set grapplePoint
    private void grappleFailed(Vector2 currentDistanceVector)
    {
        ropeGrapplePoint = (Vector2)gunHolder.position + (currentDistanceVector.normalized * maxDistance); //Gets grapple point for rope to go to
        grappleDirection = ropeGrapplePoint - (Vector2)gunHolder.position; //Get grapple distance vector

        grappleRope.enabled = true; //Starts grappleRope script

        //Sets grapple failed to true
        grappleRope.grappleFailed = true;
    }

    //Does grapple movement
    public void grapple()
    {
        springJoint2D.connectedAnchor = grapplePoint; //Sets spring joint start to end of grapple
        springJoint2D.enabled = true; //Turns on spring joint
    }

    //Speeds up grapple based on current positon
    void speedUp()
    {
        //Gets current distance between the player and the grapplePoint
        Vector2 currentDistance = grapplePoint - (Vector2)gunHolder.position;

        //Gets that precange of the amount moved so far
        float movementPrecentage = Mathf.Abs(currentDistance.magnitude - grappleDirection.magnitude) / grappleDirection.magnitude;

        //Sets current launch speed to starting launch speed times the movement precentage 
        currentLaunchSpeed = startLaunchSpeed + (endLaunchSpeed * movementPrecentage);

        //If movement is done (only goes up to about 0.75 for some reason)
        if (movementPrecentage > 0.7)
        {
            springJoint2D.frequency = 0; //Turns off pulling
        }
        else
        {
            springJoint2D.frequency = currentLaunchSpeed; //Sets spring joint pull speed to launchspeed
        }
    }

    //Resets everything for next grapple
    public void resetGrapple()
    { 
        //Turn rope and springjoint back off
        grappleRope.enabled = false;
        springJoint2D.enabled = false;

        //If not grapple failed and not noBoost
        if (!grappleRope.grappleFailed && !noBoost)
        {
            //Add boost
            rb2.velocity = new Vector2(grappleDirection.normalized.x * grappleDirection.magnitude, rb2.velocity.y);
        }
        else
        {
            //Turn off grapple failed
            grappleRope.grappleFailed = false;
        }

        //Resets varibles
        noBoost = false;
        currentLaunchSpeed = startLaunchSpeed;
        grappling = false;
        grappleRope.grappleFailed = false;
        stuckOnWall = false;


        //Turns on collision
        c2d.enabled = true;

        //Turns on player movement
        pm.enabled = true;

        //Turns off grappleFinder
        grappleFinder.gameObject.SetActive(false);
    }

    //Draws max distance circle
    private void OnDrawGizmosSelected()
    {
        //If there is a maxdistance
        if (hasMaxDistance)
        {
            //Draw a circle the size of maxdistance
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}

