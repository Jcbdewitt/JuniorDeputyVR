using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroOfficerScript : MonoBehaviour {

    public GameObject spawnner;
    public GameObject path;
    public GameObject firstDoor;
    public GameObject secondDoor;
    public GameObject poleToLookAt;
    public GameObject poleToLookAt1;
    public GameObject clock;

    public Animator walkingAnimation;

    public AudioSource audioSource;

    public AudioClip IntroClip;
    public AudioClip OutroClip;

    public ClockScript clockScript;

    public DoorScript firstDoorScript;
    public DoorScript secondDoorScript;
    DoorScript currentDoorScript;

    public List<Transform> pathToTake = new List<Transform>();

    Transform firstDoorTransform;
    Transform secondDoorTransform;
    Transform currentDoorTransform;

    public float speed = 10.0f;
    public float maxDistanceToPoint = 3.0f;
    public float damping = 1.0f;
    public float distanceToOpenDoor = 5.5f;

    public int current = 0;
    private int countdownNumber = 15;
    private int currentToStop = 3;
    private int countIncrament = 1;
    public int countDownToTable;

    public bool wait = false;
    public bool stopped = false;
    public bool stoppedAtTable = false;
    public bool startGame = false;
    public bool startTransition = false;
    public bool changeScenes = false;
    bool playedIntroClip = false;
    bool firstDoorOpened = false;

    // Use this for initialization
    void Start()
    {
        pathToTake = path.GetComponent<PathScripts>().nodes;
        clockScript = clock.GetComponent<ClockScript>();
        firstDoorScript = firstDoor.GetComponent<DoorScript>();
        secondDoorScript = secondDoor.GetComponent<DoorScript>();

        firstDoorTransform = firstDoor.GetComponent<Transform>();
        secondDoorTransform = secondDoor.GetComponent<Transform>();

        audioSource = GetComponent<AudioSource>();

        countDownToTable = pathToTake.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stoppedAtTable)
        {
            Walking();

            OpenDoors();
        }
        else
        {
            wait = true;
            if (startGame)
            {
                startGame = false;
                spawnner.GetComponent<IntroSpawnnerScript>().startGame = true;
                clockScript.clockStart = true;
            }
            
            if (clockScript.won)
            {
                startTransition = true;
                stoppedAtTable = false;
                wait = false;
                countIncrament = -countIncrament;
                currentToStop = 5;
                countDownToTable = 8;
                countdownNumber = 9;
            }
        }

        Looking();
        WalkingAnimation();

        if (changeScenes)
        {
            LoadNextLevel();
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
            else if (countDownToTable == 0)
            {
                stoppedAtTable = true;
                startGame = true;
            }
            else
            {
                current = (current + countIncrament) % pathToTake.Count;
                currentToStop = currentToStop - 1;
                countDownToTable = countDownToTable - 1;
            }

            if (currentToStop == 0)
            {
                wait = true;
                stopped = true;
                currentToStop = -1;
                if (!playedIntroClip)
                {
                    playedIntroClip = true;
                    audioSource.PlayOneShot(IntroClip);
                }
                else
                {
                    playedIntroClip = false;
                    audioSource.PlayOneShot(OutroClip);
                }
            }
        }
    }

    private void Looking()
    {
        if (!wait)
        {
            Vector3 lookPos = pathToTake[current].position - transform.position;
            lookPos.y = 0.0f;
            Quaternion theRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, theRotation, Time.deltaTime * damping);
        }
        else
        {
            if (!startTransition)
            {
                Vector3 lookPos = poleToLookAt.GetComponent<Transform>().position - transform.position;
                lookPos.y = 0.0f;
                Quaternion theRotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, theRotation, Time.deltaTime * damping);
            }
            else
            {
                Vector3 lookPos = poleToLookAt1.GetComponent<Transform>().position - transform.position;
                lookPos.y = 0.0f;
                Quaternion theRotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, theRotation, Time.deltaTime * damping);
            }
        }

    }

    private void OpenDoors()
    {
        if (!firstDoorOpened)
        {
            currentDoorScript = firstDoorScript;
            currentDoorTransform = firstDoorTransform;
        }
        else
        {
            if (Vector3.Distance(transform.position, firstDoorTransform.position) > distanceToOpenDoor)
            {
                currentDoorScript.openDoor = false;
                currentDoorScript = secondDoorScript;
                currentDoorTransform = secondDoorTransform;
            }
        }

        if (Vector3.Distance(transform.position, currentDoorTransform.position) < distanceToOpenDoor)
        {
            currentDoorScript.openDoor = true;
            firstDoorOpened = true;
        }
        else
        {
            currentDoorScript.openDoor = false;
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

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(2);
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
            if (startTransition) { changeScenes = true; }
        }
    }
}
