using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderStopScript : MonoBehaviour {

    public List<GameObject> carsToStop = new List<GameObject>();

    public GameObject vrHead;
    public GameObject carToStop;
    public GameObject connectedController;
    private GameObject nearestCar;

    public Transform vrHeadTransform;

    public bool colliedWithACar = false;
    bool colliededWithACar = false;
    bool leftCar = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        vrHeadTransform = vrHead.GetComponent<Transform>();
    }

    private void Update()
    {
        if (connectedController.GetComponent<ControllerScript>().detectCollisons)
        {
            DetectStops();
        }
        else
        {
            if (carsToStop.Count > 0)
            {
                for (int i = 0; i < carsToStop.Count; i++)
                {
                    carsToStop[i].GetComponent<DrivingScript>().isBraking = false;
                }
                carsToStop.Clear();
            }
        }
    }

    private void DetectStops()
    {
        if (colliededWithACar)
        {
            
            for (int i = 0; i < carsToStop.Count; i++)
            {
                if (i == 0)
                {
                    nearestCar = carsToStop[0];
                }
                else
                {
                    if (Vector3.Distance(vrHeadTransform.position, nearestCar.GetComponent<Transform>().position) > Vector3.Distance(vrHeadTransform.position, carsToStop[i].GetComponent<Transform>().position))
                    {
                        nearestCar = carsToStop[i];
                    }
                }
            }
            
            if (nearestCar.GetComponent<DrivingScript>().stoppedAble)
            {
                nearestCar.GetComponent<DrivingScript>().isBraking = true;
            }
            
        }
        else
        {
            if (leftCar)
            {
                for (int i = 0; i < carsToStop.Count; i++)
                {
                    carsToStop[i].GetComponent<DrivingScript>().isBraking = false;
                }
                carsToStop.Clear();
                leftCar = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            carsToStop.Add(other.gameObject);
            colliededWithACar = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Car")
        {
            colliededWithACar = false;
            leftCar = true;
        }
    }
}
