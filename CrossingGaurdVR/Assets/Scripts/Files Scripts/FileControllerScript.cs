using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileControllerScript : MonoBehaviour {

    public GameObject connectedColliderCube;
    public GameObject filesLeft;

    SteamVR_TrackedController steamControllerScript;

    FileColliderCubeScript colliderCubeScript;

    public bool inGame = true;
    bool triggerPress = false;

    private void Awake()
    {
        colliderCubeScript = connectedColliderCube.GetComponent<FileColliderCubeScript>();
        steamControllerScript = GetComponent<SteamVR_TrackedController>();
    }

	void Update ()
    {
		if (!inGame)
        {
            MenuButton();
            connectedColliderCube.GetComponent<Renderer>().enabled = true;
            colliderCubeScript.detectButtons = true;
        }
        else
        {
            connectedColliderCube.GetComponent<Renderer>().enabled = false;
        }
	}

    void MenuButton()
    {
        colliderCubeScript.detectButtons = true;
        colliderCubeScript.GetComponent<MeshRenderer>().enabled = true;
        if (colliderCubeScript.detectedButton)
        {
            colliderCubeScript.menuButton.GetComponent<FileMenuButtonScript>().highlighted = true;
        }
        else
        {
            colliderCubeScript.menuButton.GetComponent<FileMenuButtonScript>().highlighted = false;
        }

        if (triggerPress && colliderCubeScript.detectedButton)
        {
            colliderCubeScript.menuButton.GetComponent<FileMenuButtonScript>().selected = true;
        }

    }

    void DetectControllerButtons()
    {
        if (steamControllerScript.triggerPressed) { triggerPress = true; }
        else { triggerPress = false; }
    }
}
