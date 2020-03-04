using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour {

    public GameObject spawnner;
    public GameObject carSpawnner;

    public Canvas clockCanvas;

    public Text clockText;

    private int minute = 3;
    private int tenth = 0;
    private int tenthReset = 5;
    private int first = 0;
    private int firstReset = 9;

    public bool won = false;
    public bool lost = false;
    public bool clockStart = false;
    public bool clockFinished = false;
    bool clockStarted = false;

    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
        clockCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update ()
    {
		if (clockStart && !clockStarted)
        {
            StartCoroutine(ClockTickDown());
            GetComponent<Renderer>().enabled = true;
            clockCanvas.enabled = true;
            clockStarted = true;
        }
        
        if (carSpawnner.GetComponent<CallToSpawn>().lost)
        {
            lost = true;
        }

        if (minute < 0)
        {
            clockText.text = "0:00";
        }

        if (clockFinished)
        {
            carSpawnner.GetComponent<CallToSpawn>().wonDeleteThings = true;
        }
	}

    private IEnumerator ClockTickDown()
    {
        clockText.text = minute.ToString() + ":" + tenth.ToString() + first.ToString();
        if (minute == 0 && tenth == 0 && first == 0)
        {
            clockFinished = true;
        }
        else if (tenth == 0 && first == 0)
        {
            minute -= 1;
            tenth = tenthReset;
            first = firstReset;
        }
        else if (first != 0)
        {
            first -= 1;
        }
        else if (tenth != 0 && first == 0)
        {
            tenth -= 1;
            first = firstReset;
        }
        yield return new WaitForSeconds(1.0f);
        if (!clockFinished && !lost && !won)
        {
            StartCoroutine(ClockTickDown());
        }
        else
        {
            if (!lost)
            {
                won = true;
            }
        }
    }
}
