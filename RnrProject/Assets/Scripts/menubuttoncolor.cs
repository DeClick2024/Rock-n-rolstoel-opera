using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menubuttoncolor : MonoBehaviour
{
    
    public bool OnMouseEnterActive = false;
    int ind = 1;
    private void changeColorTo(Color toColor)
    {
        var noteColor4 = GameObject.Find("4 notes").GetComponent<Renderer>();
         noteColor4.material.SetColor("_Color", toColor);
      
    }
    private void OnMouseEnter()
    {
        if (!OnMouseEnterActive) return;
        changeColorTo(Color.red);
        StartCoroutine(waitCoroutine());
        ind = 2;


    }
    private void OnMouseExit()
    {
        if (!OnMouseEnterActive) return;
        changeColorTo(Color.white);
        ind = 1;
    }

    IEnumerator waitCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        if (ind == 2)
        {
            SceneManager.LoadScene("4 notes");
        }
        else

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

   
}
