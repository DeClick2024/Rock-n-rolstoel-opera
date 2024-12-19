using System;
using UnityEngine;  

public class AnyInputController : MonoBehaviour
{
    /// <summary>
    /// Subscribe to this event to track left joystick value
    /// </summary>
    public static event Action<Vector2> LeftJoystick;

    /// <summary>
    /// Subscribe to this event to detect when pause is on/offl
    /// </summary>
    public static event Action PauseButton;

    /// <summary>
    /// Subscribe to this event to detect arrows
    /// </summary>
    public static event Action<Vector2> Arrows;

    private float _prevValueY = 0;
    private float _prevValueX = 0;

    void Update()
    {
        OVRInput.Update();
        if (!PauseController.IsGamePaused) ReadLeftJoystick();
        ReadPauseButton();
        ReadArrows();
    }

    void ReadLeftJoystick()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            float horizonal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            LeftJoystick?.Invoke(new Vector2(horizonal, vertical));
        }
    }

    void ReadPauseButton()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three) || Input.GetButtonDown("XButton"))
        {
            PauseButton?.Invoke();
        }
    }

    void ReadArrows() //for switching the pitch
    {
        if(Input.GetJoystickNames().Length > 0)
        {
            float horizonal = Input.GetAxis("XDpadHorizontal");
            float vertical = Input.GetAxis("XDpadVertical");
            if (_prevValueX != horizonal || _prevValueY != vertical) Arrows?.Invoke(new Vector2(horizonal, vertical));
            _prevValueX = horizonal;
            _prevValueY = vertical;
        }
    }
}
