using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    public Vector3 cursorPoint;

    [Header("Ray Settings")]
    [SerializeField] private float rayDistance = 1.0f;
    [SerializeField] private float rayWidth = 0.01f;
    [SerializeField] private LayerMask layersToInclude;
    [SerializeField] private Color rayColorDefaultState = Color.yellow;
    [SerializeField] private Color rayColorHoverState = Color.red;
    [SerializeField] private GameObject lookAtObject;
    [SerializeField] private GameObject lookAtPlaneObject;

    public bool showLine = true;
    private Color colorInvisible = new Color(0, 0, 0, 0);
    private Plane lookAtPlane;

    private LineRenderer _lineRenderer;

    private EyeInteractable _noteHovered;
    private ChangeSceneOnOver _sceneButtonHovered;
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        lookAtPlane = new Plane(-lookAtPlaneObject.transform.forward,lookAtPlaneObject.transform.position);
        SetupRay();
    }

    void FixedUpdate()
    {
        //show lookAtObject

        if (Application.isEditor)
        {
            Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitMouse;

            // Casts the ray and get the first game object hit
            if (Physics.Raycast(MouseRay, out hitMouse, Mathf.Infinity, layersToInclude))
            {
                Debug.Log("This hit at " + hitMouse.point);

                //only change position if first hit is not itself or other follow object
                if (hitMouse.collider.gameObject.tag != lookAtObject.tag)
                //if (hitMouse.colliderInstanceID == lookAtPlaneObject.GetComponent<Collider>().GetInstanceID())
                {
                    lookAtObject.transform.position = hitMouse.point;
                }
            }
        }
        

        RaycastHit hit;
        Vector3 rayDirection = transform.TransformDirection(Vector3.forward) * rayDistance;

        /*
        Plane p = new Plane(Vector3.right, 0);
        Ray ray = new Ray(transform.position, transform.forward);
        if (p.Raycast(ray, out float enter))
        {
            cursorPoint = ray.GetPoint(enter);
            if (!Application.isEditor)
            {
                //show hitpoint of ray on first object
                lookAtObject.transform.position = cursorPoint;
            }
        }
        */

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, layersToInclude))
        {
            cursorPoint = hit.point;//ray.GetPoint(enter);
            if (!Application.isEditor)
            {
                //show hitpoint of ray on first object
                //only change position if first hit is not itself or other follow object
                if (hit.collider.gameObject.tag != lookAtObject.tag)
                {
                    lookAtObject.transform.position = cursorPoint;
                }
            }

            Unselect();
            _lineRenderer.startColor = rayColorHoverState;
            _lineRenderer.endColor = rayColorHoverState;

            _noteHovered = hit.transform.GetComponent<EyeInteractable>();
            if (_noteHovered != null) _noteHovered.IsHovered = true;

            _sceneButtonHovered = hit.transform.GetComponent<ChangeSceneOnOver>();
            if (_sceneButtonHovered != null) _sceneButtonHovered.IsHovered = true;
        }
        else
        {
            _lineRenderer.startColor = rayColorDefaultState;
            _lineRenderer.endColor = rayColorDefaultState;
            Unselect(true);
        }

        if (!showLine)
        {
            _lineRenderer.startColor = colorInvisible;
            _lineRenderer.endColor = colorInvisible;
        }
    }

    private void Unselect(bool clear = false)
    {
        if (_noteHovered != null) _noteHovered.IsHovered = false;
        if (_sceneButtonHovered != null) _sceneButtonHovered.IsHovered = false;
        _noteHovered = null;
        _sceneButtonHovered = null;
    }

    private void SetupRay() //should be changed to some cursor w/ delay
    {
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = rayWidth;
        _lineRenderer.endWidth = rayWidth;
        _lineRenderer.startColor = rayColorDefaultState;
        _lineRenderer.endColor = rayColorDefaultState;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));
    }
}