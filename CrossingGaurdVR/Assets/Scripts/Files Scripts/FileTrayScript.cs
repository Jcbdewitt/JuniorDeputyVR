using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileTrayScript : MonoBehaviour {

    public GameObject spawnner;
    public GameObject filesLeftDisplay;
    public GameObject connectedFile;

    AudioSource audioSource;

    public AudioClip rightFileAudio;
    public AudioClip wrongFileAudio;

    public ParticleSystem rightFile;
    public ParticleSystem wrongFile;

    public bool playParticle = false;
    public bool fileInTray = false;
    public bool rightFileInTray = false;

    public int letterCheck = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update ()
    {
		if (fileInTray)
        {
            if (connectedFile.GetComponent<FileScript>().randomLetterNum == letterCheck)
            {
                rightFileInTray = true;
            }
            else
            {
                rightFileInTray = false;
            }

            if (playParticle)
            {
                if (rightFileInTray)
                {
                    rightFile.Play();
                    audioSource.PlayOneShot(rightFileAudio);
                    spawnner.GetComponent<FileSpawnScript>().filesSpawnned -= 1;
                    filesLeftDisplay.GetComponent<FilesLeftDisplayScript>().filesLeft -= 1;
                    Destroy(connectedFile.gameObject);

                }
                else
                {
                    wrongFile.Play();
                    audioSource.PlayOneShot(wrongFileAudio);
                    spawnner.GetComponent<FileSpawnScript>().filesSpawnned -= 1;
                    filesLeftDisplay.GetComponent<FilesLeftDisplayScript>().filesLeft += 5;
                    Destroy(connectedFile.gameObject);
                }
                playParticle = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "File")
        {
            connectedFile = other.gameObject;
            fileInTray = true;
            playParticle = true;
        }
    }
}
