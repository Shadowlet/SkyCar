using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    public Transform car;
    public float distance;
    public float height;
    public float rotationDamping = 3;
    public float heightDamping = 2;
    private float desiredAngle = 0;

    private void FixedUpdate()
    {
        desiredAngle = car.eulerAngles.y;

        //if the car is going backwards, add 100 to the wanted rotation.
        Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);
        if(localVelocity.z < -.5f)
        {
            desiredAngle += 180;
        }
    }

    private void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        //Determine where we want to be.
        
        float desiredHeight = car.position.y + height;

        //Now move towards our goals.
        currentAngle = Mathf.LerpAngle(currentAngle, desiredAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, desiredHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        //Set our new positions.
        Vector3 finalPosition = car.position - (currentRotation * Vector3.forward * distance);
        finalPosition.y = currentHeight;
        transform.position = finalPosition;
        transform.LookAt(car);
    }
}
