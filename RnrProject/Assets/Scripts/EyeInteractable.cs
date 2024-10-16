using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(AudioSource))]

public class EyeInteractable : MonoBehaviour
{
    public bool IsHovered { get; set; }
    public bool OnMouseEnterActive = false; //should it be public?
    public bool UseUnityAudioClip = false;

    [SerializeField] private UnityEvent OnObjectHover;
    [SerializeField] private Material OnHoverActiveMaterial;
    [SerializeField] private Material OnHoverInactiveMaterial;
    [SerializeField] private AudioClip hoverSound;

    private MeshRenderer meshRenderer;
    private AudioSource audioSource;

    //[SerializeField] private extOSC.Examples.SendNoteOnOver OscSend;
    [SerializeField] private SendNoteOnOver OscSend;

    private bool IsNoteTriggered = false;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsHovered && !IsNoteTriggered)
        {
            OscSend.PlayNote();
            IsNoteTriggered = true;
            if (meshRenderer.material != OnHoverActiveMaterial)
            {
                audioSource.PlayOneShot(hoverSound);
                //audioSource.PlayOneShot(hoverSound, velocity); or smth like that

            }
            OnObjectHover?.Invoke();
            meshRenderer.material = OnHoverActiveMaterial;
        }
        else if (!IsHovered && IsNoteTriggered)
        {
            meshRenderer.material = OnHoverInactiveMaterial;
            OscSend.StopNote();
            IsNoteTriggered = false;
        }
    }

    //maybe this script could be split into 2 separate scripts
    //one for eyes, one for mouse

    /// <summary>
    /// Helper function to simulate eyetracking interactiuon in editor
    /// </summary>
    private void OnMouseEnter()
    {
        if (!OnMouseEnterActive) return;

        OscSend.PlayNote();
        if (meshRenderer.material != OnHoverActiveMaterial)
        {
            audioSource.PlayOneShot(hoverSound);
            //audioSource.PlayOneShot(hoverSound, velocity); or smth like that
        }
        meshRenderer.material = OnHoverActiveMaterial;
    }

    /// <summary>
    /// Helper function to simulate eyetracking interactiuon in editor
    /// </summary>
    private void OnMouseExit()
    {
        if (!OnMouseEnterActive) return;

        meshRenderer.material = OnHoverInactiveMaterial;
        OscSend.StopNote();
    }
}
