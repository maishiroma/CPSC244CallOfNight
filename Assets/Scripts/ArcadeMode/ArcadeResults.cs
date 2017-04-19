using UnityEngine;
using System.Collections;

//Either congragulates the player for surviving, or displays the round totals if they failed. Similar to StageResults

public class ArcadeResults : MonoBehaviour {
	public bool hasSuceeded = false;		//Did the player succeed in the section?
	public GUIStyle fontForResults = new GUIStyle();

	private bool decided = false;			//Has the single calculation in Update finish?
	private float rentDue;					//Temp variable to store the rent.

	// Use this for initialization
	void Start () {
		rentDue = GameObject.Find("HUD").GetComponent<SectionGUI>().currentRentDue;
		GameObject.Find("Player").GetComponent<PlayerScript>().startSection = false;

		if(rentDue <= GameObject.Find("Player").GetComponent<PlayerScript>().totalMoney)
			hasSuceeded = true;
		else
			hasSuceeded = false;

		//Temporary font stuff
		fontForResults.fontSize = 30;
		fontForResults.normal.textColor = Color.black;
	}

	//Sets the section GUI back to normal once the player leaves the results screen
	void OnDestroy()
	{
		if(GameObject.Find("HUD") != null)
			GameObject.Find("HUD").GetComponent<SectionGUI>().inResults = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("space") == true && decided == false)
		{
			decided = true;
			if(hasSuceeded == false)
				GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().goToGameOver();
			else
			{
				GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().increaseDifficulty();
				GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().NextSection();
			}
		}
	}

	//The current rent that the player needs to pay off is tallied here.
	void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( Screen.width / 1600.0f, Screen.height / 900.0f, 1.0f ) );
		float screenWidth = Screen.width/2f;
		float screenHeight = Screen.height/2f;
		int resultRounds = GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().numbRounds - 1;
		double resultCash = GameObject.Find("Player").GetComponent<PlayerScript>().totalMoney;

		GUI.Label(new Rect(230f, 250f, screenWidth, screenHeight), "You reached a DrumpfPoint! Do you have $" + rentDue  + "?",fontForResults);
		GUI.Label(new Rect(230f, 650f, screenWidth,screenHeight),"You've survived " + resultRounds + " rounds and made $" + resultCash + "!", fontForResults);
		GUI.Label(new Rect(230f, 750f, screenWidth, screenHeight),"Press space to continue...",fontForResults);

		if(hasSuceeded == false)
			GUI.Label(new Rect(230f, 450f, screenWidth, screenHeight),"Oh no! You don't have enough! You got booted out!",fontForResults);
		else
			GUI.Label(new Rect(230f, 450f, screenWidth, screenHeight),"You impressed the DrumpfPoint! Continue on!",fontForResults);
	}
}
