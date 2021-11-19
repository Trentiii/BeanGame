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
    [SerializeField] private float maxDistnace = 15;

    [Header("Launching:")]
    [Tooltip("Holds speed of grapple")]
    [SerializeField] private float launchSpeed = 2.54f;

    //--Private varibles--
    [HideInInspector] public Vector2 grapplePoint; //Holds point to grapple too
    [HideInInspector] public Vector2 grappleDirection; //Holds vector towards grapple point

    //--Private references--
    private Camera mCamera; //Holds main camera 
    private Transform gunHolder; //Holds parent
    private GrapplingRope grappleRope; //Holds grappleRope script
    private SpringJoint2D springJoint2D; //Holds springJoint

    private void Start()
    {
        //Gets references
        gunHolder = transform.parent;
        grappleRope = transform.GetChild(0).GetComponent<GrapplingRope>();
        springJoint2D = gunHolder.GetComponent<SpringJoint2D>();
        mCamera = Camera.main;

        //Sets rope and spring joint to off by default
        grappleRope.enabled = false;
        springJoint2D.enabled = false;
    }

    private void Update()
    {
        //If right click detected
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Start setGrapplePoint
            setGrapplePoint();
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
                if (Vector2.Distance(_hit.point, transform.position) <= maxDistnace || !hasMaxDistance)
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
        springJoint2D.frequency = launchSpeed; //Sets spring joint pull speed to lanchspeed
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
            Gizmos.DrawWireSphere(transform.position, maxDistnace);
        }
    }

}

