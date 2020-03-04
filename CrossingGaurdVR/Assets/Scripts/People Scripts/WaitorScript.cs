using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitorScript : MonoBehaviour {

    public GameObject spawnner;
    public GameObject path;
    public GameObject door;
    public GameObject rightHand;
    public GameObject selectedRandomFood;
    public GameObject tableLocation;
    GameObject spawnnedFood;

    public GameObject randomFoodOne;
    public GameObject randomFoodTwo;
    public GameObject randomFoodThree;
    public GameObject randomFoodFour;

    public Animator walkingAnimation;

    public DoorScript doorScript;

    public List<Transform> pathToTake = new List<Transform>();

    public Transform rightHandTransform;
    public Transform randomFoodTransform;
    public Transform tableLocationTransform;

    public float speed = 5.0f;
    public float maxDistanceToPoint = 0.5f;
    public float damping = 1.0f;

    public int current = 0;
    private int countdownNumber = 30;
    private int countdownReset = 30;
    private int currentToStop = 3;
    public int countDownToTurnAround;
    public int advanceNumber = 1;

    public bool spawnFood = true;
    public bool startServering = false;
    public bool wait = false;
    public bool stopped = false;
    public bool turnAroundReached = false;
    bool startFoodDestroyCountDown = false;
    bool runOnce = true;

    // Use this for initialization
    void Start ()
    { 
        pathToTake = path.GetComponent<PathScripts>().nodes;
        doorScript = door.GetComponent<DoorScript>();
        rightHandTransform = rightHand.GetComponent<Transform>();
        tableLocationTransform = tableLocation.GetComponent<Transform>();

        countDownToTurnAround = pathToTake.Count - 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (spawnner.GetComponent<IntroSpawnnerScript>().startGame == true) { startServering = true; }

        if (startServering)
        {
            Walking();
            OpenDoor();
            Looking();
            WalkingAnimation();
        }

        if (spawnFood)
        {

            int randomFoodNumber = UnityEngine.Random.Range(0, 5);
            switch (randomFoodNumber)
            {
                case 0:
                    selectedRandomFood = randomFoodOne;
                    break;
                case 1:
                    selectedRandomFood = randomFoodTwo;
                    break;
                case 2:
                    selectedRandomFood = randomFoodThree;
                    break;
                case 3:
                    selectedRandomFood = randomFoodFour;
                    break;
            }
            spawnnedFood = Instantiate(selectedRandomFood, rightHandTransform);
            spawnnedFood.transform.localScale = new Vector3(20.00f, 20.00f, 20.00f);
            spawnFood = false;
        }
    }

    private void Walking()
    {
        if (wait)
        {
            if (stopped)
            {
                StartCoroutine(Countdown());
                stopped = false;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, pathToTake[current].position) > maxDistanceToPoint)
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, pathToTake[current].position, speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(pos);
            }
            else if (countDownToTurnAround == 0)
            {
                countDownToTurnAround = pathToTake.Count - 1;
                advanceNumber = -advanceNumber;
                if (!turnAroundReached) { turnAroundReached = true; }
                else
                {
                    wait = true;
                    stopped = true;
                    turnAroundReached = false;
                }
            }
            else
            {
                current = (current + advanceNumber) % pathToTake.Count;
                currentToStop = currentToStop - 1;
                countDownToTurnAround = countDownToTurnAround - 1;
            }
        }

        if (!turnAroundReached)
        {
            spawnnedFood.GetComponent<Transform>().position = rightHandTransform.position;
        }
        else
        {
            spawnnedFood.GetComponent<Transform>().position = tableLocationTransform.position;
            spawnnedFood.GetComponent<Transform>().rotation = tableLocationTransform.rotation;
            if (startFoodDestroyCountDown && runOnce)
            {
                startFoodDestroyCountDown = false;
                runOnce = false;
                StartCoroutine(FoodCountdown());
            }
        }
    }

    private void Looking()
    {
        Vector3 lookPos = pathToTake[current].position - transform.position;
        lookPos.y = 0.0f;
        Quaternion theRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, theRotation, Time.deltaTime * damping);
    }

    private void OpenDoor()
    {
        if (wait)
        {
            doorScript.openDoor = false;
        }
        else
        {
            doorScript.openDoor = true;
        }
    }

    private void WalkingAnimation()
    {
        if (wait == false)
        {
            walkingAnimation.SetInteger("Moving", 1);
        }
        else
        {
            walkingAnimation.SetInteger("Moving", 0);
        }
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1.0f);
        countdownNumber -= 1;
        if (countdownNumber > 0)
        {
            StartCoroutine(Countdown());
        }
        else
        {
            wait = false;
            countdownNumber = countdownReset;
        }
    }
    private IEnumerator FoodCountdown()
    {
        yield return new WaitForSeconds(15.0f);
        Debug.Log("deleted food on table");
        spawnFood = true;
        startFoodDestroyCountDown = true;
        runOnce = true;
        Destroy(spawnnedFood.gameObject);
    }
}
