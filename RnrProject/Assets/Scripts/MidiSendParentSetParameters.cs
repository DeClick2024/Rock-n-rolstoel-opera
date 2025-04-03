using UnityEngine;

public class MidiSendParentSetParameters : MonoBehaviour
{
    [SerializeField] bool OnMouseEnterActive; //guess this can be hidden
    [SerializeField] bool UseUnityAudioClip;
    public bool UseJoyStick;

    void Start()
    {
        if (!Application.isEditor)
        {
            OnMouseEnterActive = false;

            EyeInteractable[] eyeInteractebles = FindEyeInteractables();
            foreach (EyeInteractable note in eyeInteractebles)
            {
                note.OnMouseEnterActive = OnMouseEnterActive;
            }
        }
    }

    private void OnValidate() //this should be removed before the building
    {
        EyeInteractable[] eyeInteractebles = FindEyeInteractables();
        foreach (EyeInteractable note in eyeInteractebles)
        {
            note.OnMouseEnterActive = OnMouseEnterActive;
            note.UseUnityAudioClip = UseUnityAudioClip;
            note.useJoyStick = UseJoyStick;
        }

        //set parameters
    }

    private EyeInteractable[] FindEyeInteractables()
    {
        EyeInteractable[] eyeInteractebles = new EyeInteractable[transform.childCount];
        int i = 0;
        foreach (Transform note in transform)
        {
            eyeInteractebles[i] = note.GetComponentInChildren<EyeInteractable>();
            i++;
        }
        return eyeInteractebles;
    }

    public void setUseJoyStick(bool value)
    {
        UseJoyStick = value;
        EyeInteractable[] eyeInteractebles = FindEyeInteractables();
        foreach (EyeInteractable note in eyeInteractebles)
        {
            note.useJoyStick = value;
        }
    }
}