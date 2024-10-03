using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(AudioSource))]
public class EyeInteractable : MonoBehaviour
{
    public bool IsHovered { get; set; }
    public bool OnMouseEnterActive = false;
    public bool UseUnityAudioClip = false;

    [SerializeField]
    private UnityEvent OnObjectHover;

    [SerializeField]
    private Material OnHoverActiveMaterial;

    [SerializeField]
    private Material OnHoverInactiveMaterial;

    [SerializeField]
    private AudioClip hoverSound;

    private MeshRenderer meshRenderer;
    private AudioSource audioSource; 

    [SerializeField]

    private extOSC.Examples.SendNoteOnOver OscSend;

    private bool Isenter=false;
    


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
      
    }

    private void Update()
    {
        if (IsHovered && !Isenter)
        {
            OscSend.PlayNote();
            Isenter = true;
            if (meshRenderer.material != OnHoverActiveMaterial)
            {
                audioSource.PlayOneShot(hoverSound);
                
            }
            OnObjectHover?.Invoke();
            meshRenderer.material = OnHoverActiveMaterial;
        }
        else if (!IsHovered && Isenter)
        {
            meshRenderer.material = OnHoverInactiveMaterial;
            OscSend.StopNote();
            Isenter = false;
        }
    }

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
