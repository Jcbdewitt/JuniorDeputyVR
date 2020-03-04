using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTextScript : MonoBehaviour {

    public GameObject vrHeadset;

    Transform vrHeadsetLocation;

    float damping = 1.0f;

    bool foundHeadset = false;

    private void Start()
    {
        if (GameObject.Find("Camera (head)"))
        {
            foundHeadset = true;
            vrHeadset = GameObject.Find("Camera (head)");
            vrHeadsetLocation = vrHeadset.GetComponent<Transform>();
        }
    }

    private void Update()
    {
        if (foundHeadset)
        {
            Vector3 lookPos = vrHeadsetLocation.position - transform.position;
            lookPos.y = 0.0f;
            Quaternion theRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, theRotation, Time.deltaTime * damping);
        }
    }
}
