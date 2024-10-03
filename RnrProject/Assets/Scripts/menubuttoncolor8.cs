using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menubuttoncolor8 : MonoBehaviour
{
    
    public bool OnMouseEnterActive = false;
    int ind8 = 1;
    private void changeColorTo(Color toColor)
    {
        var noteColor8 = GameObject.Find("8 notes").GetComponent<Renderer>();
         noteColor8.material.SetColor("_Color", toColor);
      
    }
    private void OnMouseEnter()
    {
        if (!OnMouseEnterActive) return;
        changeColorTo(Color.red);
        StartCoroutine(wait8Coroutine());
        ind8 = 2;


    }
    private void OnMouseExit()
    {
        if (!OnMouseEnterActive) return;
        changeColorTo(Color.white);
        ind8 = 1;
    }

    IEnumerator wait8Coroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        if (ind8 == 2)
        {
            SceneManager.LoadScene("Testing");
        }
        else

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

   
}
