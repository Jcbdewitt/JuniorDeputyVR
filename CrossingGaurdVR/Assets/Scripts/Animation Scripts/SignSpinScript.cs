using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignSpinScript : MonoBehaviour {

	void Update ()
    {
        transform.Rotate(Vector3.down * Time.deltaTime * 50);
    }
}
