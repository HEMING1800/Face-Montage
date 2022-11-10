using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceMontage : MonoBehaviour
{
    [SerializeField] GameObject postImage;

    private int leftComponetNumber;
    private int rightComponetNumber;
    private int mouthComponentNumber;

    // Create image in the certain location depends on the chose answer
    public void SetTheComponent(string whichCompArea, int whichComp)
    {

        switch (whichCompArea)
        {
            case "L":
                leftComponetNumber = whichComp;
                break;
            case "R":
                rightComponetNumber = whichComp;
                break;
            case "M":
                mouthComponentNumber = whichComp;
                break;
        }
     
    }

    public void DisplayFaceMontage()
    {
        GameObject leftEye = postImage.transform.GetChild(0).gameObject;
        GameObject rightEye = postImage.transform.GetChild(1).gameObject;
        GameObject mouth = postImage.transform.GetChild(2).gameObject;

        postImage.SetActive(true);
        leftEye.transform.GetChild(leftComponetNumber).gameObject.SetActive(true);
        rightEye.transform.GetChild(rightComponetNumber).gameObject.SetActive(true);
        mouth.transform.GetChild(mouthComponentNumber).gameObject.SetActive(true);
    }

}
