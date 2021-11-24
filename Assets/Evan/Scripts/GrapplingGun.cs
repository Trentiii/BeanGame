using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    //--Editable varibles--
    [Header("Layers Settings:")]
    [Tooltip("If you are able to grapple to all layers")]
    [SerializeField] private bool grappleToAll = false;
    [Tooltip("Holds grapple layer number if needed")]
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Distance:")]
    [Tooltip("If grapple has a max distance value")]
    [SerializeField] private bool hasMaxDistance = false;
    [Tooltip("Max shoot distance for the grapple")]
    [SerializeField] private float maxDistance = 15;

    [Header("Launching")]
    [Tooltip("If letting go of right click will cancel the grapple early")]
    [SerializeField] private bool cancelable = false;
    [Tooltip("Distance from grapple point to be considered done")]
    [SerializeField] private float finalDistance = 0.8f;
    [Tooltip("Holds start speed of grapple")]
    [SerializeField] private float startLaunchSpeed = 2.54f;
    [Tooltip("Holds ending speed of grapple")]
    [SerializeField] private float endLaunchSpeed = 7;
    [Tooltip("Hold max speed for all movement")]
    [SerializeField] private float maxSpeed = 10;

    //--Public varibles--
    [HideInInspector] public Vector2 ropeGrapplePoint; //Holds point for the rope to grapple too
    [HideInInspector] public Vector2 grappleDirection; //Holds vector towards grapple point
    [HideInInspector] public float currentLaunchSpeed; //Holds currentLaunchSpeed

    //--Private varibles--
    private bool grappling = false;
    private Vector2 grapplePoint; //Holds point to grapple too
    private Vector2 grappleNormal; //Holds the normal of the grappled surface

    //--Private references--
    private Camera mCamera; //Holds main camera 
    private Transform gunHolder; //Holds parent
    private GrapplingRope grappleRope; //Holds grappleRope script
    private SpringJoint2D springJoint2D; //Holds springJoint
    private Rigidbody2D rb2; //Holds rigibody2d
    private PlayerMovement pm; //Holds playerMovement script

    private void Start()
    {
        //Gets references
        gunHolder = transform.parent;
        grappleRope = transform.GetChild(0).GetComponent<GrapplingRope>();
        springJoint2D = gunHolder.GetComponent<SpringJoint2D>();
        rb2 = gunHolder.GetComponent<Rigidbody2D>();
        mCamera = Camera.main;
        pm = gunHolder.GetComponent<PlayerMovement>();

        //Sets rope and spring joint to off by default
        grappleRope.enabled = false;
        springJoint2D.enabled = false;
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

            //Turns off player movement
            pm.enabled = false;
        }
        else if (cancelable && Input.GetKeyUp(KeyCode.Mouse0)) //If right click was let go
        {
            //Sets grapple to ended
            grappleRope.grappleEnded = true;
        }
        else if (!cancelable && grappling && !Input.GetKey(KeyCode.Mouse0) && ((grapplePoint - (Vector2)gunHolder.position).magnitude < finalDistance)) //If right click was let go and grapple is done
        {
            //Sets grapple to ended
            grappleRope.grappleEnded = true;
        }

        //If grappling
        if (grappling)
        {
            //Starts speedUp
            speedUp();
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
                    grappleNormal = hit.normal; //Gets grapple normal
                    ropeGrapplePoint = hit.point; //Gets grapple point for rope to go to
                    grapplePoint = hit.point + (grappleNormal * 0.4f); //Set grapple point to raycast hit point + normal + x offset
                    grappleDirection = grapplePoint - (Vector2)gunHolder.position; //Get grapple distance vector
                    grappleRope.enabled = true; //Starts grappleRope script
            }
            else
            {
                grappleNormal = hit.normal; //Gets grapple normal
                ropeGrapplePoint = hit.point; //Gets grapple point for rope to go to
                grapplePoint = hit.point + (grappleNormal * 0.4f); //Set grapple point to raycast hit point + normal + x offset
                grappleDirection = grapplePoint - (Vector2)gunHolder.position; //Get grapple distance vector
                grappleRope.enabled = true; //Starts grappleRope script

                //Sets grapple failed to true
                grappleRope.grappleFailed = true;
            }
        }
        else
        {
            ropeGrapplePoint = distanceVector.normalized * maxDistance; //Gets grapple point for rope to go to
            grappleDirection = ropeGrapplePoint - (Vector2)gunHolder.position; //Get grapple distance vector

            grappleRope.enabled = true; //Starts grappleRope script

            //Sets grapple failed to true
            grappleRope.grappleFailed = true;
        }
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

        //If not grapple failed
        if (!grappleRope.grappleFailed)
        {
            //Add reflect movement
            rb2.velocity = new Vector2(Vector2.Reflect(grappleDirection, grappleNormal).x, rb2.velocity.y);
        }
        else
        {
            //Tunr off grapple failed
            grappleRope.grappleFailed = false;
        }

        //Resets currentLaunchSpeed
        currentLaunchSpeed = startLaunchSpeed;

        //Flips grappling and grappling failed
        grappling = false;
        grappleRope.grappleFailed = false;

        //Turns on player movement
        pm.enabled = true;
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

