using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    AudioSource audioSource;

    public Animator doorAnimator;

    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    public bool onlyPlayOpenSound = false;
    public bool openDoor = false;
    bool playSounds = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (openDoor)
        {
            doorAnimator.SetInteger("DoorState", 1);
            if (playSounds)
            {
                playSounds = false;
                StartCoroutine(DoorSound());
            }
        }
        else
        { 
            doorAnimator.SetInteger("DoorState", 0);
            if (!playSounds)
            {
                playSounds = true;
                StartCoroutine(DoorSound());
            }
        }
    }

    private IEnumerator DoorSound() 
    {
        if (openDoor)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }
        else
        {
            if (onlyPlayOpenSound)
            {
                audioSource.PlayOneShot(doorOpenSound);
            }
            else
            {
                audioSource.PlayOneShot(doorCloseSound);
            }
        }
        yield return new WaitForSeconds(0.0f);
    }
}
