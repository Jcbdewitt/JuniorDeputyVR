using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerScript : MonoBehaviour {

    public GameObject path;
    public GameObject firstDoor;
    public GameObject poleToLookAt;
    public GameObject clock;

    public Animator walkingAnimation;

    public AudioSource audioSource;

    public AudioClip IntroClip;

    public DoorScript firstDoorScript;

    public List<Transform> pathToTake = new List<Transform>();

    Transform firstDoorTransform;

    public float speed = 5.0f;
    public float maxDistanceToPoint = 0.3f;
    public float damping = 1.0f;

    public int current = 0;
    private int countdownNumber = 6;
    private int currentToStop = 2;
    private int countIncrament = 1;
    public int countDownToTable;

    public bool wait = false;
    public bool stopped = false;
    public bool stoppedAtTable = false;
    public bool moving = false;
    bool playedIntroClip = false;
    public bool startGame = false;

    void Start ()
    {
        pathToTake = path.GetComponent<PathScripts>().nodes;
        firstDoorScript = firstDoor.GetComponent<DoorScript>();
        firstDoorTransform = firstDoor.GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();

        countDownToTable = pathToTake.Count - 1;
    }
	
	void Update ()
    {
        Walking();
        OpenDoors();
        Looking();
        WalkingAnimation();
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
            else
            {
                current = (current + countIncrament) % pathToTake.Count;
                currentToStop = currentToStop - 1;
                countDownToTable = countDownToTable - 1;
            }

            if (currentToStop == 0)
            {
                if (stoppedAtTable)
                {
                    moving = false;
                    Destroy(gameObject);
                }
                wait = true;
                stopped = true;
                stoppedAtTable = true;
                currentToStop = 1;
                if (!playedIntroClip)
                {
                    playedIntroClip = true;
                    audioSource.PlayOneShot(IntroClip);
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
            Vector3 lookPos = poleToLookAt.GetComponent<Transform>().position - transform.position;
            lookPos.y = 0.0f;
            Quaternion theRotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, theRotation, Time.deltaTime * damping);
        }

    }

    private void OpenDoors()
    {
        if (moving == true)
        {
            firstDoorScript.openDoor = true;
        }
        else
        {
            firstDoorScript.openDoor = false;
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
        }
    }
}
