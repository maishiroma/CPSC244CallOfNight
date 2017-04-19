using UnityEngine;
using System.Collections;

// Generates the minigame triggers for Arcade mode as well as randomly creating minigame difficulties

public class ArcadeMinigameTriggerGenerator : MonoBehaviour {
	public int numbGamesCreated;			//The number of minigames the generator has made.
	public int numbRounds = 1;				//The number of rounds that the player is on
	public int rentRequirement;				//The rent requirement for the current round.
	public int rentIncrementor;				//How much will the rent will increase per round?

	public GameObject generatedTrigger;		//The current minigame that the script has created.
	public GameObject trigger;				//The template that the minigame trigger will use
	public Sprite[] minigameDisplay = new Sprite[3];			//stores all of the sprites used to display the minigame

	private bool createdTrigger = false;	//Did the generator create a minigame?

	//The rest of these variables are identical to what's found in MinigameTrigger
	public float tickerSpeed;
	public float maxSizeZone;
	public float minSizeZone;
	public bool canZoneMove;
	public float minZoneSpeed;
	public float maxZoneSpeed;

	// Update is called once per frame
	void Update () {
		if(GameObject.Find("Player") != null)
		{
			if(GameObject.Find("Player").GetComponent<PlayerScript>().startSection == true)
			{
				if(createdTrigger == false)
				{
					createdTrigger = true;
					Invoke("GenerateTrigger",1f);
				}
			}
		}
	}

	//Generates the generatedTrigger and sends it out to the player.
	void GenerateTrigger()
	{
		if(numbGamesCreated >= 10)	//Every 10 minigames, the player is taken to the results screen to be evaluated.
		{
			createdTrigger = false;
			numbRounds++;
			numbGamesCreated = 0;
			GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().arcadeRoundResults();
		}
		else if(generatedTrigger == null)
		{
			int gaugeType = Random.Range(0,3);
			int numbWinZones = Random.Range(1,5);
			float zoneSpeed = Random.Range(minZoneSpeed,maxZoneSpeed);
			int temp = Random.Range(1,11);

			if(temp <= 5)
			{
				canZoneMove = true;
				zoneSpeed = Random.Range(minZoneSpeed,maxZoneSpeed);
			}
			else
				canZoneMove = false;

			//Depending on the gauge type, the amount of time to clear it is determined here.
			float timeToAct = 10f;
			switch(gaugeType)
			{
				case 0:
					timeToAct = 5f;
					break;
				case 1:
					if(numbWinZones >= 3)
						timeToAct = 7f;
					break;
				case 2:
					if(numbWinZones == 3)
						timeToAct = 8f;
					else if(numbWinZones == 4)
						timeToAct = 9f;
					break;
			}

			generatedTrigger = (GameObject)Instantiate(trigger,gameObject.transform.position,Quaternion.identity);

			//Change the sprite depending on the type of gauge it is
			switch(gaugeType)
			{
			case 0:
				generatedTrigger.GetComponent<SpriteRenderer>().sprite = minigameDisplay[0];
				break;
			case 1:
				generatedTrigger.GetComponent<SpriteRenderer>().sprite = minigameDisplay[1];
				break;
			case 2:
				generatedTrigger.GetComponent<SpriteRenderer>().sprite = minigameDisplay[2];
				break;
			}

			generatedTrigger.GetComponent<MinigameTrigger>().setValues(tickerSpeed,numbWinZones,
				true,maxSizeZone,maxSizeZone,minSizeZone,gaugeType,canZoneMove,zoneSpeed,timeToAct);
			numbGamesCreated++;
		}
	}

	//Gets rid of the generated trigger so another one can be formed.
	public void destroyGeneratedTrigger()
	{
		Destroy(generatedTrigger);
		generatedTrigger = null;
		createdTrigger = false;
	}

	//Depending on the round, the speed of the ticker increases.
	public void increaseDifficulty()
	{
		if(tickerSpeed < 0.9f)
			tickerSpeed += 0.01f;

		rentIncrementor += (100 * numbRounds);

		//if the rent requirement reaches a point where it's impossible to get, the rent becomes the player's cash + 900.
		if((rentIncrementor + rentRequirement) - GameObject.Find("Player").GetComponent<PlayerScript>().totalMoney > 1000)
			rentRequirement = ((int)GameObject.Find("Player").GetComponent<PlayerScript>().totalMoney) + 900;
		else
			rentRequirement += rentIncrementor;
	}
}
