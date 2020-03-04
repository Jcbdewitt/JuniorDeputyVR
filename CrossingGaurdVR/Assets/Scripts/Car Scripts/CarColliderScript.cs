using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EDIT THIS CLEAN UP

public class CarColliderScript : MonoBehaviour {

    public DrivingScript attachedCar;
    DrivingScript carCollidingWith;

    MeshRenderer cubeMesh;

    bool collidedWithCar = false;
    bool collidedWithCollider = false;

    private void Start()
    {
        cubeMesh = GetComponent<MeshRenderer>();

        cubeMesh.enabled = !cubeMesh.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            carCollidingWith = other.GetComponent<DrivingScript>();
            if (carCollidingWith.debugFroozen)
            {
                attachedCar.isBraking = true;
                collidedWithCar = true;
            }
            else if (carCollidingWith.currentNode < carCollidingWith.targetStart.Length)
            {
                attachedCar.isBraking = true;
                collidedWithCar = true;
            }
            
        }
        /*
        if (other.tag == "Right collider")
        {
            Debug.Log("collided with collidercube");
            attachedCar.gameObject.tag = "Selected Car";
            attachedCar.selected = true;
            collidedWithCollider = true;
        }
        else if (other.tag == "Left collider")
        {
            attachedCar.gameObject.tag = "Left Selected Car";
            attachedCar.selected = true;
            collidedWithCollider = true;
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidedWithCar)
        { 
            attachedCar.isBraking = false;
            collidedWithCar = false;
        }
        /*
        if (collidedWithCollider)
        {
            attachedCar.selected = false;
            collidedWithCollider = false;
        }
        */
    }
}
