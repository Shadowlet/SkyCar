using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject wings;

    public float maxTurnAngle = 10;
    public float maxTorque = 10;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public Transform wheelTransformFL;
    public Transform wheelTransformFR;
    public Transform wheelTransformRL;
    public Transform wheelTransformRR;

    public Transform car;
    public Vector3 centerOfMass = new Vector3(0, -0.9f, 0);
    private Rigidbody body;

    private Vector3 wheelRotation;
    private float topRotation = 45f;

    public float spoilerRatio = .1f;
    public float decelerationTorque = 30;

    public float topSpeed = 150000;
    private float currentSpeed;

    public float maxBrakeTorque = 100;
    private bool applyHandbrake = false;

    public float handBrakeForwardSlip = 0.04f;
    public float handBrakeSidewaySlip = 0.08f;

    public GameObject brakeLight;

    public Texture2D idleLightTex;
    public Texture2D brakeLightTex;
    public Texture2D reverseLightTex;

    private int numberOfGears;
    private float gearSpread;

    private bool isFlying = false;
    private float counter;

    private void Start()
    {
        counter = 0;
        body = GetComponent<Rigidbody>();
        body.centerOfMass += centerOfMass;
        gearSpread = topSpeed / numberOfGears;


        //--NOT FIXED--//
        wheelRotation = new Vector3(0, Input.GetAxis("Horizontal") * 2, 0);
    }

    private void Update()
    {
        UpdateWheelPositions();
        UpdateWheelRotation();

        float rotationThisFrame = 360 * Time.deltaTime;
        wheelTransformFL.Rotate(wheelFL.rpm / rotationThisFrame, 0, 0);
        wheelTransformFR.Rotate(wheelFR.rpm / rotationThisFrame, 0, 0);
        wheelTransformRL.Rotate(wheelRL.rpm / rotationThisFrame, 0, 0);
        wheelTransformRR.Rotate(wheelRR.rpm / rotationThisFrame, 0, 0);

        DetermineBrakeLightState();
        EngineSound();
    }

    private void EngineSound()
    {
        if(currentSpeed > 0)
        {
            if(currentSpeed > topSpeed)
            {
                GetComponent<AudioSource>().pitch = 1.75f;
            }
            else
            {
                GetComponent<AudioSource>().pitch = ((currentSpeed % gearSpread) / gearSpread) + 0.75f;
            }
        }
        else
        {
            //Reversing
        }
    }

    private void DetermineBrakeLightState()
    {
        if((currentSpeed > 0 && Input.GetAxis("Vertical") < 0) || (currentSpeed < 0 && Input.GetAxis("Vertical") > 0) || applyHandbrake)
        {
            brakeLight.GetComponent<Renderer>().material.mainTexture = brakeLightTex;
        } else if(currentSpeed < 0 && Input.GetAxis("Vertical") < 0)
        {
            brakeLight.GetComponent<Renderer>().material.mainTexture = reverseLightTex;
        }
        else
        {
            brakeLight.GetComponent<Renderer>().material.mainTexture = idleLightTex;
        }
    }

    private void UpdateWheelPositions()
    {
        WheelHit contact = new WheelHit();

        if(wheelFL.GetGroundHit(out contact))
        {
            Vector3 temp = wheelFL.transform.position;
            temp.y = (contact.point + (wheelFL.transform.right * wheelFL.radius)).y + .3f;
            wheelTransformFL.position = temp;
        }
        if (wheelFR.GetGroundHit(out contact))
        {
            Vector3 temp = wheelFR.transform.position;
            temp.y = (contact.point + (wheelFR.transform.right * wheelFR.radius)).y + .3f;
            wheelTransformFR.position = temp;
        }
        if (wheelRL.GetGroundHit(out contact))
        {
            Vector3 temp = wheelRL.transform.position;
            temp.y = (contact.point + (wheelRL.transform.right * wheelRL.radius)).y + .3f;
            wheelTransformRL.position = temp;
        }
        if (wheelRR.GetGroundHit(out contact))
        {
            Vector3 temp = wheelRR.transform.position;
            temp.y = (contact.point + (wheelRR.transform.right * wheelRR.radius)).y + .3f;
            wheelTransformRR.position = temp;
        }
    }
    
    private void UpdateWheelRotation()
    {
        if (wheelTransformFL.transform.eulerAngles.y > topRotation)
        {
            wheelTransformFL.transform.eulerAngles = wheelRotation;
            wheelTransformRL.transform.eulerAngles = wheelRotation;
        }
        

        if (wheelTransformFR.transform.eulerAngles.y > topRotation)
        {
            wheelTransformFR.transform.eulerAngles = wheelRotation;
            wheelTransformRR.transform.eulerAngles = wheelRotation;
        }
    }

    private void UpdateCarFlight()
    {
        
        if (currentSpeed < topSpeed)
        {
            currentSpeed += (currentSpeed + 2) - topSpeed;
            //body.velocity = new Vector3(currentSpeed, this.transform.position.y);
        }
        body.velocity = transform.forward * 20;

        //Vector3 flightRotation
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            car.transform.Rotate(Input.GetAxis("Vertical"), transform.rotation.y, Input.GetAxis("Horizontal"));
        }
        else
        {
            car.transform.Rotate(0, 0, 0);
        }
        
    }

    private void UpdateCarGround()
    {
        currentSpeed = wheelRL.radius * wheelRL.rpm * Mathf.PI * 0.12f;

        //Handbrake controls
        if (Input.GetButton("Jump"))
        {
            Debug.Log("Brake");
            applyHandbrake = true;
            wheelFL.brakeTorque = maxBrakeTorque;
            wheelFR.brakeTorque = maxBrakeTorque;

            //Wheels are locked, so power slide!
            if (GetComponent<Rigidbody>().velocity.magnitude > 1)
            {
                SetSlipValues(handBrakeForwardSlip, handBrakeSidewaySlip);
            }
            else
            {
                SetSlipValues(1f, 1f);
            }
        }
        else
        {
            applyHandbrake = false;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            SetSlipValues(1f, 1f);
        }

        if (currentSpeed < topSpeed)
        {
            //Rear wheel drive
            wheelRL.motorTorque = Input.GetAxis("Vertical") * maxTorque;
            wheelRR.motorTorque = Input.GetAxis("Vertical") * maxTorque;
        }
        else
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }


        //Spoilers add down pressure based on the car's speed
        Vector3 localVelocity = transform.InverseTransformDirection(body.velocity);
        body.AddForce(-transform.up * (localVelocity.z * spoilerRatio), ForceMode.Impulse);

        //Front wheel steering
        wheelFL.steerAngle = Input.GetAxis("Horizontal") * maxTurnAngle;
        wheelFR.steerAngle = Input.GetAxis("Horizontal") * maxTurnAngle;



        if (!applyHandbrake && ((Input.GetAxis("Vertical") <= -0.5f && localVelocity.z > 0) || (Input.GetAxis("Vertical") >= 0.5f && localVelocity.z < 0)))
        {
            wheelRL.brakeTorque = decelerationTorque + maxTorque;
            wheelRR.brakeTorque = decelerationTorque + maxTorque;
        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            wheelRL.brakeTorque = decelerationTorque;
            wheelRR.brakeTorque = decelerationTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }

    private void FixedUpdate()
    {
        /*if (isFlying == false)
        {
            UpdateCarGround();
        }
        else if (isFlying == true)
        {
            UpdateCarFlight();
        }*/

        UpdateCarFlight();
        //UpdateCarGround();
        

        
    }

    private void SetSlipValues(float forward, float sideways)
    {
        //Change the stiffness values of the wheel friction
        WheelFrictionCurve tempStruct = wheelRR.forwardFriction;
        tempStruct.stiffness = forward;
        wheelRR.forwardFriction = tempStruct;

        tempStruct = wheelRR.sidewaysFriction;
        tempStruct.stiffness = sideways;
        wheelRR.sidewaysFriction = tempStruct;

        tempStruct = wheelRL.forwardFriction;
        tempStruct.stiffness = forward;
        wheelRL.forwardFriction = tempStruct;

        tempStruct = wheelRL.sidewaysFriction;
        tempStruct.stiffness = sideways;
        wheelRL.sidewaysFriction = tempStruct;
    }

    private void SwitchMode(bool isInAir)
    {
        if (isInAir == true)
        {
            wings.gameObject.SetActive(true);
            body.useGravity = false;
        }
        if (isInAir == false)
        {
            wings.gameObject.SetActive(false);
            body.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider triggerBox)
    {
        if (triggerBox.gameObject.tag == "boxAir")
        {
            SwitchMode(false);
        }

        if (triggerBox.gameObject.tag == "boxGround")
        {
            SwitchMode(true);
        }
    }
}
