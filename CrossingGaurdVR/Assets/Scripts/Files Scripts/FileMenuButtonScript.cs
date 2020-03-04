using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileMenuButtonScript : MonoBehaviour {

    public Canvas canvas;

    public Text text;

    Renderer buttonRenderer;
    AudioSource audioSource;

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

    // Use this for initialization
    void Start () {
        buttonRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (clickable)
        {
            buttonRenderer.enabled = true;
            canvas.enabled = true;
            if (lost)
            {
                text.text = "You Lose";
            }
            else
            {
                text.text = "You Win";
            }
            Invoke("LoadMainMenu", 5.0f);
        }
        else
        {
            buttonRenderer.enabled = false;
            canvas.enabled = false;
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

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
