using UnityEngine;
using System.Collections;

//Handles all of the GUI that occurs in the current section the player is in. Also displays the start of the new section

public class SectionGUI : MonoBehaviour {
	public int currentStageNumber = 0;		//The current stage/section number
	public float currentRentDue = 0;		//The current rent that's due in the stage
	public int currentSectionNumber = 0;	//The current section number
	public double displayReward;			//The current reward the player got in a minigame.
	public bool isDisplayingReward;			//Is the reward for the minigame beign shown?
	public bool inResults;					//Hides the section GUI if the player is in a results screen.

	public GUIStyle fontForRent = new GUIStyle();
	public GUIStyle fontForStage = new GUIStyle();
	public GUIStyle fontForMoney = new GUIStyle();
	public GUIStyle fontForObjective = new GUIStyle();
	public AudioClip winSound;				//What sound will play when the player wins a game?
	public AudioClip failSound;				//What sound will play when the player loses a game?

	private bool playingSound;
	private bool arcadeVersion;				//Changes what the GUI displays when the player's in arcade mode
	private Vector3 posOfRent;
	private Vector3 posOfStage;
	private Vector3 posOfMoney;

	// Use this for initialization
	void Start () {
		if(GameObject.Find("Player") != null)
			arcadeVersion = GameObject.Find("Player").GetComponent<PlayerScript>().inArcadeMode;

		posOfStage = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.GetChild(0).position);
		posOfRent = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.GetChild(1).position);
		posOfMoney = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(gameObject.transform.GetChild(2).position);

		//What the fonts looks like. Can be changed later...
		fontForRent.fontSize = 40;
		fontForRent.normal.textColor = Color.red;
		fontForStage.fontSize = 40;
		fontForStage.normal.textColor = Color.white;
		fontForMoney.fontSize = 40;
		fontForMoney.normal.textColor = Color.green;
		fontForObjective.fontSize = 40;
		fontForObjective.normal.textColor = Color.white;
	}

	//Checks what GUI is being displayed right now.
	void Update () {
		if(GameObject.Find("Player") != null)
		{
			if(GameObject.Find("Player").GetComponent<PlayerScript>().startSection == false && inResults == false)
			{
				if(Input.GetKeyUp("space") == true)
				{
					GameObject.Find("Player").GetComponent<PlayerScript>().startSection = true;
					displayHUD(false);
				}
			}
		}
	}

	//Draws the GUI for the cash, rent for stage, and current section.
	void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( Screen.width / 1600.0f, Screen.height / 900.0f, 1.0f ) );

		if(inResults == false)
		{
			if(GameObject.Find("Player") != null)
			{
				if(GameObject.Find("Player").GetComponent<PlayerScript>().startSection == true)
				{
					double playerMoney = GameObject.Find("Player").GetComponent<PlayerScript>().totalMoney;

					GUI.Label(new Rect(1380f, 735f, posOfMoney.x,posOfMoney.y),"$" + playerMoney,fontForMoney);
					GUI.Label(new Rect(1310f, 820f, posOfRent.x,posOfRent.y),"$" + currentRentDue,fontForRent);
					if(arcadeVersion == false)
					{
						GUI.Label(new Rect(0, 750f,posOfStage.x,posOfStage.y),"Stage " + currentStageNumber,fontForStage);
						GUI.Label(new Rect(0, 820f,posOfStage.x,posOfStage.y),"Section " + currentSectionNumber,fontForStage);
					}
					else
					{
						int numbOfGames = GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().numbGamesCreated;
						GUI.Label(new Rect(0, 750f,posOfStage.x,posOfStage.y),"Round " + currentStageNumber,fontForStage);
						GUI.Label(new Rect(0, 820f,posOfStage.x,posOfStage.y),"# Targets " + numbOfGames,fontForStage);
					}
				}
				else
				{
					float screenWidth = Screen.width/2f;
					float screenHeight = Screen.height/2f;

					if(arcadeVersion == false)
					{
						GUI.Label(new Rect(600f, 250f, screenWidth,screenHeight),"Stage " + currentStageNumber,fontForObjective);
						GUI.Label(new Rect(600f, 350f, screenWidth, screenHeight),"Rent To Pay: $" + currentRentDue,fontForObjective);
						GUI.Label(new Rect(600f, 450f, screenWidth, screenHeight),"Current Section: " + currentSectionNumber,fontForObjective);
						GUI.Label(new Rect(600f, 550f, screenWidth, screenHeight),"Hit space to proceed...",fontForObjective);
					}
					else
					{
						GUI.Label(new Rect(600f, 250f, screenWidth,screenHeight),"Round " + currentStageNumber,fontForObjective);
						GUI.Label(new Rect(600f, 350f, screenWidth, screenHeight),"Target Amount: $" + currentRentDue,fontForObjective);
						GUI.Label(new Rect(600f, 450f, screenWidth, screenHeight),"Hit space to proceed...",fontForObjective);
					}
				}
			}

			if(isDisplayingReward == true)
			{
				if(displayReward <= 0)
				{
					if(playingSound == false)
					{
						GameObject.Find("BGM_Stage").GetComponent<AudioSource>().PlayOneShot(failSound);
						playingSound = true;
					}
					GUI.Label(new Rect(250f,400f,100f,100f),"$" + displayReward.ToString(), fontForRent);
				}
				else
				{
					if(playingSound == false)
					{
						GameObject.Find("BGM_Stage").GetComponent<AudioSource>().PlayOneShot(winSound);
						playingSound = true;
					}
					GUI.Label(new Rect(250f,400f,100f,100f),"+$" + displayReward.ToString(), fontForMoney);
				}

				Invoke("hideRewardDisplay",0.5f);
			}
		}
	}

	//Called to hide the number amount that the player gets when completing a game.
	void hideRewardDisplay()
	{
		playingSound = false;
		isDisplayingReward = false;
	}

	//Hides or displays the main game HUD.
	public void displayHUD(bool isHiding)
	{
		for(int i = 0; i < gameObject.transform.childCount; ++i)
		{
			if(gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
			{
				if(isHiding == true)
					gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
				else
					gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
			}
		}
	}
}
