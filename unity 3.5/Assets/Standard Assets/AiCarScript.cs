using UnityEngine;
using System.Collections;
using System.Collections.Generic; //allows us to use lists

public class AiCarScript : MonoBehaviour {

	public Vector3 CenterOfMass;
	public Transform pathGroup;
	public float maxSteer = 15.0f;
	public WheelCollider WheelFL;
	public WheelCollider WheelFR;
	public WheelCollider WheelRL;
	public WheelCollider WheelRR;
	public int currentPathObj;
	float distFromPath = 5;
	float maxTorque = 50;
	float currentSpeed;
	float topSpeed = 100;
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
		GetSteer();
		Move();
	}
	
	void GetSteer ()
	{
		Vector3 steerVector = transform.InverseTransformPoint(new Vector3(path[currentPathObj].position.x, transform.position.y, path[currentPathObj].position.z));
		float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
		//dir = steerVector.x / steerVector.magnitude;
		WheelFL.steerAngle = newSteer;
		WheelFR.steerAngle = newSteer;

		if (steerVector.magnitude  <= distFromPath){
			currentPathObj++;
		if (currentPathObj >= path.Count)
				currentPathObj = 0;
		}

	}
	void Move ()
	{
		WheelRL.motorTorque = maxTorque;
		WheelRR.motorTorque = maxTorque;
	}
}﻿