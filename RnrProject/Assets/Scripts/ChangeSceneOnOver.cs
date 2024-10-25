using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

enum OnOverRole { ChangeScene, Quit };

public class ChangeSceneOnOver : MonoBehaviour
{
    //private SceneAsset GotoScene;  //only works in editor, not on headset
    public bool IsHovered { get; set; }
    public bool OnMouseEnterActive = false;

    
    [SerializeField] private OnOverRole role;
    [SerializeField] private string GotoSceneString;
    [SerializeField] private UnityEvent OnObjectHover;
    [SerializeField] private Material OnHoverActiveMaterial;
    [SerializeField] private Material OnHoverInactiveMaterial;

    public TMPro.TextMeshProUGUI UIText; //can we change it from ccanvas to smth else?
    private MeshRenderer meshRenderer;

    private bool _isButtonTriggered = false;
    private int _buttonTriggerSeconds = -1;

    private void OnValidate()
    {
        if (GotoSceneString != null)
        {
            gameObject.name = "Goto: " + GotoSceneString;
            UIText.text = GotoSceneString;
        }
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (IsHovered && !_isButtonTriggered) OnStartHover();
        else if (!IsHovered && _isButtonTriggered) OnEndHover();
    }

    private void OnMouseEnter()
    {
        if (!OnMouseEnterActive) return;
        IsHovered = true;
    }


    private void OnMouseExit()
    {
        if (!OnMouseEnterActive) return;
        IsHovered = false;
    }

    void OnStartHover()
    {
        _isButtonTriggered = true;
        OnObjectHover?.Invoke();
       
        if (meshRenderer.material != OnHoverActiveMaterial)
        {
            _buttonTriggerSeconds = 0;
            StartCoroutine(waitCoroutine());
            UIText.text = GotoSceneString + " " + (5 - _buttonTriggerSeconds).ToString();
        }
        meshRenderer.material = OnHoverActiveMaterial;
    }

    void OnEndHover()
    {
        Debug.Log("Onendhover");
        meshRenderer.material = OnHoverInactiveMaterial;
        _isButtonTriggered = false;
        _buttonTriggerSeconds = -1;
    }

    IEnumerator waitCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        for (int i=0; i<6; i++)
        {
            Debug.Log("ind=" + _buttonTriggerSeconds);
            yield return new WaitForSeconds(1);

            if (_buttonTriggerSeconds >= 5)
            {
               if (role == OnOverRole.ChangeScene)
                {
                    SceneManager.LoadScene(GotoSceneString);
                }
                if (role == OnOverRole.Quit)
                {
                    Debug.Log("Quit from menu");
                    Application.Quit();
                }

            }
            else if (_buttonTriggerSeconds >= 0)
            {

                _buttonTriggerSeconds++;
                UIText.text = GotoSceneString + " " + (5 - _buttonTriggerSeconds).ToString();
            }

            else
            {
                UIText.text = GotoSceneString;
                break;
            }
        }
        //Print the time when the function ends
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
