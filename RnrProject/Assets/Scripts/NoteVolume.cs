using System;
using System.Collections;
using UnityEngine;

public class NoteVolume : MonoBehaviour
{
    [SerializeField] float WaitingTime = 0.3f;

    public static event Action<float> VolumeSender;

    private Vector2 pos;
    private float volume;

    private void Awake()
    {
        AnyInputController.LeftJoystick += JoystickRecieve;
    }

    private void Start()
    {
        StartCoroutine(NoteVolumeSetter());
    }

    void OnDestroy()
    {
        AnyInputController.LeftJoystick -= JoystickRecieve;
    }

    void JoystickRecieve(Vector2 joystickPosition)
    {
        pos = joystickPosition;
    }

    IEnumerator NoteVolumeSetter()
    {
        Vector2 startPos = Vector2.zero;
        while (true)
        {
            if (startPos != pos)
            {
                Vector2 temp = pos;
                volume = Vector2.Distance(pos, startPos);
                if (volume > 1) volume = 1;
                VolumeSender(volume);
                yield return new WaitForSeconds(WaitingTime);
                startPos = temp;  
            }
        }
    }
}
