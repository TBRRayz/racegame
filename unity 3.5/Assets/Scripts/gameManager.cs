using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {


	float timer = 5f;
	bool timerOn = true;
	float round;
	bool win = false;
	bool lose = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer < 1 )
		{
			AICarScript.maxTorque = 35;
			MoveCar.MoterForce = 40;

		}
		if (timer < 0)
		{
			timerOn = false;
		}
		if (checkpointScript.roundPlayer == 1)
		{
			round = 1;

		}
		if (checkpointScript.roundPlayer == 4)
		{
			round = 2;
		}
		if (checkpointScript.roundPlayer == 7)
		{
			round = 3;
		}
		if (checkpointScript.roundPlayer == 10)
		{
			win = true;
			AICarScript.maxTorque = 0;
			MoveCar.MoterForce = 0;

		}
		if (checkpointScript.roundrivalcar1 == 10)
		{
			lose = true;
			AICarScript.maxTorque = 0;
			MoveCar.MoterForce = 0;
			
		}
		if (checkpointScript.roundrivalcar2 == 10)
		{
			lose = true;
			AICarScript.maxTorque = 0;
			MoveCar.MoterForce = 0;
			
		}
		if (checkpointScript.roundrivalcar3 == 10)
		{
			lose = true;
			AICarScript.maxTorque = 0;
			MoveCar.MoterForce = 0;
			
		}




	
	}
	void OnGUI (){

		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = 50;
		if (timerOn == true)
		{
		GUI.Label(new Rect(600, 100, 150, 30), "" + (int)timer + "", myStyle);
		}
		GUI.Label(new Rect(600, 10, 150, 30), "" + round + "/3", myStyle);
		if (win == true)
		{
			GUI.Label(new Rect(600, 100, 150, 30), "je hebt gewonnen" , myStyle);

			if(GUI.Button(new Rect(600, 200, 150, 30), "restart" , myStyle))
			{
				Application.LoadLevel (Application.loadedLevel);
				checkpointScript.roundrivalcar1 = 0;
				checkpointScript.roundrivalcar2 = 0;
				checkpointScript.roundrivalcar3 = 0;
				checkpointScript.roundPlayer = 0;
				AICarScript.maxTorque = 0;
				MoveCar.MoterForce = 0;
				round = 0;
				timer = 5f;
				timerOn = true;
				win = false;

			}
			
		}
		if (lose == true)
		{
			GUI.Label(new Rect(600, 100, 150, 30), "je hebt verloren" , myStyle);

			if(GUI.Button(new Rect(600, 200, 150, 30), "restart" , myStyle))
			{

				Application.LoadLevel (Application.loadedLevel);
				checkpointScript.roundrivalcar1 = 0;
				checkpointScript.roundrivalcar2 = 0;
				checkpointScript.roundrivalcar3 = 0;
				checkpointScript.roundPlayer = 0;
				AICarScript.maxTorque = 0;
				MoveCar.MoterForce = 0;
				round = 0;
				timer = 5f;
				timerOn = true;
				lose = false;


			}
			
		}

	}

}
