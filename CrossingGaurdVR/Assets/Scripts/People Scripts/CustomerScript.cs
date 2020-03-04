using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour {

    public GameObject rightDoor;
    public GameObject leftDoor;
    public GameObject spawnedFood;
    public GameObject rightHand;

    public DoorScript rightDoorScript;
    public DoorScript leftDoorScript;

    Transform rightDoorTransform;
    Transform leftDoorTransform;
    Transform rightHandTransform;

    public float distanceToOpenDoor = 5.5f;
    public float distanceToLeftOpenDoor = 3.5f;

    public bool customerMode = false;
    public bool spawnRandomFood = false;
    bool firstDoorOpened = false;
    bool shutRightDoor = false;

    private void Start()
    {
        rightDoorScript = rightDoor.GetComponent<DoorScript>();
        leftDoorScript = leftDoor.GetComponent<DoorScript>();

        rightDoorTransform = rightDoor.GetComponent<Transform>();
        leftDoorTransform = leftDoor.GetComponent<Transform>();
        rightHandTransform = rightHand.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
		if (customerMode)
        {
            OpenDoors();
        }

        if (spawnRandomFood)
        {
            spawnRandomFood = false;

            GameObject justSpawnnedFood;

            justSpawnnedFood = Instantiate(spawnedFood, rightHandTransform);
            justSpawnnedFood.transform.localScale = new Vector3(20.00f, 20.00f, 20.00f);
        }
	}

    void OpenDoors()
    {
        if (shutRightDoor)
        {
            if (Vector3.Distance(transform.position, leftDoorTransform.position) < distanceToLeftOpenDoor)
            {
                leftDoorScript.openDoor = true;
            }
            else
            {
                leftDoorScript.openDoor = false;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, rightDoorTransform.position) < distanceToOpenDoor)
            {
                rightDoorScript.openDoor = true;
                firstDoorOpened = true;
            }
            else
            {
                if ((Vector3.Distance(transform.position, rightDoorTransform.position) > distanceToOpenDoor) && firstDoorOpened)
                {
                    rightDoorScript.openDoor = false;
                    StartCoroutine(LeftDoorDelay());
                }
            }
        }
    }

    private IEnumerator LeftDoorDelay()
    {
        yield return new WaitForSeconds(2.0f);
        shutRightDoor = true;
    }
}
