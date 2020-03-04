using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileScript : MonoBehaviour {

    public Text letterOnFile;

    public bool checkNumber = false;

    public int randomLetterNum;

	void Start ()
    {
        randomLetterNum = UnityEngine.Random.Range(0, 3);
        switch (randomLetterNum)
        {
            case 0:
                letterOnFile.text = "A";
                break;
            case 1:
                letterOnFile.text = "B";
                break;
            case 2:
                letterOnFile.text = "C";
                break;
        }
	}
}
