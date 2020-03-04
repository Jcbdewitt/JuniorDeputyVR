using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicScript : MonoBehaviour {
    private static MusicScript instance = null;
    public static MusicScript Instance
    {
        get { return instance; }
    }
	void Awake ()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(2);
        }
    }
}
