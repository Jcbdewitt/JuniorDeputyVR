using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSpawnScript : MonoBehaviour {

    public GameObject pulledLever;

    public GameObject fileSpawnner0;
    public GameObject fileSpawnner1;
    public GameObject fileSpawnner2;
    public GameObject fileSpawnner3;
    public GameObject fileSpawnner4;

    FileSpawnnerScript fileSpawnner0Script;
    FileSpawnnerScript fileSpawnner1Script;
    FileSpawnnerScript fileSpawnner2Script;
    FileSpawnnerScript fileSpawnner3Script;
    FileSpawnnerScript fileSpawnner4Script;

    VRIO_PullLever pulledLeverScript;

    public int filesSpawnned = 0;

    public bool ableToSpawnMore = true;
    public bool spawn = false;
    public bool ignoreFirstBool = true;

	void Start ()
    {
        fileSpawnner0Script = fileSpawnner0.GetComponent<FileSpawnnerScript>();
        fileSpawnner1Script = fileSpawnner1.GetComponent<FileSpawnnerScript>();
        fileSpawnner2Script = fileSpawnner2.GetComponent<FileSpawnnerScript>();
        fileSpawnner3Script = fileSpawnner3.GetComponent<FileSpawnnerScript>();
        fileSpawnner4Script = fileSpawnner4.GetComponent<FileSpawnnerScript>();

        pulledLeverScript = pulledLever.GetComponent<VRIO_PullLever>();
	}

    void Update()
    {
        if (pulledLeverScript.pulled)
        {
            spawn = true;
        }

        if (spawn && ableToSpawnMore)
        {
            ableToSpawnMore = false;
            spawn = false;
            filesSpawnned = 5;
            fileSpawnner0Script.spawn = true;
            fileSpawnner1Script.spawn = true;
            fileSpawnner2Script.spawn = true;
            fileSpawnner3Script.spawn = true;
            fileSpawnner4Script.spawn = true;
        }

        if (filesSpawnned == 0)
        {
            spawn = false;
            ableToSpawnMore = true;
        }
    }
}
