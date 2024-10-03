using UnityEngine;

public class Change : MonoBehaviour
{
    [SerializeField]
    private Color defaultColor = Color.white;
    [SerializeField]
    private Color hoverColor = Color.red;

    private Color originalColor;
    private Renderer rend;
    private bool isHovered = false;

    public bool IsHovered
    {
        get { return isHovered; }
        set { isHovered = value; }
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
        else
        {
            Debug.LogWarning("No Renderer component found on object " + gameObject.name);
        }
    }

    void Update()
    {
        if (isHovered)
        {
            ChangeColor(hoverColor);
        }
        else
        {
            ChangeColor(defaultColor);
        }
    }

    void ChangeColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
}
