using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileColliderCubeScript : MonoBehaviour {

    public GameObject connectedController;
    public GameObject menuButton;

    FileControllerScript controllerScript;
    public bool detectButtons = false;
    public bool detectedButton = false;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        if (connectedController)
        {
            controllerScript = connectedController.GetComponent<FileControllerScript>();
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
