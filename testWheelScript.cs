
using UnityEngine;
using System.Collections;

public class testWheelScript : MonoBehaviour
{

    enum wheelType { Steer, SteerAndMotor, Motor, JustAWheel }; //types of wheel
    wheelType typeOfWheel;	//Object of wheelType
    bool handBreakable = false;	//can apply handbrakes
    bool invertSteer = false; //invert the steer control
    bool braked = false;
    Transform wheelTransform;	//Mesh of the wheel
    float speedFactor;	//switch between steer angles
    WheelCollider wheelCollider;    //wheel collider attached to the same game object
    float mySidewayFriction;	//default value
    float myForwardFriction;	//default value
    float slipSidewayFriction;	//Custom value
    float slipForwardFriction;	//Custom value
    float decellarationSpeed = 30;
    float lowestSteerAtSpeed = 50;
    float lowSpeedSteerAngel = 10;
    float highSpeedSteerAngel = 1;
    float maxTorque = 50;
    float currentSpeed;
    float currentSteerAngel;
    float topSpeed = 150;
    float maxBrakeTorque = 100;
    float maxReverseSpeed = 50;
    RaycastHit hit;
    Vector3 wheelPos;


    //Start

    void Start()
    {
        wheelCollider = gameObject.GetComponent<WheelCollider>();
        myForwardFriction = wheelCollider.forwardFriction.stiffness;
        mySidewayFriction = wheelCollider.sidewaysFriction.stiffness;
        slipForwardFriction = 0.05f;
        slipSidewayFriction = 0.085f;
    }

    void Update()
    {
        wheelTransform.position = wheelPos;
        ReverseSlip();
        //Rotation Control
        wheelTransform.Rotate(wheelCollider.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        if (typeOfWheel == wheelType.Steer || typeOfWheel == wheelType.SteerAndMotor)
            wheelTransform.localEulerAngles.y = wheelCollider.steerAngle - wheelTransform.localEulerAngles.z;

        if (typeOfWheel == wheelType.Motor || typeOfWheel == wheelType.SteerAndMotor)
        {
            TorqueControle();
        }
        if (typeOfWheel == wheelType.Steer || typeOfWheel == wheelType.SteerAndMotor)
        {
            SteerControle();
        }
        if (handBreakable)
        {
            HandBrake();
        }
        if (!braked)
        {
            Decellaration();
        }
    }
    //Decellaration

    void Decellaration()
    {
        if (Input.GetButton("Vertical") == false)
        {
            wheelCollider.brakeTorque = decellarationSpeed;
        }
        else
        {
            wheelCollider.brakeTorque = 0;
        }
    }

    //Steer Control

    void SteerControle()
    {
        speedFactor = transform.parent.root.rigidbody.velocity.magnitude / lowestSteerAtSpeed;
        currentSteerAngel = Mathf.Lerp(lowSpeedSteerAngel, highSpeedSteerAngel, speedFactor);
        if (invertSteer)
            currentSteerAngel *= -Input.GetAxis("Horizontal");
        else
            currentSteerAngel *= Input.GetAxis("Horizontal");
        wheelCollider.steerAngle = currentSteerAngel;
    }



    //Torque Control

    void TorqueControle()
    {
        if (currentSpeed < topSpeed && currentSpeed > -maxReverseSpeed && !braked)
        {
            wheelCollider.motorTorque = maxTorque * Input.GetAxis("Vertical");
        }
        else
        {
            wheelCollider.motorTorque = 0;
        }
    }

    //Hand Brake
    
    void HandBrake()
    {
        if (braked)
        {
            if (currentSpeed > 1)
            {
                SetRearSlip(slipForwardFriction, slipSidewayFriction);
            }
            else if (currentSpeed < 0)
            {
                SetRearSlip(1, 1);
            }
            wheelCollider.brakeTorque = maxBrakeTorque;
            wheelCollider.motorTorque = 0;

        }
        else
        {
            wheelCollider.brakeTorque = 0;
            SetRearSlip(myForwardFriction, mySidewayFriction);
        }
    }
    
    //Reverse Slip
    
    void ReverseSlip()
    {
        if (currentSpeed < 0)
        {
            SetFrontSlip(slipForwardFriction, slipSidewayFriction);
        }
        else
        {
            SetFrontSlip(myForwardFriction, mySidewayFriction);
        }
    }
    
    //Slip Settings
    
    void SetRearSlip(float currentForwardFriction, float currentSidewayFriction)
    {
        if (typeOfWheel == wheelType.Motor || typeOfWheel == wheelType.SteerAndMotor && !braked)
        {
            wheelCollider.forwardFriction.stiffness = currentForwardFriction;
            wheelCollider.sidewaysFriction.stiffness = currentSidewayFriction;
            
        }
    }
    void SetFrontSlip(float currentForwardFriction, float currentSidewayFriction)
    {
        if (typeOfWheel == wheelType.Steer || typeOfWheel == wheelType.SteerAndMotor && !braked)
        {
            wheelCollider.forwardFriction.stiffness = currentForwardFriction;
            wheelCollider.sidewaysFriction.stiffness = currentSidewayFriction;
        }
    }
}

