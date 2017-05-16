using UnityEngine;
using System.Collections;

public class MoveCar : MonoBehaviour {

    public enum wheelType { Steer, SteerAndMotor, Motor, JustAWheel }; //types of wheel
    public wheelType typeOfWheel;	//Object of wheelType
	public static float MoterForce;
	public float SteerForce;
	//public WheelCollider WheelColFL;
	//public WheelCollider WheelColFR;
	//public WheelCollider WheelColRR;
	//public WheelCollider WheelColRL;
    public WheelCollider wheelCollider;
    public Transform wheelTransform;
	float currentSpeed;
	public float topSpeed = 115f;
   
    
    
	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () {

		currentSpeed = 2*(22/7)*wheelCollider.radius*wheelCollider.rpm * 60 / 1000;
		currentSpeed = Mathf.Round (currentSpeed);
		if (currentSpeed >= topSpeed){

			MoterForce = 0;

		}else{

			MoterForce = 40;
		
		}

			
		

        WheelPosition();
        wheelTransform.Rotate(wheelCollider.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        float v = Input.GetAxis("Vertical") * MoterForce;
        float h = Input.GetAxis("Horizontal") * SteerForce;



		wheelTransform.localEulerAngles = new Vector3(wheelTransform.localEulerAngles.x, wheelCollider.steerAngle - wheelTransform.localEulerAngles.z, wheelTransform.localEulerAngles.z);
        if (typeOfWheel == wheelType.Motor)
        {
            wheelCollider.motorTorque = v;
        }
        if (typeOfWheel == wheelType.Steer)
        {
            wheelCollider.steerAngle = h;
            
        }

	}
    void WheelPosition()
    {
        RaycastHit hit;
        Vector3 wheelPos;
       
        if (Physics.Raycast(transform.position, -transform.up, out hit, wheelCollider.radius + wheelCollider.suspensionDistance))
        {
            wheelPos = hit.point + transform.up * wheelCollider.radius;
        }
        else
        {
            wheelPos = transform.position - transform.up * wheelCollider.suspensionDistance;
        }
        wheelTransform.position = wheelPos;
    }
   
}


