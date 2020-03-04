using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour {

    public GameObject Spawnner;
    public GameObject loseScreen;

    CallToSpawn spawwnerScript;

    public Canvas canvas;
    Renderer buttonRenderer;
    AudioSource audioSource;

    public Text MessageBoard;

    public Material MenuNotHighlighted;
    public Material MenuHighlighted;
    public AudioClip buttonclicked;

    public bool display = false;
    public bool lost = false;
    public bool highlighted = false;
    public bool selected = false;
    public bool clickable = true;
    public bool playOnce = true;

    public float loadLevelDelay = 1.0f;
    public int menuChoice = 0;

    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        if (!clickable)
        {
            spawwnerScript = Spawnner.GetComponent<CallToSpawn>();
            MessageBoard = loseScreen.GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update () {
        if (clickable)
        {
            if (highlighted)
            {
                buttonRenderer.material = MenuHighlighted;
            }
            else
            {
                buttonRenderer.material = MenuNotHighlighted;
            }

            if (selected)
            {
                Selected();
            }
        }
        else
        {
            buttonRenderer.material = MenuNotHighlighted;

            if (lost)
            {
                if (display) { MessageBoard.text = "You Lose"; }
                else
                {
                    clickable = true;
                    GetComponent<Renderer>().enabled = true;
                    canvas.enabled = true;
                }
            }
            else
            {
                if (display) { MessageBoard.text = "Crashed: " + spawwnerScript.crashedCars; }
                else
                {
                    GetComponent<Renderer>().enabled = false;
                    canvas.enabled = false;
                }
            }
        }
	}

    void Selected()
    {
        if (playOnce)
        {
            playOnce = false;
            audioSource.PlayOneShot(buttonclicked);
        }

        //Add invoke to switch between scenes
        switch (menuChoice)
        {
            case 0:
                Invoke("LoadMainMenu", loadLevelDelay);
                break;
            case 1:
                Invoke("LoadLevelOne", loadLevelDelay);
                break;
        }
    }

    private void LoadLevelOne()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (clickable) { highlighted = true; }
    }

    private void OnTriggerExit(Collider other)
    {
        highlighted = false;
    }
}
