using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiSendParentSetParameters : MonoBehaviour
{

    public bool OnMouseEnterActive = false;
    public bool UseUnityAudioClip = false;

    // Start is called before the first frame update
    void Start()
    {
        //if not in editor disable onmouseover
        if (!Application.isEditor)
        {
            OnMouseEnterActive = false;

            //look up all childs of Eyeinteractable
            EyeInteractable[] myItems = FindObjectsOfType(typeof(EyeInteractable)) as EyeInteractable[];
            //Debug.Log("Found " + myItems.Length + " instances with this script attached");
            foreach (EyeInteractable item in myItems)
            {
                item.OnMouseEnterActive = OnMouseEnterActive;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
     * Script to set parameters in all childs
     */
    private void OnValidate()
    {
        //look up all childs of Eyeinteractable
        EyeInteractable[] myItems = FindObjectsOfType(typeof(EyeInteractable)) as EyeInteractable[];
        //Debug.Log("Found " + myItems.Length + " instances with this script attached");
        foreach (EyeInteractable item in myItems)
        {
            item.OnMouseEnterActive = OnMouseEnterActive;
            item.UseUnityAudioClip = UseUnityAudioClip;
        }

        //set parameters


    }
}
