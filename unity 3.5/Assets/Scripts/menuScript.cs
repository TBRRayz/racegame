using UnityEngine;
using System.Collections;

public class menuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = 50;
		if (GUI.Button(new Rect(600, 300, 150, 30), "start game" , myStyle))
		{
			Application.LoadLevel ("racetest1");
			
		}
	}

}
