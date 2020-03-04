using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {

    public GameObject Spawnner;
    public GameObject connectedColliderCube;
    public GameObject connectedColliderStop;

    SteamVR_TrackedController steamControllerScript;

    ColliderCubeScript colliderCubeScript;

    ColliderStopScript colliderStopScript;

    CallToSpawn callToSpawn;

    float absRotationX;
    float absRotationZ;

    public bool inGame = false;
    public bool inMenuButton = false;
    public bool onMainMenu = false;
    public bool detectCollisons = false;
    bool triggerPress = false;
    bool overEnoughX = false;
    bool overEnoughZ = false;
 

    private void Awake()
    {
        if (!onMainMenu)
        {
            colliderStopScript = connectedColliderStop.GetComponent<ColliderStopScript>();
        }
        colliderCubeScript = connectedColliderCube.GetComponent<ColliderCubeScript>();
        steamControllerScript = GetComponent<SteamVR_TrackedController>();
        if (!onMainMenu)
        {
            callToSpawn = Spawnner.GetComponent<CallToSpawn>();
        }
    }

    void Update ()
    {
        if (!onMainMenu)
        {
            TryToStop();
        }
        else
        {
            if (!onMainMenu)
            {
                colliderStopScript.GetComponent<MeshRenderer>().enabled = false;
            }
            MenuButton();
            DetectControllerButtons();
        }
    }

    private void TryToStop()
    {
        if (callToSpawn.continuallySpawn)
        {
            colliderCubeScript.GetComponent<MeshRenderer>().enabled = false;

            ControllerRotation();
            if (detectCollisons)
            {
                colliderStopScript.GetComponent<MeshRenderer>().enabled = true;
                colliderStopScript.colliedWithACar = true;
            }
            else
            {
                colliderStopScript.GetComponent<MeshRenderer>().enabled = false;
                colliderStopScript.colliedWithACar = false;
            }
        }
        else
        {
            colliderStopScript.GetComponent<MeshRenderer>().enabled = false;
            MenuButton();
            DetectControllerButtons();
        }
    }

    void ControllerRotation()
    {
        colliderCubeScript.detectButtons = false;

        absRotationX = Mathf.Abs(transform.eulerAngles.x);
        absRotationZ = Mathf.Abs(transform.eulerAngles.z);

        //Debug.Log("X: " + absRotationX);
        //Debug.Log("Z: " + absRotationZ);

        // i think when eulerangles goes to go negetive it hops to postive 360 and goes backwards investigate
        //i think its fixed
        if (absRotationX < 180.0f)
        {
            if (0.0f <= absRotationX && absRotationX <= 25.0f) { overEnoughX = true; }
            else { overEnoughX = false; }
        }
        else
        {
            if (330.0f <= absRotationX && absRotationX <= 360.0f) { overEnoughX = true; }
            else { overEnoughX = false; }
        }

        if (absRotationZ < 180.0f)
        {
            if (60.0f <= absRotationZ && absRotationZ <= 115.0f) { overEnoughZ = true; }
            else { overEnoughZ = false; }
        }
        else
        {
            if (240.0f <= absRotationZ && absRotationZ <= 295.0f) { overEnoughZ = true; }
            else { overEnoughZ = false; }
        }


        if (overEnoughX && overEnoughZ)
        {
            //connectedColliderCube.GetComponent<MeshRenderer>().enabled = true;
            detectCollisons = true;
        }
        else { detectCollisons = false; }
    }

    void MenuButton()
    {
        colliderCubeScript.detectButtons = true;
        colliderCubeScript.GetComponent<MeshRenderer>().enabled = true;

        if (triggerPress && colliderCubeScript.detectedButton)
        {
            colliderCubeScript.menuButton.GetComponent<MenuButtonScript>().selected = true;
        }

    }

    void DetectControllerButtons()
    {
        if (steamControllerScript.triggerPressed) { triggerPress = true; }
        else { triggerPress = false; }
    }
}
