using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;

public class TestGameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            print("No gamepad");
        }
        if (gamepad is DualShockGamepad)
        {
            print("Playstation gamepad");
        }
        else if (gamepad is XInputController)
        {
            print("Xbox gamepad");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //this is not working
        //float vert = Input.GetAxis("Horizontal")

        //this is
        //float vert = Gamepad.current.leftStick.value.y;
        float vert = Gamepad.current.leftStick.x.ReadValue();
        if (vert > 0)
        {
            //Log if stick goes up
            Debug.Log("vert=" + vert);
        }
    }
}
