using UnityEngine;
using System.Collections;

//This holds the player variables like cash, extra tries, score, etc.
using System.Runtime.InteropServices;

public class PlayerScript : MonoBehaviour {
	public bool inArcadeMode;			//IMPORTANT! Changes the functions of a lot of functions if it is.
	public double totalMoney;			//How much cash the player has in total.
	public float moveSpeed;				//How fast the player moves in the stages
	public bool startSection;			//Is the player currently in a section and "playing" it
	public GameObject currentMinigame;	//the current minigame the player is currently at.
	public static PlayerScript thisInstance;	//Makes sure only one copy of this is in existance
	public AudioClip clearSection;			//The sound that plays when the player wins a section.

	private float origSpeed;				//Stores the original speed of the player.

	void Start () {
		origSpeed = moveSpeed;
	}

	//Prevents the player from getting destroyed when changing scenes.
	void Awake()
	{
		if(thisInstance)
			DestroyImmediate(gameObject);
		else
		{
			DontDestroyOnLoad(gameObject);
			thisInstance = this;
		}
	}
	
	void Update () {
		if(startSection == true)
			this.gameObject.transform.position += new Vector3(moveSpeed,0f,0f);
	}
		
	//When the player enters a zone that has a minigame, it activates the minigame. Or if the player collides into the goal, taken to the goal.
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Minigame")
		{
			GameObject gauge = GameObject.FindGameObjectWithTag("Gauge");
			currentMinigame = other.gameObject;
			currentMinigame.GetComponent<MinigameTrigger>().ChangeGaugeDifficulty();
			gauge.transform.GetChild(0).GetComponent<GaugeGame>().displayGame();
			moveSpeed = 0f;
		}
		if(other.tag == "Goal")
		{
			//Here the section bonus is tested.
			GameObject.Find("BGM_Stage").GetComponent<AudioSource>().PlayOneShot(clearSection);
			addCash(GameObject.Find("Section_System").GetComponent<SectionScript>().DetermineBonus());
			Invoke("GoToNextSection",2f);
		}
	}

	//Moves the player to the next section.
	void GoToNextSection()
	{
		startSection = false;
		GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().NextSection();
	}

	//Allows the player to move at the normal speed again
	public void resetSpeed()
	{
		moveSpeed = origSpeed;
	}

	//Takes away cash from the player. If the player is in the negatives, return true. Else, return false.
	public bool takeAwayCash(double amountLost)
	{
		totalMoney -= amountLost;
		if(totalMoney < 0)
			return false;
		
		GameObject.Find("HUD").GetComponent<SectionGUI>().isDisplayingReward = true;
		GameObject.Find("HUD").GetComponent<SectionGUI>().displayReward = -1 * amountLost;
		return true;
	}

	//Adds cash to the player, usually after winning a minigame
	public void addCash(double amountGained)
	{
		totalMoney += amountGained;
		currentMinigame = null;
		GameObject.Find("HUD").GetComponent<SectionGUI>().isDisplayingReward = true;
		GameObject.Find("HUD").GetComponent<SectionGUI>().displayReward = amountGained;
	}

	//Destroys the player. Called when the player moves on to the next stage.
	public void destroyPlayer()
	{
		Destroy(gameObject);
	}
}
