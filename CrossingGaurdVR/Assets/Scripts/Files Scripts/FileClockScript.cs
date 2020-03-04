 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileClockScript : MonoBehaviour {

    public GameObject filesLeft;

    public Text clockText;

    public int minute = 3;
    private int tenth = 0;
    private int tenthReset = 5;
    private int first = 0;
    private int firstReset = 9;

    public bool clockFinished = false;
    public bool lost = false;
    public bool won = false;

    void Start ()
    {
        StartCoroutine(ClockTickDown());
    }
	
	void Update ()
    {
        if (minute < 0)
        {
            clockText.text = "0:00";
            lost = true;
        }

        if (clockFinished && lost)
        {
            //What to do once timer is done
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
        if (!clockFinished && !won)
        {
            StartCoroutine(ClockTickDown());
        }
        else
        {
            if (!won)
            {
                lost = true;
            }
        }
    }
}
