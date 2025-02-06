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
    private bool _isAtThePlayPosition = false;

    private float _currentJoystickXPosition;
    private float _prevJoystickXPosition = 0f;

    //[SerializeField] private UnityEvent OnObjectHover;
    [SerializeField] private Material _onHoverActiveMaterial;
    [SerializeField] private Material _onNoteTriggeredMaterial;
    [SerializeField] private AudioClip _hoverSound;
    private float _oneStringVelocity;
    private float[] _velocitiesSet = new float[4];

    [Tooltip("Set 'true' if you want to play note on middle position of the joystick, and 'false' if you want to play note on negative and positve positions")]
    [SerializeField] private bool PlayOnMiddlePosition;

    private int _finalVelocityINT;
    private float _finalVelocity;

    private MeshRenderer _meshRenderer;
    private Material _originalMaterial;
    private AudioSource _audioSource;

    [SerializeField] private SendNoteOnOver OscSend;

    private bool IsNoteTriggered = false;
    private bool timerActive = false;

    private void Awake()
    {
        AnyInputController.LeftJoystick += JoystickReciever;
    }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _originalMaterial = _meshRenderer.material;
    }

    private void Update()
    {
        ChangeMaterial();
        if (IsHovered) PlaySound();
    }

    private void PlaySound()
    {
        //Debug.Log($"'{IsHovered}, {isAtThePlayPosition}, {timerActive}");
        if (IsHovered && _isAtThePlayPosition && !timerActive)
        {
            _finalVelocity = (_velocitiesSet[0] + _velocitiesSet[1] + _velocitiesSet[2] + _velocitiesSet[3]) / 4;
            VelocityIntToFloat();
            OscSend.PlayNote(_finalVelocityINT);
            _audioSource.PlayOneShot(_hoverSound, _finalVelocity);
            StartCoroutine(StartTimer());
        }
    }

    private void ChangeMaterial()
    {
        if (IsHovered && !IsNoteTriggered) _meshRenderer.material = _onHoverActiveMaterial;
        else if (IsHovered && IsNoteTriggered) _meshRenderer.material = _onNoteTriggeredMaterial; //coroutine
        else if (!IsHovered) _meshRenderer.material = _originalMaterial;
    }

    private void FixedUpdate() //should be replaced by smth workable
    {
        _oneStringVelocity = _currentJoystickXPosition - _prevJoystickXPosition;
        if (_oneStringVelocity < 0) _oneStringVelocity *= -1;
        if (_oneStringVelocity > 1) _oneStringVelocity = 1;
        _prevJoystickXPosition = _currentJoystickXPosition;
        if (_oneStringVelocity != 0)
        {
            for (int i = 0; i < _velocitiesSet.Length - 1; i++)
            {
                _velocitiesSet[i] = _velocitiesSet[i + 1];
            }
            _velocitiesSet[0] = _oneStringVelocity;
        }
    }

    private void OnDestroy()
    {
        AnyInputController.LeftJoystick -= JoystickReciever;
    }

    void JoystickReciever(Vector2 currentPosition) //задержка времени
    {
        _currentJoystickXPosition = currentPosition.x;
        if (PlayOnMiddlePosition)
        {
            if (currentPosition.x == 0f) _isAtThePlayPosition = true;
            else _isAtThePlayPosition = false;
        }
        else if (!PlayOnMiddlePosition)
        {
            if (currentPosition.x >= 0.9f || currentPosition.x <= -0.9f) _isAtThePlayPosition = true;
            else _isAtThePlayPosition = false;
        }
    }

    void VelocityIntToFloat()
    {
        float temp = _finalVelocity * 100;
        _finalVelocityINT = Convert.ToInt32(temp);
    }

    private IEnumerator StartTimer()
    {
        timerActive = true; // Устанавливаем флаг
        yield return new WaitForSeconds(1f); // Ждём 1 секунду;
        timerActive = false; // Сбрасываем флаг
    }
}
