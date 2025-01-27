using System;
using UnityEngine;

[ExecuteInEditMode] //allows to do thing that should be done in runtime in editmode too
public class SceneSettings : MonoBehaviour
{
    [SerializeField][Range(-10.0f, 10.0f)]
    private float distanseFromCamera = 0.0f;
    private float previousDistanseFromCamera = 0.0f;

    [SerializeField][Range(0.5f, 1.5f)]
    private float buttonSize = 0.0f;
    private float previousButtonSize = 0.0f;

    //public event Action<float> ButtonsPosition;
    public event Action<float> ButtonsSize;

    private Transform _allButtons;
    private Vector3 _originalPosition;

    void OnEnable()
    {
        _allButtons = GetComponent<Transform>();
        _originalPosition = _allButtons.position;
    }

    void ChangeButtonsPosition()
    {
        //if (previousDistanseFromCamera != distanseFromCamera) ButtonsPosition?.Invoke(distanseFromCamera);
        if (previousDistanseFromCamera != distanseFromCamera)
        {
            Vector3 newPosition = new Vector3(_originalPosition.x, _originalPosition.y, _originalPosition.z + distanseFromCamera);
            _allButtons.position = newPosition;
            previousDistanseFromCamera = distanseFromCamera;
        }
    }

    void ChangeButtonsSize()
    {
        if (previousButtonSize!=buttonSize) ButtonsSize?.Invoke(buttonSize);
    }

    void Update()
    {
        ChangeButtonsSize();
        ChangeButtonsPosition();
    }
}