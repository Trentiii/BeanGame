using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    [Tooltip("Hold reference to grappleRope Script")]
    public GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [Tooltip("If you are able to grapple to all layers")]
    [SerializeField] private bool grappleToAll = false;
    [Tooltip("Holds grapple layer number if needed")]
    [SerializeField] private int grappableLayerNumber = 9;

    //Holds main camera
    [HideInInspector] public Camera m_camera;

    [Header("Transform Ref:")]
    [Tooltip("Put Player here")]
    public Transform gunHolder;
    [Tooltip("Put GunPivot here")]
    public Transform gunPivot;
    [Tooltip("Put FirePoint here")]
    public Transform firePoint;

    [Header("Physics Ref:")]
    [Tooltip("Holds springJoint on player")]
    public SpringJoint2D m_springJoint2D;
    [Tooltip("Holds Rigibody on player")]
    public Rigidbody2D m_rigidbody;

    [Header("Distance:")]
    [Tooltip("If grapple has a max distance value")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

    [Header("Launching:")]
    [Tooltip("Holds speed of grapple")]
    [SerializeField] private float launchSpeed = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        //Gets main camaera reference
        m_camera = Camera.main;

        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
        }
    }

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                }
            }
        }
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;

        m_springJoint2D.connectedAnchor = grapplePoint;

        Vector2 firePointDistanceVector = firePoint.position - gunHolder.position;

        m_springJoint2D.distance = firePointDistanceVector.magnitude;
        m_springJoint2D.frequency = launchSpeed;
        m_springJoint2D.enabled = true;

    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

}

