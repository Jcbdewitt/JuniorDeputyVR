using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWalkersScript : MonoBehaviour {

    public GameObject individualSpawnnerOne;
    public GameObject individualSpawnnerTwo;
    public GameObject individualSpawnnerThree;
    public GameObject individualSpawnnerFour;

    public int spawningLocatation;
    public float spawningInterval;

    public bool spawnOnStart = false;
    public bool startSpawning = false;
    public bool stopSpawnning = false;

    void Start()
    {
        if (spawnOnStart) { StartCoroutine(SpawnWalkers()); }
        
    }

    private void Update()
    {
        if (startSpawning && !spawnOnStart)
        {
            startSpawning = false;
            StartCoroutine(SpawnWalkers());
        }
    }

    private IEnumerator SpawnWalkers()
    {
        spawningLocatation = UnityEngine.Random.Range(1, 5);
        spawningInterval = UnityEngine.Random.Range(10, 21);
        switch (spawningLocatation)
        {
            case 1:
                individualSpawnnerOne.GetComponent<IndividualWalkerSpawnnerScript>().spawn = true;
                break;
            case 2:
                individualSpawnnerTwo.GetComponent<IndividualWalkerSpawnnerScript>().spawn = true;
                break;
            case 3:
                individualSpawnnerThree.GetComponent<IndividualWalkerSpawnnerScript>().spawn = true;
                break;
            case 4:
                individualSpawnnerFour.GetComponent<IndividualWalkerSpawnnerScript>().spawn = true;
                break;
        }
        yield return new WaitForSeconds(spawningInterval);
        if (!stopSpawnning) { StartCoroutine(SpawnWalkers()); }
    }
}
