using UnityEngine;
using System.Collections;
using System.Collections.Generic; //allows us to use lists

public class AICarScript : MonoBehaviour {
	public Vector3 CenterOfMass;
	public Transform pathGroup;
	public float maxSteer = 15.0f;
	public WheelCollider WheelFL;
	public WheelCollider WheelFR;
	public WheelCollider WheelRL;
	public WheelCollider WheelRR;
	public int currentPathObj;
	float distFromPath = 10f;
	public static float maxTorque = 0f;
	float currentSpeed;
	public float topSpeed = 100f;
	float decellationSpeed = 10f;
	float sensorLength = 5f;
	float frontSensorStartPoint = 2.5f;
	float frontSensorSideDist = 1f;
	float frontSensorAngle = 30f;
	float sidewaySensorLength = 4f;
    float avoidSpeed = 20f;
    bool reversing = false;
    float reverCounter = 0.0f;
    float waitToReverse = 2f;
    float reverFor = 2f;
	int flag = 0;
	//public float dir; //test variable
	
	private List<Transform> path; //we use a list so that it can have a dynamic size, meaning the size can change when we need it to
	
	void Start ()
	{
		rigidbody.centerOfMass = CenterOfMass;
		GetPath();
	}
	
	void GetPath ()
	{
		Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform>();
		path = new List<Transform>();
		
		for (int i = 0; i < path_objs.Length; i++)
		{
			if (path_objs[i] != pathGroup)
			{
				path.Add(path_objs[i]);
			}
		}
	}
	
	void Update ()
	{
		if (flag == 0)
		GetSteer();
		Move();
		Sensors();
	}
	
	void GetSteer ()
	{
		Vector3 steerVector = transform.InverseTransformPoint(new Vector3(path[currentPathObj].position.x, transform.position.y, path[currentPathObj].position.z));
		float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
		WheelFL.steerAngle = newSteer;
		WheelFR.steerAngle = newSteer;
		if (steerVector.magnitude <= distFromPath){
			currentPathObj++;
		if (currentPathObj >= path.Count)
				currentPathObj = 0;
		}
	}

	void Move ()
	{
		// bereken de snelheid die hij op dit moment heeft
		currentSpeed = 2*(22/7)*WheelRL.radius*WheelRL.rpm * 60 / 1000;
		currentSpeed = Mathf.Round (currentSpeed);
		// als de snelheid onder dat topspeed licht
		if (currentSpeed <= topSpeed){
			// als hij niet achteruit hoeft
            if (!reversing)
            {
                WheelRL.motorTorque = maxTorque;
                WheelRR.motorTorque = maxTorque;
            }
			// als hij wel achteruit moet
            else
            {
                WheelRL.motorTorque = -maxTorque;
                WheelRR.motorTorque = -maxTorque;
            }
		WheelRL.brakeTorque = 0;
		WheelRR.brakeTorque = 0;
		// als zijn snelheid boven de topspeed komt moet hij afremmen
		}else{
		WheelRL.motorTorque = 0;
		WheelRR.motorTorque = 0;
		WheelRL.brakeTorque = decellationSpeed;
		WheelRR.brakeTorque = decellationSpeed;

		}
	}

	void Sensors()
	{
		// Fornt mid

		RaycastHit hit;
		Vector3 pos;
		flag = 0;
		float avoidSenstivity = 0;
		// die positie van de raycast word bepaalt vanaf de auto
		pos = transform.position;
		pos += transform.forward*frontSensorStartPoint;
        // break sensor
        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {
			// als de raycast iets raakt behalve de onderdellen die een tag terrain hebben
            if (hit.transform.tag != "Terrain")
            {
                flag++;
                WheelRL.brakeTorque = decellationSpeed;
                WheelRR.brakeTorque = decellationSpeed;
                //Debug.Log("avoid");
                Debug.DrawLine(pos, hit.point, Color.white);
            }
        }
        else
        {
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;

        }

      
		// Front right
		// positie van de raycast word bepaald van af de auto
		pos = transform.position;
		pos += transform.right*frontSensorSideDist;
		if (Physics.Raycast(pos, transform.forward, out hit, sensorLength)){
			// als de raycast iets raakt behalve de onderdellen die een tag terrain hebben
			if (hit.transform.tag != "Terrain"){
			flag++;
			// de kracht waarmee hij opzij moet sturen
			avoidSenstivity -= 1.5f;
			Debug.DrawLine(pos,hit.point,Color.white);
		}
		}
		// side right
        else if (Physics.Raycast(pos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
			// als de raycast iets raakt behalve de onderdellen die een tag terrain hebben
            if (hit.transform.tag != "Terrain")
            {
                flag++;
				// de kracht waarmee hij opzij moet sturen
                avoidSenstivity -= 1f;
                Debug.DrawLine(pos, hit.point, Color.white);
            }
        }

		// Front left
		pos = transform.position;
		pos += transform.forward*frontSensorStartPoint;
		pos -= transform.right*frontSensorSideDist;
		if (Physics.Raycast(pos, transform.forward, out hit, sensorLength)){
			if (hit.transform.tag != "Terrain"){
				flag++;
				avoidSenstivity += 1.5f;
			Debug.DrawLine(pos,hit.point,Color.white);
			}
		}
		// side left
        else if (Physics.Raycast(pos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.transform.tag != "Terrain")
            {
                flag++;
                avoidSenstivity += 1f;
                Debug.DrawLine(pos, hit.point, Color.white);
            }
        }
		
        
        
        
        // right sideway
        if (Physics.Raycast(transform.position, transform.right, out hit, sidewaySensorLength))
        {
            if (hit.transform.tag != "Terrain")
            {
                flag++;
                avoidSenstivity -= 1f;
                Debug.DrawLine(transform.position, hit.point, Color.white);
            }
        }
		// left sideway
        if (Physics.Raycast(transform.position, -transform.right, out hit, sidewaySensorLength))
        {
            if (hit.transform.tag != "Terrain")
            {
                flag++;
                avoidSenstivity += 1f;
                Debug.DrawLine(transform.position, hit.point, Color.white);
            }
        }

        //front sensor
        pos = transform.position;
        pos += transform.forward * frontSensorStartPoint;
        if (avoidSenstivity == 0)
        {
            if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
            {
                if (hit.transform.tag != "Terrain")
                {


                    if (hit.normal.x < 0)
                    {
                        avoidSenstivity = -1;
                    }
                    else
                    {
                        avoidSenstivity = 1;
                    }
                }
            }

                //Debug.DrawLine(pos, hit.point, Color.white);
            }

        if (rigidbody.velocity.magnitude < 2 && !reversing)
        {
            reverCounter += Time.deltaTime;
            if (reverCounter >= waitToReverse)
            {
                reverCounter = 0;
                reversing = true;
            }
        }
        else if (!reversing)
        {
            reverCounter = 0;
        }
        if (reversing)
        {
            avoidSenstivity *= -1;
            reverCounter += Time.deltaTime;
            if (reverCounter >= reverFor)
            {
                reverCounter = 0;
                reversing = false;
            }
        }
		// als flag iets anders als nul is
		if (flag != 0)
		{
			// dan word avoidsteer aan geroepen mat de waarde avoidSenstivity
			AvoidSteer (avoidSenstivity);   
		}
	}
	// sensetivity is de waarde avoidsensitvity
	void AvoidSteer(float sensetivity)
	{
		// de auto stuurt de andere kant op van waar hij tegen aan rijdt
		WheelFL.steerAngle = avoidSpeed*sensetivity;
		WheelFR.steerAngle = avoidSpeed*sensetivity;

	}
}﻿