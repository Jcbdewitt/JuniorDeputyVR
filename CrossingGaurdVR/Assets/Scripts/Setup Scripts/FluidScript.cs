using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidScript : MonoBehaviour {

    public GameObject mLiquid;
    public GameObject mLiquidMesh;

    public Animator mugAnimator;

    private float timer = 0.0f;

    private int mSloshSpeed = 60;
    private int difference = 25;

    private bool startTimer = false;

    // Update is called once per frame
    void Update () {
        Slosh();
        Spill();
        LiquidLevels();

        mugAnimator.SetFloat("Seconds", timer);
	}

    private void Slosh()
    {
        //Inverse cup rotation
        Quaternion inverseRotation = Quaternion.Inverse(transform.localRotation);

        //Rotate to
        Vector3 finalRotation = Quaternion.RotateTowards(mLiquid.transform.localRotation, inverseRotation, mSloshSpeed * Time.deltaTime).eulerAngles;

        //Clamp
        finalRotation.x = ClampRotationValue(finalRotation.x, difference);
        finalRotation.z = ClampRotationValue(finalRotation.z, difference);

        //Set 
        mLiquid.transform.localEulerAngles = finalRotation;
    }

    private void Spill()
    {
        if (Mathf.Abs(transform.localRotation.eulerAngles.z) > 80.0f) { startTimer= true; }
        else { startTimer = false; }
        
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
    }

    private void LiquidLevels()
    {
        if (timer > 3.2f) { mLiquidMesh.GetComponent<Renderer>().enabled = false; }
    }

    private float ClampRotationValue(float value, float difference)
    {
        float returnValue = 0.0f;

        if (value > 180)
        {
            //Clamp
            returnValue = Mathf.Clamp(value, 360 - difference, 360);
        }
        else
        {
            //Clamp
            returnValue = Mathf.Clamp(value, 0, difference);
        }

        return returnValue;
    }
}
