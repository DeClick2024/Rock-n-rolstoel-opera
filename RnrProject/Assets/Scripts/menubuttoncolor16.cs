using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menubuttoncolor16 : MonoBehaviour
{
    
    public bool OnMouseEnterActive = false;
    int ind16 = 1;
    private void changeColorTo(Color toColor)
    {
        var noteColor16 = GameObject.Find("16 notes").GetComponent<Renderer>();
         noteColor16.material.SetColor("_Color", toColor);
      
    }
    private void OnMouseEnter()
    {
        if (!OnMouseEnterActive) return;
        changeColorTo(Color.red);
        StartCoroutine(wait16Coroutine());
        ind16 = 2;


    }
    private void OnMouseExit()
    {
        if (!OnMouseEnterActive) return;
        changeColorTo(Color.white);
        ind16 = 1;
    }

    IEnumerator wait16Coroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        if (ind16 == 2)
        {
            SceneManager.LoadScene("16 notes");
        }
        else

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

   
}
