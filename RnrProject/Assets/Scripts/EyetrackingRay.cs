using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 1.0f;

    [SerializeField]
    private float rayWidth = 0.01f;

    [SerializeField]
    private LayerMask layersToInclude;

    [SerializeField]
    private Color rayColorDefaultState = Color.yellow;

    [SerializeField]
    private Color rayColorHoverState = Color.red;

    private LineRenderer lineRenderer;

    private List<EyeInteractable> eyeInteractables = new List<EyeInteractable>();
    private List<ChangeSceneOnOver> sceneChangeList = new List<ChangeSceneOnOver>();  

    float pokeForce;

    
   

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupRay();
    }

    private void SetupRay()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColorDefaultState;
        lineRenderer.endColor = rayColorDefaultState;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));
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
            UnSelect();
            lineRenderer.startColor = rayColorHoverState;
            lineRenderer.endColor = rayColorHoverState;

            //check for eye interactables
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>();
            if (eyeInteractable != null)
            {
                eyeInteractables.Add(eyeInteractable);
                eyeInteractable.IsHovered = true;
            }
            //eyeInteractables.Add(eyeInteractable);
            //eyeInteractable.IsHovered = true;

            //check for scenechange elements
            var sceneEyeInteractable = hit.transform.GetComponent<ChangeSceneOnOver>();
            if (sceneEyeInteractable != null)
            {
                sceneChangeList.Add(sceneEyeInteractable);
                sceneEyeInteractable.IsHovered = true;
            }
        }
        else
        {
            lineRenderer.startColor = rayColorDefaultState;
            lineRenderer.endColor = rayColorDefaultState;
            UnSelect(true);
        }
    }

    private void UnSelect(bool clear = false)
    {
        foreach (var interactable in eyeInteractables)
        {
            interactable.IsHovered = false;
        }

        foreach (var sceneEyeInteract in sceneChangeList)
        {
            sceneEyeInteract.IsHovered = false;
        }


        if (clear)
            eyeInteractables.Clear();
    }
}