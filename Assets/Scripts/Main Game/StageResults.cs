using UnityEngine;
using System.Collections;

//The player has to pay the rent in this scene. Else, it's a game over.
using UnityEngine.SceneManagement;

public class StageResults : MonoBehaviour {
	public bool hasSuceeded = false;		//Did the player succeed in the section?
	public GUIStyle fontForResults = new GUIStyle();

	private bool decided = false;			//Has the single calculation in Update finish?
	private float rentDue;					//Temp variable to store the rent.

	void Start()
	{
		GameObject.Find("Player").GetComponent<PlayerScript>().startSection = false;
		rentDue = GameObject.Find("HUD").GetComponent<SectionGUI>().currentRentDue;
		hasSuceeded = GameObject.Find("Player").GetComponent<PlayerScript>().takeAwayCash(rentDue);

		if(GameObject.Find("BGM_Stage"))
			Destroy(GameObject.Find("BGM_Stage"));

		//Temporary font stuff
		fontForResults.normal.textColor = Color.black;
		fontForResults.fontSize = 30;
	}

	//Sets the section GUI back to normal once the player leaves the results screen
	void OnDestroy()
	{
		if(GameObject.Find("HUD") != null)
			GameObject.Find("HUD").GetComponent<SectionGUI>().inResults = false;
	}

	void Update()
	{
		if(Input.GetKeyUp("space") == true && decided == false)
		{
			decided = true;
			if(hasSuceeded == false)
				GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().goToGameOver();
			else
				GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().NextStage();
		}
	}
		
	//The current rent that the player needs to pay off is tallied here.
	void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( Screen.width / 1600.0f, Screen.height / 900.0f, 1.0f ) );
	
		float screenWidth = Screen.width/2f;
		float screenHeight = Screen.height/2f;

		GUI.Label(new Rect(230f, 250f, screenWidth, screenHeight),"Pay up pal! You owe: $" + rentDue  + "!",fontForResults);
		GUI.Label(new Rect(230f, 750f, screenWidth, screenHeight),"Press space to continue...",fontForResults);

		if(hasSuceeded == false)
			GUI.Label(new Rect(230f, 450f, screenWidth, screenHeight),"AHA! You can't pay it off! Get out of here!",fontForResults);
		else
			GUI.Label(new Rect(230f, 450f, screenWidth, screenHeight),"You managed to pay off the rent!",fontForResults);
	}
}