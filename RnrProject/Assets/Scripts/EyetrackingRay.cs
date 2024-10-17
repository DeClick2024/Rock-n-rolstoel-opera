using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1.0f;
    [SerializeField] private float rayWidth = 0.01f;
    [SerializeField] private LayerMask layersToInclude;
    [SerializeField] private Color rayColorDefaultState = Color.yellow;
    [SerializeField] private Color rayColorHoverState = Color.red;

    private LineRenderer _lineRenderer;

    private EyeInteractable _noteHovered;
    private ChangeSceneOnOver _sceneButtonHovered;
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        SetupRay();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.TransformDirection(Vector3.forward) * rayDistance;
        Plane p = new Plane(Vector3.right, 0);
        var ray = new Ray(transform.position, transform.forward);
        if (p.Raycast(new Ray(transform.position, transform.forward), out float enter))
        {
            Vector3 thePoint = ray.GetPoint(enter);
        }

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, layersToInclude))
        {
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