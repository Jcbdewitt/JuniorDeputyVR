using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualWalkerSpawnnerScript : MonoBehaviour {

    public GameObject Person;
    public GameObject pathRight;
    public GameObject pathLeft;

    public bool spawn = false;
    public bool spawnRight = false;
    public bool inverseRight = false;
    public bool inverseLeft = false;
    public bool disableSpawn = false;

    //figure out when to spawn on reversed path
    void Update () {
        if (spawn && !disableSpawn)
        {
            GameObject personSpawnned;
            WalkingScript personScript;

            spawn = false;
            personSpawnned = Instantiate(Person, transform.position, transform.rotation);
            personScript = personSpawnned.GetComponent<WalkingScript>();
            if (UnityEngine.Random.Range(0,2) == 0) { spawnRight = true; }
            else { spawnRight = false; }

            if (spawnRight)
            {
                personScript.path = pathRight;
                if (inverseRight) { personScript.reverseFollowPath = true; }
            }
            else
            {
                personScript.path = pathLeft;
                if (inverseLeft) { personScript.reverseFollowPath = true; }
            }
        }
	}
}
