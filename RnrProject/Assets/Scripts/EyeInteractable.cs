using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(AudioSource))]

public class EyeInteractable : MonoBehaviour
{
    public bool IsHovered { get; set; } //changed by eyetrackingray (need to make a mouse ray for tests)
    public bool OnMouseEnterActive = false;
    public bool UseUnityAudioClip = false;
    public bool useJoyStick = true;
    private bool isAtThePlayPosition = false;

    private float currentJoystickXPosition;
    private float prevJoystickXPosition = 0f;

    //[SerializeField] private UnityEvent OnObjectHover;
    [SerializeField] private Material OnHoverActiveMaterial;
    [SerializeField] private Material OnNoteTriggeredMaterial;
    [SerializeField] private AudioClip hoverSound;
    private float OneStringVelocity;

    [Tooltip("Set 'true' if you want to play note on middle position of the joystick, and 'false' if you want to play note on negative and positve positions")]
    [SerializeField] private bool PlayOnMiddlePosition;

    private int OneStringVelocityINT;

    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    private AudioSource audioSource;

    [SerializeField] private SendNoteOnOver OscSend;

    private bool IsNoteTriggered = false;
    private bool timerActive = false;
    private float joyStickZeroStamp = 0;
    private float timeStrumValue = 0;
    private bool isOnMiddlePos = true;
    private bool notePlayed = false;
    private bool triggeredByStick = false;

    private void Awake()
    {
        AnyInputController.LeftJoystick += JoystickReciever;
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        originalMaterial = meshRenderer.material;
    }

    private void Update()
    {
        ChangeMaterial();
        if (IsHovered && useJoyStick) PlaySound();
        else if (IsHovered && !notePlayed)
        {
            OscSend.PlayNote(100);
            audioSource.PlayOneShot(hoverSound);
            notePlayed = true;
        }

        if (!IsHovered & !triggeredByStick)
        {
            OscSend.StopNote();
            notePlayed = false;
        }
    }

    private void PlaySound()
    {
        if (!useJoyStick)
        {
            OscSend.PlayNote(100);
        }
        else
        {
            //Debug.Log($"'{IsHovered}, {isAtThePlayPosition}, {timerActive}");
            if (IsHovered && isAtThePlayPosition && !timerActive && !notePlayed)
            {
                notePlayed = true;
                Debug.Log("playSound:" + timeStrumValue);
                //OscSend.PlayNote(OneStringVelocityINT);
                int volumeSend = Convert.ToInt32(timeStrumValue * 100);

                if (volumeSend < 0) volumeSend = 1;
                if (volumeSend > 128) volumeSend = 128;


                OscSend.PlayNote(volumeSend);
                //audioSource.PlayOneShot(hoverSound, OneStringVelocity);
                audioSource.PlayOneShot(hoverSound, timeStrumValue);
                //StartCoroutine(StartTimer());
            }
        }
    }

    private void ChangeMaterial()
    {
        if (IsHovered && !IsNoteTriggered) meshRenderer.material = OnHoverActiveMaterial;
        else if (IsHovered && IsNoteTriggered) meshRenderer.material = OnNoteTriggeredMaterial; //coroutine
        else if (!IsHovered) meshRenderer.material = originalMaterial;
    }

    private void FixedUpdate()
    {
        //Debug.Log("curjoy=" + currentJoystickXPosition + " prevjoy=" + prevJoystickXPosition);
        OneStringVelocity = currentJoystickXPosition - prevJoystickXPosition;
        if (OneStringVelocity < 0) OneStringVelocity *= -1;
        if (OneStringVelocity > 1) OneStringVelocity = 1;
        VelocityIntToFloat();
        prevJoystickXPosition = currentJoystickXPosition;
    }

    private void OnDestroy()
    {
        AnyInputController.LeftJoystick -= JoystickReciever;
    }

    void JoystickReciever(Vector2 currentPosition) //задержка времени
    {
        if (!IsHovered) return;

        currentJoystickXPosition = currentPosition.x;
        if (PlayOnMiddlePosition)
        {
            if (currentPosition.x == 0f) isAtThePlayPosition = true;
            else isAtThePlayPosition = false;
        }
        else if (!PlayOnMiddlePosition)
        {
            if (currentPosition.x == 0)
            {
                if (!isOnMiddlePos)
                {
                    Debug.Log("Set middle position");
                    OscSend.StopNote();
                    timerActive = false;
                    isOnMiddlePos = true;
                    notePlayed = false;
                    isAtThePlayPosition = false;
                }
                joyStickZeroStamp = Time.time;

            }
            else if (currentPosition.x >= 0.9f || currentPosition.x <= -0.9f)
            {
                if (!notePlayed)
                {
                    Debug.Log("Play note");
                    timeStrumValue = 1 - (Time.time - joyStickZeroStamp);
                    Debug.Log("Timestrumvalue=" + timeStrumValue);
                    isAtThePlayPosition = true;
                    isOnMiddlePos = false;
                    //notePlayed = true;
                }

            }
            else
            {
                isOnMiddlePos = false;
                isAtThePlayPosition = false;
            }
        }
    }

    void VelocityIntToFloat()
    {
        float temp = OneStringVelocity * 100;
        OneStringVelocityINT = Convert.ToInt32(temp);
    }

    private IEnumerator StartTimer()
    {
        timerActive = true; // Устанавливаем флаг
        yield return new WaitForSeconds(1f); // Ждём 1 секунду;
        timerActive = false; // Сбрасываем флаг
    }

    private void OnTriggerEnter(Collider other)
    {
        OscSend.PlayNote(100);
        audioSource.PlayOneShot(hoverSound);
        notePlayed = true;
        triggeredByStick = true;

    }

    private void OnTriggerExit(Collider other)
    {
        OscSend.StopNote();
        notePlayed = false;
        triggeredByStick = false;
    }
}