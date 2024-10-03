using UnityEngine;
using Oculus;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();

        if (OVRInput.GetDown(OVRInput.Button.Three) || Input.GetButtonDown("XButton"))
        {
            IsGamePaused = !IsGamePaused;
            Time.timeScale = IsGamePaused ? 0 : 1;
        }
    }
}
