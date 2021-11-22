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
    [SerializeField] private float maxDistance = 15;

    [Header("Launching:")]
    [Tooltip("Holds start speed of grapple")]
    public float startLaunchSpeed = 2.54f;
    [Tooltip("Holds ending speed of grapple")]
    public float endLaunchSpeed = 7;
    [SerializeField] private float maxSpeed = 10;

    //--Public varibles--
    [HideInInspector] public Vector2 grapplePoint; //Holds point to grapple too
    [HideInInspector] public Vector2 grappleDirection; //Holds vector towards grapple point
    [HideInInspector] public float currentLaunchSpeed; //Holds currentLanchSpeed

    //--Private references--
    private Camera mCamera; //Holds main camera 
    private Transform gunHolder; //Holds parent
    private GrapplingRope grappleRope; //Holds grappleRope script
    private SpringJoint2D springJoint2D; //Holds springJoint
    private Rigidbody2D rb2; //Holds rigibody2d

    private void Start()
    {
        //Gets references
        gunHolder = transform.parent;
        grappleRope = transform.GetChild(0).GetComponent<GrapplingRope>();
        springJoint2D = gunHolder.GetComponent<SpringJoint2D>();
        rb2 = gunHolder.GetComponent<Rigidbody2D>();
        mCamera = Camera.main;

        //Sets rope and spring joint to off by default
        grappleRope.enabled = false;
        springJoint2D.enabled = false;
    }

    private void Update()
    {
        //Clamp velocity to maxSpeed
        rb2.velocity = new Vector2(Mathf.Clamp(rb2.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb2.velocity.y, -maxSpeed, maxSpeed));

        //If right click detected
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Sets start launch speed
            currentLaunchSpeed = startLaunchSpeed;

            //Start setGrapplePoint
            setGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0)) //If right click is currently held down
        {
            //Starts speedUp
            speedUp();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) //If right click was let go
        {
            //Turn rope and springjoint back off
            grappleRope.enabled = false;
            springJoint2D.enabled = false;
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
            RaycastHit2D _hit = Physics2D.Raycast(transform.position, distanceVector.normalized);

            //If that hit is on a grapplable layer
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                //If distance is less than max
                if (Vector2.Distance(_hit.point, transform.position) <= maxDistance || !hasMaxDistance)
                {
                    grapplePoint = _hit.point; //Set grapple point to raycast hit point 
                    grappleDirection = grapplePoint - (Vector2)gunHolder.position; //Get grapple distance vector
                    grappleRope.enabled = true; //Starts grappleRope script
                }
            }
        }
    }

    //Does grapple movement
    public void grapple()
    {
        springJoint2D.connectedAnchor = grapplePoint; //Sets spring joint start to end of grapple
        springJoint2D.enabled = true; //Turns on spring joint
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
}

