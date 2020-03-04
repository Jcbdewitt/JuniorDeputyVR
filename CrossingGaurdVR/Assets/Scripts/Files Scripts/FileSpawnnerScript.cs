using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSpawnnerScript : MonoBehaviour {

    public GameObject File;
    public GameObject spawnnedFile;

    public bool spawn = false;
	
	void Update ()
    {
		if (spawn)
        {
            spawn = false;
            spawnnedFile = Instantiate(File, transform.position, transform.rotation);
        }
	}
}
