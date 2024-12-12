using Meta.WitAi;
using System;
using System.Diagnostics.Eventing.Reader;
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
    private bool isAtThePlayPosition = false;

    private float currentJoystickXPosition;
    private float prevJoystickXPosition = 0f;

    [SerializeField] private UnityEvent OnObjectHover;
    [SerializeField] private Material OnHoverActiveMaterial;
    [SerializeField] private Material OnHoverInactiveMaterial;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private float OneStringVelocity;

    [SerializeField][Tooltip("Set 'true' if you want to play note on middle position of the joystick, and 'false' if you want to play note on negative and positve positions")]
    private bool PlayOnMiddlePosition;

    private int OneStringVelocityINT;

    private MeshRenderer meshRenderer;
    private AudioSource audioSource;

    [SerializeField] private SendNoteOnOver OscSend;

    private bool IsNoteTriggered = false;

    private void Awake()
    {
        AnyInputController.LeftJoystick += JoystickReciever;
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsHovered && !IsNoteTriggered)
        {
            OscSend.PlayNote(OneStringVelocityINT); 
            IsNoteTriggered = true;
            if (meshRenderer.material != OnHoverActiveMaterial)
            {
                audioSource.PlayOneShot(hoverSound, OneStringVelocity); 

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

    private void FixedUpdate()
    {
        OneStringVelocity = currentJoystickXPosition - prevJoystickXPosition;
        if (OneStringVelocity < 0) OneStringVelocity *= -1;
        if (OneStringVelocity > 1) OneStringVelocity = 1;
        VelocityIntToFloat();
        prevJoystickXPosition = currentJoystickXPosition;
        if (OneStringVelocity != 0) Debug.Log(OneStringVelocity);
    }

    private void OnDestroy()
    {
        AnyInputController.LeftJoystick -= JoystickReciever;
    }


    /// <summary>
    /// Helper function to simulate eyetracking interactiuon in editor
    /// </summary>
    private void OnMouseOver()
    {
        if (!OnMouseEnterActive) return;

        if (isAtThePlayPosition)
        {
            OscSend.PlayNote(OneStringVelocityINT);
            if (meshRenderer.material != OnHoverActiveMaterial)
            {
                //audioSource.PlayOneShot(hoverSound);
                audioSource.PlayOneShot(hoverSound, OneStringVelocity);
                isAtThePlayPosition = false;
            }
            meshRenderer.material = OnHoverActiveMaterial;
        }
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

    void JoystickReciever(Vector2 currentPosition)
    {
        currentJoystickXPosition = currentPosition.x;
        if (PlayOnMiddlePosition)
        {
            if (currentPosition.x == 0f)
            {
                isAtThePlayPosition = true;
                Debug.Log(isAtThePlayPosition);
            }
        } else if (!PlayOnMiddlePosition)
        {
            if (currentPosition.x >= 0.9f || currentPosition.x <= -0.9f)
            {
                isAtThePlayPosition = true;
                Debug.Log(isAtThePlayPosition);
            }
        }
        else isAtThePlayPosition = false;
    }

    void VelocityIntToFloat()
    {
        float temp = OneStringVelocity * 100;
        OneStringVelocityINT = Convert.ToInt32(temp);
    }
}
