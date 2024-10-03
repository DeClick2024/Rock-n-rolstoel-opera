using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGuideBall : MonoBehaviour
{
    public GuideBall guideBall; // Reference to the GuideBall script
    private bool isMoving = false;

    void Update()
    {
        // Start moving the GuideBall when the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true;
            StartCoroutine(MoveGuideBall());
        }
    }

    IEnumerator MoveGuideBall()
    {
        int currentIndex = 0;

        while (isMoving)
        {
            guideBall.MoveToPosition(currentIndex);

            // Wait until the ball has reached the target position
            while ((guideBall.transform.position - (guideBall.positions[currentIndex].position + new Vector3(0, 0, -0.5f))).sqrMagnitude > 0.01f)

            {
                yield return null;
            }

            // Move to the next position in the array
            currentIndex = (currentIndex + 1) % guideBall.positions.Length;
        }
    }
}
