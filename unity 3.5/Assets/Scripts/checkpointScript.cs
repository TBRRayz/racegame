using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class checkpointScript : MonoBehaviour {

    
    List<string> Position = new List<string>();
  
	float time;
	public static float roundPlayer = 0f;
	public static float roundrivalcar1 = 0f;
	public static float roundrivalcar2 = 0f;
	public static float roundrivalcar3 = 0f;


	bool timeOn = false;

	// Use this for initialization
	void Start () {

        
        
	}
	
	// Update is called once per frame
	void Update () {

       if (timeOn == true)
		{
			time += Time.deltaTime;
		}
        if (time >= 10)
		{
			Position.Clear();
			timeOn = false;
			time = 0;

		}

        
        
	
	}

    void OnTriggerEnter(Collider check)
    {
        if (check.gameObject.tag == "raceCar")
        {
			Position.Add(check.gameObject.name);
			timeOn = true;

            
        }
		if(check.gameObject.name == "you")
		{
			roundPlayer ++;
		}
		if(check.gameObject.name == "Car27")
		{
			roundrivalcar1 ++;
		}
		if(check.gameObject.name == "Car23")
		{
			roundrivalcar2 ++;
		}
		if(check.gameObject.name == "Car14")
		{
			roundrivalcar3 ++;
		}




    }

    void OnGUI()
    {
		GUI.Box(new Rect(10, 10, 150, 130), "stand");
        if (Position.Count > 0)
        {
            
            GUI.Label(new Rect(10, 30, 150, 30), "1 " + Position[0] + "");
            if (Position.Count > 1)
            {
                GUI.Label(new Rect(10, 50, 150, 30), "2 " + Position[1] + "");

                if (Position.Count > 2)
                {
                    GUI.Label(new Rect(10, 70, 150, 30), "3 " + Position[2] + "");

					if (Position.Count > 3)
					{
						GUI.Label(new Rect(10, 90, 150, 30), "4 " + Position[3] + "");
					}
                }


            }
        }
       
       
    }

}
