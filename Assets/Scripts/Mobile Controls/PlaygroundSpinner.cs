using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundSpinner : MonoBehaviour
{

    private float angleY;

    public void SpinLeft()
    {
        StartCoroutine(SpinWait(1));
     }

    public void SpinRight()
    {
        StartCoroutine(SpinWait(-1));
    }

    //yDirection offers direction to spin; 1 is left; -1 is right
    public IEnumerator SpinWait(float yDirection)
    {

        angleY = 1;
        while (angleY < 105)
        {
            yield return new WaitForSeconds(0.015f);
            transform.Rotate(0, yDirection, 0);
            angleY++;
        }
        int slowDown = 0;
        while (slowDown < 12) {

            yield return new WaitForSeconds(slowDown * 0.015f);
            transform.Rotate(0, yDirection, 0);
            slowDown++;

        }
        

    }
    }
