using UnityEngine;

[ExecuteInEditMode]
public class CyllinderSizeSetter : MonoBehaviour
{
    private SceneSettings _sceneSettings;
    private Transform _buttonTransform;
    private Vector3 _originalScale;

    private void OnEnable()
    {
        _buttonTransform = GetComponent<Transform>();
        _sceneSettings = _buttonTransform.root.GetComponent<SceneSettings>();
        //_sceneSettings.ButtonsSize += ChangeButtonSize;
        _buttonTransform = GetComponent<Transform>();
        _originalScale = transform.localScale;
    }

    void Start()
    {
        
    }

    void ChangeButtonSize(float buttonSizeMultiplier)
    {
        _buttonTransform.localScale = new Vector3(_originalScale.x * buttonSizeMultiplier, _originalScale.y, _originalScale.z * buttonSizeMultiplier);
    }

    private void OnDestroy()
    {
        //_sceneSettings.ButtonsSize -= ChangeButtonSize;
    }
}