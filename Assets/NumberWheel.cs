using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberWheel : MonoBehaviour
{
    public float rotationTarget;
    public float speed;
    public bool rotating;
    public Quaternion destinationRotation;
    public float currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        speed = 7;
        currentRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (rotating)
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, destinationRotation, Time.deltaTime * speed);

            if (destinationRotation == transform.rotation)
            {
                rotating = false;
            }

        }

    }

    public void RotateWheel(int totalTries, int currentTry)
    {
        rotationTarget = 270 - (currentTry * (270 / totalTries));
        destinationRotation = Quaternion.AngleAxis(rotationTarget - currentRotation, Vector3.forward) * transform.rotation ;

        if (currentRotation != rotationTarget)
        {

            currentRotation = currentRotation + rotationTarget - currentRotation;
        }

        rotating = true;

    }
}
