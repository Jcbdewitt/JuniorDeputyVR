using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSpawnnerScript : MonoBehaviour {

    public GameObject spawnner;
    public GameObject introOfficer;
    public GameObject path;
    public GameObject firstDoor;
    public GameObject secondDoor;
    public GameObject poleToLookAt;
    public GameObject poleToLookAt1;
    public GameObject clock;

    public bool startGame = false;

	void Start ()
    {
        GameObject spawnnedOfficer;
        IntroOfficerScript spawnnedOfficerScript;

        spawnnedOfficer = Instantiate(introOfficer, transform.position, transform.rotation);
        spawnnedOfficerScript = spawnnedOfficer.GetComponent<IntroOfficerScript>();
        spawnnedOfficerScript.path = path;
        spawnnedOfficerScript.firstDoor = firstDoor;
        spawnnedOfficerScript.secondDoor = secondDoor;
        spawnnedOfficerScript.spawnner = gameObject;
        spawnnedOfficerScript.poleToLookAt = poleToLookAt;
        spawnnedOfficerScript.poleToLookAt1 = poleToLookAt1;
        spawnnedOfficerScript.clock = clock;
	}

    private void Update()
    {
        if (startGame)
        {
            startGame = false;
            spawnner.GetComponent<CallToSpawn>().startSpawning = true;
        }
    }
}
