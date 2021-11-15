using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField]
    private float maxGrappleDist = 10;
    [SerializeField]
    private float launchSpeed = 5;

    //Reference to main Camera
    Camera cam;

    //holds mouse position
    Vector2 mousePos;

    //Holds point to grapple too
    Vector2 grapplePoint;
    //Holds the current look direction and sets the starting vector for it
    Vector2 currentLookDir = new Vector2(1, 0);
    //Holds Grapple direction 
    Vector2 grappleDistanceVector;

    //Component References
    Rigidbody2D rb2;
    PlayerMovement pm;
    public SpringJoint2D sj2D;

    private void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        sj2D = GetComponent<SpringJoint2D>();

        cam = Camera.main;

        sj2D.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Sets mousePos to current mouse position
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            setGrapplePoint();
        }
        else if (Input.GetMouseButton(0))
        {
            doGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            sj2D.enabled = false;
            grapplePoint = Vector2.zero;
        }
    }

    //Physics code, fixed update is called before physics updates
    void FixedUpdate()
    {

    }

    void doGrapple()
    {
        if (grapplePoint != Vector2.zero)
        {
            sj2D.connectedAnchor = grapplePoint; ;

            sj2D.distance = 0.1f;
            sj2D.frequency = launchSpeed;
            sj2D.enabled = true;
        }
    }

    void setGrapplePoint()
    {
        currentLookDir = mousePos - (Vector2)transform.position;

        if (Physics2D.Raycast(transform.position, currentLookDir.normalized))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, currentLookDir.normalized);
            if (Vector2.Distance(hit.point, transform.position) <= maxGrappleDist)
            {
                grapplePoint = hit.point;
                grappleDistanceVector = grapplePoint - (Vector2)transform.position;
            }

            grapplePoint = Physics2D.Raycast(transform.position, currentLookDir.normalized).point;
        }
    }
}
