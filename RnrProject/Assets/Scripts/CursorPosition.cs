using UnityEngine;

public class CursorPosition : MonoBehaviour
{
    [SerializeField] EyeTrackingRay leftEyeTracking;
    [SerializeField] EyeTrackingRay rightEyeTracking;

    private Vector3 _realCursorPoint;

    void Start()
    {
        
    }

    void Update()
    {
        _realCursorPoint = (leftEyeTracking.cursorPoint + rightEyeTracking.cursorPoint) / 2;
        transform.position = _realCursorPoint;
    }
}
