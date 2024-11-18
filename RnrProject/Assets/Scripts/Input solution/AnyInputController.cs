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

    void Update()
    {
        OVRInput.Update();
        if (!PauseController.IsGamePaused) ReadLeftJoystick();
        ReadPauseButton();
    }

    void ReadLeftJoystick()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            float horizonal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (LeftJoystick != null) LeftJoystick(new Vector2(horizonal, vertical));
        }
    }

    void ReadPauseButton()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three) || Input.GetButtonDown("XButton"))
        {
            if (PauseButton != null) PauseButton();
        }
    }
}
