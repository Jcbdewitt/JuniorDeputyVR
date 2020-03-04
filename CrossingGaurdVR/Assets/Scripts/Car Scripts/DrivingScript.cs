using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour {

    Rigidbody rigidBody;

    public GameObject CarBase;
    public GameObject RightBlinker;
    public GameObject LeftBlinker;
    public GameObject Spawnner;

    public Renderer carBaseRend;
    Renderer rightBlinkerRend;
    Renderer leftBlinkerRend;

    public CallToSpawn callToSpawn;

    public Material StartBlinkerColor;
    public Material BlinkingBlinkerColor;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    private List<Transform> nodes = new List<Transform>();

    public Transform[] targetStart;
    public Transform[] targetTurn;
    public Transform[] targetEnd;
    public Transform[] target;
    public Transform path;

    public Vector3 centerOfMass;

    private float targetSteerAngle = 0;
    public float maxDistance = 1.0f;
    public float maxSteerAngle = 45f;
    public float turnSpeed = 5f;
    public float maxMotorTorque = 80f;
    public float maxBrakeTorque = 200f;
    public float currentSpeed = 0.1f;
    public float maxSpeed = 100f;
    float blinkerDelay = 0.5f;

    public int currentNode = 0;
    public int maxNodeNumber;

    public bool isBraking = false;
    public bool turn = false;
    public bool turnRight = false;
    public bool debugFroozen = false;
    public bool selected = false;
    public bool toBeDestroyed = false;
    public bool stoppedAble = false;

    private void Awake()
    {
        carBaseRend = CarBase.GetComponent<Renderer>();
        rightBlinkerRend = RightBlinker.GetComponent<Renderer>();
        leftBlinkerRend = LeftBlinker.GetComponent<Renderer>();
        rigidBody = GetComponent<Rigidbody>();
        callToSpawn = GetComponent<CallToSpawn>();
        rigidBody.centerOfMass = centerOfMass;
        Spawnner = GameObject.Find("Spawnner");
        callToSpawn = Spawnner.GetComponent<CallToSpawn>();
    }

    private void Start()
    {
        if (turn)
        {
            StartCoroutine(BlinkOn());

            target = new Transform[targetStart.Length + targetTurn.Length + targetEnd.Length];

            int pass = 0;
            for (int k = 0; k < targetStart.Length; k++)
            {
                target[pass] = targetStart[k];
                pass++;
            }
            for (int l = 0; l < targetTurn.Length; l++)
            {
                target[pass] = targetTurn[l];
                pass++;
            }
            for (int p = 0; p < targetEnd.Length; p++)
            {
                target[pass] = targetEnd[p];
                pass++;
            }

            if (turnRight)
            {
                maxNodeNumber = 6;
            }
            else
            {
                maxNodeNumber = 8;
            }
        }
        else
        {
            target = new Transform[(targetStart.Length + targetEnd.Length) ];

            maxNodeNumber = 4;

            int pass = 0;
            for (int g = 0; g < targetStart.Length ; g++)
            {
                target[pass] = targetStart[g];
                pass++;
            }
            for (int b = 0; b < targetEnd.Length; b++)
            {
                target[pass] = targetEnd[b];
                pass++;
            }


        }

        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] != transform)
            {
                nodes.Add(target[i]);
            }
        }
    }

    void FixedUpdate()
    {
        if (!debugFroozen)
        {
            ApplySteer();
            Drive();
            CheckWaypointDistance();
            LerpToSteerAngle();
            Braking();
            CanStop();
            if (gameObject.tag == "Destroyed Car" && toBeDestroyed == false)
            {
                toBeDestroyed = true;
                StartCoroutine(DestroyCar());
            }
        } 
    }

    //Definatly need to change tags dont need all the stupid extra tags now
    //might need to change this to ontriggerenter instead of collison
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Car" && 0 < currentNode && currentNode < maxNodeNumber)
        {
            gameObject.tag = "Destroyed Car";
            collision.gameObject.tag = "Destroyed Car";
            callToSpawn.increaseCrashedCars = true;
        }
        if (collision.gameObject.tag == "Person")
        {
            collision.gameObject.tag = "Hit Person";
            callToSpawn.increaseCrashedCars = true;
        }
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < maxDistance)
        {
            if (currentNode == nodes.Count - 1)
            {
                StartCoroutine(DestroyCar());
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
            wheelFL.brakeTorque = maxBrakeTorque;
            wheelFR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
        }
    }
    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private void CanStop()
    {
        int maxCheckNode;

        if (!turn)
        {
            maxCheckNode = 3;
        }
        else
        {
            maxCheckNode = 4;
        }

        if (0 < currentNode && currentNode < maxCheckNode)
        {
            stoppedAble = true;
        }
        else
        {
            stoppedAble = false;
        }
    }

    private IEnumerator BlinkOff()
    {
        rightBlinkerRend.material = StartBlinkerColor;
        leftBlinkerRend.material = StartBlinkerColor;

        yield return new WaitForSeconds(blinkerDelay);

        StartCoroutine(BlinkOn());
    }

    private IEnumerator BlinkOn()
    {
        if (turnRight) { rightBlinkerRend.material = BlinkingBlinkerColor; }
        else { leftBlinkerRend.material = BlinkingBlinkerColor; }

        yield return new WaitForSeconds(blinkerDelay);

        StartCoroutine(BlinkOff());
    }

    private IEnumerator DestroyCar()
    {
        float destroyDelay;

        if (toBeDestroyed)
        {
            rigidBody.velocity = new Vector3(UnityEngine.Random.Range(-10, 10), 20, UnityEngine.Random.Range(-10, 10));
            destroyDelay = 3.5f;
        }
        else
        {
            destroyDelay = 1.0f;
        }

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}