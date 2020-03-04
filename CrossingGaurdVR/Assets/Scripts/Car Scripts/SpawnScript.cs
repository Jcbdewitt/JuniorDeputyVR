using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public GameObject Car;
    public GameObject SpawnedCar;

    DrivingScript spawnedCarScript;

    public Transform[] targetStartSpawn;
    public Transform[] targetLeftSpawn;
    public Transform[] targetRightSpawn;
    public Transform[] targetStraightEndSpawn;
    public Transform[] targetRightEndSpawn;
    public Transform[] targetLeftEndSpawn;

    public Material theCarBody;
    public Material theCarHighlightedBody;

    public Material carBody1;
    public Material carBody2;
    public Material carBody3;
    public Material carBody4;

    public Material carHighlightedBody1;
    public Material carHighlightedBody2;
    public Material carHighlightedBody3;
    public Material carHighlightedBody4;

    public bool debugSpawnRight = false;
    public bool debugSpawnLeft = false;
    public bool spawn = false;
    public bool unableToSpawn = false;
    public bool debugMode = false;

    int randomNum;
    int bodyChoice = 0;

    // Update is called once per frame
    void Update()
    {
        if (!debugMode)
        {
            if (spawn && !unableToSpawn)
            {
                spawn = false;
                StartCoroutine(PickCarColor());
                SpawnedCar = Instantiate(Car, transform.position, transform.rotation);
                spawnedCarScript = SpawnedCar.GetComponent<DrivingScript>();
                spawnedCarScript.carBaseRend.material = theCarBody;
                spawnedCarScript.targetStart = targetStartSpawn;
                randomNum = UnityEngine.Random.Range(1, 4);
                switch (randomNum)
                {
                    case 1:
                        spawnedCarScript.targetEnd = targetStraightEndSpawn;
                        break;
                    case 2:
                        spawnedCarScript.targetTurn = targetRightSpawn;
                        spawnedCarScript.targetEnd = targetRightEndSpawn;
                        spawnedCarScript.turn = true;
                        spawnedCarScript.turnRight = true;
                        break;
                    case 3:
                        spawnedCarScript.targetTurn = targetLeftSpawn;
                        spawnedCarScript.targetEnd = targetLeftEndSpawn;
                        spawnedCarScript.turn = true;
                        break;
                }
            }
        }
        else
        {
            if (spawn && !unableToSpawn)
            {
                spawn = false;
                SpawnedCar = Instantiate(Car, transform.position, transform.rotation);
                spawnedCarScript = SpawnedCar.GetComponent<DrivingScript>();
                spawnedCarScript.targetStart = targetStartSpawn;

                if (debugSpawnRight)
                {
                    spawnedCarScript.targetTurn = targetRightSpawn;
                    spawnedCarScript.targetEnd = targetRightEndSpawn;
                    spawnedCarScript.turn = true;
                    spawnedCarScript.turnRight = true;
                }
                else if (debugSpawnLeft)
                {
                    spawnedCarScript.targetTurn = targetLeftSpawn;
                    spawnedCarScript.targetEnd = targetLeftEndSpawn;
                    spawnedCarScript.turn = true;
                }
                else
                {
                    spawnedCarScript.targetEnd = targetStraightEndSpawn;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            unableToSpawn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            unableToSpawn = false;
        }
    }

    IEnumerator PickCarColor()
    {
        bodyChoice = UnityEngine.Random.Range(0, 4);

        switch (bodyChoice)
        {
            case 0:
                theCarBody = carBody1;
                theCarHighlightedBody = carHighlightedBody1;
                break;
            case 1:
                theCarBody = carBody2;
                theCarHighlightedBody = carHighlightedBody2;
                break;
            case 2:
                theCarBody = carBody3;
                theCarHighlightedBody = carHighlightedBody3;
                break;
            case 3:
                theCarBody = carBody4;
                theCarHighlightedBody = carHighlightedBody4;
                break;
            default:
                Debug.Log("yo wtf happened");
                break;
        }
        yield return new WaitForSeconds(0.0f);
    }
}
