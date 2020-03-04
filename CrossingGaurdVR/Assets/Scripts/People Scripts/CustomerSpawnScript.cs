using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnScript : MonoBehaviour {

    public GameObject person;
    public GameObject path;

    public GameObject rightDoor;
    public GameObject leftDoor;

    public List<GameObject> possibleSpawnedFood = new List<GameObject>();

    public float customerSpawnDelay = 25.0f;

    bool spawn = false;

    private void Start()
    {
        rightDoor = GameObject.Find("Gas Station Right Door");
        leftDoor = GameObject.Find("Gas Station Left Door");
        StartCoroutine(SpawnCustomer());
    }

    // Update is called once per frame
    void Update () {
		if (spawn)
        {
            GameObject customerSpawnned;

            spawn = false;
            customerSpawnned = Instantiate(person, transform.position, transform.rotation);
            customerSpawnned.GetComponent<CustomerScript>().customerMode = true;
            customerSpawnned.GetComponent<CustomerScript>().rightDoor = rightDoor;
            customerSpawnned.GetComponent<CustomerScript>().leftDoor = leftDoor;
            customerSpawnned.GetComponent<CustomerScript>().spawnedFood = possibleSpawnedFood[UnityEngine.Random.Range(0, possibleSpawnedFood.Count)];
            customerSpawnned.GetComponent<WalkingScript>().customerMode = true;
            customerSpawnned.GetComponent<WalkingScript>().path = path;
        }
	}

    private IEnumerator SpawnCustomer()
    {
        spawn = true;
        yield return new WaitForSeconds(customerSpawnDelay);
        StartCoroutine(SpawnCustomer());
    }
}
