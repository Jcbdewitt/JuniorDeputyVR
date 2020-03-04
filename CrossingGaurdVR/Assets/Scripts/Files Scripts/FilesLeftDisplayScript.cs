using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilesLeftDisplayScript : MonoBehaviour {

    public GameObject clock;
    public GameObject display0;

    FileMenuButtonScript display0Script;

    public Text filesLeftDisplayText;

    public GameObject[] filesToDestroy;

    public int filesLeft = 45;

    public bool won = false;

    public bool endGame = false;
    public bool deleteAllFiles = true;

    private void Start()
    {
        display0Script = display0.GetComponent<FileMenuButtonScript>();
    }

    void Update ()
    {
        if (filesLeft <= 0)
        {
            won = true;
            endGame = true;
        }
        else
        {
            filesLeftDisplayText.text = filesLeft.ToString();
        }

        if(endGame)
        {
            if (won)
            {
                display0Script.clickable = true;
            }
            else
            {
                display0Script.clickable = true;
                display0Script.lost = true;
            }
        }
	}
}
