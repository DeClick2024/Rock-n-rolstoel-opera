using UnityEngine;

public class MidiSendParentSetParameters : MonoBehaviour
{
    [SerializeField] bool OnMouseEnterActive; //guess this can be hidden
    [SerializeField] bool UseUnityAudioClip;

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

    private void OnValidate() //this should be removed and all the necessary logis should be transfered to start
    {
        EyeInteractable[] eyeInteractebles = FindEyeInteractables();
        foreach (EyeInteractable note in eyeInteractebles)
        {
            note.OnMouseEnterActive = OnMouseEnterActive;
            note.UseUnityAudioClip = UseUnityAudioClip;
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
}