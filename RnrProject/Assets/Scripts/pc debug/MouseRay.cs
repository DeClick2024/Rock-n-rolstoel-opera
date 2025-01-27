using UnityEngine;

public class MouseRay : MonoBehaviour
{
    private EyeInteractable _noteHovered;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Unselect();
            _noteHovered = hit.transform.GetComponent<EyeInteractable>();
            if (_noteHovered != null) _noteHovered.IsHovered = true;
        } else Unselect();
    }

    private void Unselect()
    {
        if (_noteHovered != null) _noteHovered.IsHovered = false;
        _noteHovered = null;  
    }
}
