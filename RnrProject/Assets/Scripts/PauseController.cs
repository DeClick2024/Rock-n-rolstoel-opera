using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; }

    private void Awake()
    {
        AnyInputController.PauseButton += PauseOnOff;
    }

    void Update()
    {
        OVRInput.Update();
    }

    void OnDestroy()
    {
        AnyInputController.PauseButton -= PauseOnOff;
    }

    void PauseOnOff()
    {
        IsGamePaused = !IsGamePaused;
        Time.timeScale = IsGamePaused ? 0 : 1;
    }
}
