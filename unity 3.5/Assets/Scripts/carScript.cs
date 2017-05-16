using UnityEngine;
using System.Collections;

public class carScript : MonoBehaviour {

    public Vector3 CenterOfMass;
	// Use this for initialization
	void Start () {

        rigidbody.centerOfMass = CenterOfMass;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
