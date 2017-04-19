using UnityEngine;
using System.Collections;

//This is used to directly change the difficulty area of the gauge.

public class MinigameTrigger : MonoBehaviour {
	public float tickerSpeed;		//How fast will the ticker move? (0.1-0.9 is pretty fast while 0.01-0.09 is pretty slow.)
	public int numbOfWinZones;		//How many win zones do you want to have?
	public int gaugeType;			//0 = normal gauge; 1 = hit all zones; 2 = hit all zones on one pass.
	public float timeForGauge;		//How long does the player have in completing the game?
	public bool randomSizeZones;	//Do you want to have the zones have randomely determined sizes?
	public float sizeOfZone;		//The size of the zone, if you don't want to make it random.
	public float maxSizeOfZone;		//The largest size of the win zone. 			
	public float minSizeOfZone;		//The smallest size of the win zone.
	public bool canWinZoneMove;		//Can the win zones move?
	public float winZoneMoveSpeed;	//What speed are these zones moving at, if they can?
	public float bailAmount;		//How much money the player has to pay if they lose
	public float winAmount;			//How much money the player earns when they win

	private bool arcadeVersion;		//Changes the behavior of the minigame if the player's in arcade mode.

	//Checks if the player's in arcade mode, which then assigns this minigame trigger to be an arcade version or not.
	void Start()
	{
		if(GameObject.Find("Player") != null)
			arcadeVersion = GameObject.Find("Player").GetComponent<PlayerScript>().inArcadeMode;
	}


	//This method alters the gauge to make it harder/easier.
	public void ChangeGaugeDifficulty()
	{
		GameObject gauge = GameObject.FindGameObjectWithTag("Gauge");
		if(gaugeType == 2)
			gauge.GetComponent<GenerateWinZones>().generateWinZones(1,randomSizeZones, sizeOfZone, 
				maxSizeOfZone, minSizeOfZone, gaugeType, canWinZoneMove, winZoneMoveSpeed);
		else
			gauge.GetComponent<GenerateWinZones>().generateWinZones(numbOfWinZones,randomSizeZones, sizeOfZone, 
				maxSizeOfZone, minSizeOfZone, gaugeType, canWinZoneMove, winZoneMoveSpeed);
		
		gauge.transform.GetChild(0).GetComponent<GaugeGame>().changeSpeedTimeAndNumbOfZones(tickerSpeed,numbOfWinZones, timeForGauge, gaugeType);
	}

	//Used to regenerate win zones for gauge 3
	public void GenerateNewZonesForGauge3(int newWinZones)
	{
		GameObject gauge = GameObject.FindGameObjectWithTag("Gauge");
		gauge.GetComponent<GenerateWinZones>().generateWinZones(newWinZones,randomSizeZones, sizeOfZone, 
			maxSizeOfZone, minSizeOfZone,gaugeType, canWinZoneMove, winZoneMoveSpeed);
	}

	//This is called when the player has finished a minigame
	public void endMinigame(bool wasSucessful)
	{
		if(arcadeVersion == true)
		{
			if(wasSucessful == false)
				GameObject.Find("Player").GetComponent<PlayerScript>().addCash(0);
			else
				GameObject.Find("Player").GetComponent<PlayerScript>().addCash(winAmount);

			GameObject.Find("Player").GetComponent<PlayerScript>().resetSpeed();
			GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().destroyGeneratedTrigger();
		}
		else
		{
			if(wasSucessful == false)
			{
				//Insert some graph of the player losing cash
				bool canContinue = GameObject.Find("Player").GetComponent<PlayerScript>().takeAwayCash(bailAmount);
				if(canContinue == false)
					GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().goToGameOver();
			}
			else
			{
				//The player can continue playing the section...
				GameObject.Find("Player").GetComponent<PlayerScript>().addCash(winAmount);
				GameObject.Find("Section_System").GetComponent<SectionScript>().numbGamesSucceeded++;
			}

			GameObject.Find("Player").GetComponent<PlayerScript>().resetSpeed();
			Destroy(gameObject);
		}
	}

	//Sets the values of the trigger. Used in Arcade Mode.
	public void setValues(float tickerSpeed, int numbOfWinZones, bool randomSizes, float winZoneSize, float randMaxSize, float randMinSize, int gaugeType, bool zoneMove, float zoneSpeed, float timeToAct)
	{
		this.tickerSpeed = tickerSpeed;	
		this.numbOfWinZones = numbOfWinZones;
		this.gaugeType = gaugeType;			
		this.timeForGauge = timeToAct;		
		this.randomSizeZones = randomSizes;	
		this.sizeOfZone = winZoneSize;		
		this.maxSizeOfZone = randMaxSize;					
		this.minSizeOfZone = randMinSize;		
		this.canWinZoneMove = zoneMove;		
		this.winZoneMoveSpeed = zoneSpeed;	
		this.bailAmount = 100f;		
		this.winAmount = 100f;

		//Sets this new minigame trigger to be a child of Triggers
		if(GameObject.Find("Triggers") != null)
			gameObject.transform.parent = GameObject.Find("Triggers").transform;
	}
}
