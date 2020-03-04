using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCubeScript : MonoBehaviour {

    public GameObject connectedController;
    public GameObject menuButton;

    ControllerScript controllerScript;
    public bool detectButtons = false;
    public bool detectedButton = false;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        if (connectedController)
        {
            controllerScript = connectedController.GetComponent<ControllerScript>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Button" && detectButtons)
        {
            menuButton = other.gameObject;
            detectedButton = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Button" && detectButtons)
        {
            detectedButton = false;
        }
    }
}
