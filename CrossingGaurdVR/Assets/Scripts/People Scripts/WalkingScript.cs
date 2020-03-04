using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WalkingScript : MonoBehaviour {

    public GameObject path;
    public GameObject body;

    public Canvas countdownTimer;

    public TextMesh countdownText;

    public GameObject countdownNewText;

    public Animator walkingAnimation;

    public Renderer bodyRenderer;

    public Material bodyColor1;
    public Material bodyColor2;
    public Material bodyColor3;
    public Material bodyColor4;
    public Material bodyColor5;
    public Material bodyColor6;

    public List<Transform> pathToTake = new List<Transform>();

    public float speed = 10.0f;
    public float maxDistanceToPoint = 3.0f;
    public float damping = 1.0f;

    public int current = 0;
    private int countdownNumber = 5;
    private int currentToStop = 3;
    private int countdownToDestroy;

    public bool debugNewText = false;
    public bool customerMode = false;
    public bool standingStill = false;
    public bool reverseFollowPath = false;
    public bool wait = false;
    public bool stopped = false;
    bool stoppedBefore = false;
    bool hit = false;

	// Use this for initialization
	void Start () {
        if (!standingStill)
        {
            pathToTake = path.GetComponent<PathScripts>().nodes;
            countdownToDestroy = pathToTake.Count - 1;
        }

        countdownTimer.enabled = false;
        countdownText.text = countdownNumber.ToString();

        if (reverseFollowPath)
        {
            current = pathToTake.Count - 1;
        }

        int randomBodyNumber = UnityEngine.Random.Range(1, 7);
        bodyRenderer = body.GetComponent<Renderer>();

        switch (randomBodyNumber)
        {
            case 1:
                bodyRenderer.material = bodyColor1;
                break;
            case 2:
                bodyRenderer.material = bodyColor2;
                break;
            case 3:
                bodyRenderer.material = bodyColor3;
                break;
            case 4:
                bodyRenderer.material = bodyColor4;
                break;
            case 5:
                bodyRenderer.material = bodyColor5;
                break;
            case 6:
                bodyRenderer.material = bodyColor6;
                break;
            default:
                Debug.Log("Random body color not set up right");
                break;
        }

        if (customerMode)
        {
            currentToStop = 7;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!standingStill)
        {
            Walking();

            Looking();

            if (wait == false)
            {
                walkingAnimation.SetInteger("Moving", 1);
            }
            else
            {
                walkingAnimation.SetInteger("Moving", 0);
            }

            if (gameObject.tag == "Hit Person" && hit == false)
            {
                hit = true;
                StartCoroutine(DestroyPerson());
            }
        }
        else
        {
            walkingAnimation.SetInteger("Moving", 0);
            countdownTimer.enabled = false;
        }

    }

    private void Walking()
    {
        if (wait)
        {
            if (!customerMode) { countdownTimer.enabled = true; }
            if (stopped)
            {
                StartCoroutine(Countdown());
                stopped = false;
            }
        }
        else
        {
            countdownTimer.enabled = false;

            if (Vector3.Distance(transform.position, pathToTake[current].position) > maxDistanceToPoint)
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, pathToTake[current].position, speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(pos);
            }
            else
            {
                if (!reverseFollowPath)
                {
                    current = (current + 1) % pathToTake.Count;
                    currentToStop = currentToStop - 1;
                    countdownToDestroy = countdownToDestroy - 1;
                }
                else
                {
                    current = (current - 1) % pathToTake.Count;
                    currentToStop = currentToStop - 1;
                    countdownToDestroy = countdownToDestroy - 1;
                }

            }

            if (countdownToDestroy < 0)
            {
                Destroy(gameObject);
            }

            if (currentToStop == 0 && !stoppedBefore)
            {
                wait = true;
                stopped = true;
                stoppedBefore = true;
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

    private IEnumerator Countdown()
    {
        countdownText.text = countdownNumber.ToString();
        yield return new WaitForSeconds(1.0f);
        countdownNumber -= 1;
        if (countdownNumber > 0)
        {
            StartCoroutine(Countdown());
        }
        else
        {
            if (customerMode) { GetComponent<CustomerScript>().spawnRandomFood = true; }
            wait = false;
        }
    }

    private IEnumerator DestroyPerson()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(0, 20), UnityEngine.Random.Range(0, 20), UnityEngine.Random.Range(0, 20));
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}