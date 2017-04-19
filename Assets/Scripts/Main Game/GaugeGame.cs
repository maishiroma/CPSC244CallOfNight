using UnityEngine;
using System.Collections;

/* Controls how the gauge behavies. There will be three types of gauges, which will be using this script.
 * Gauge_1: the player has to stop the ticker in a specified zone. 
 * Gauge_2: the player has to stop the ticker in all of the zones specified in order to proceed.
 * Gauge_3: the gauge starts out with one win zone. Once the player lands in the win zone, more win zones spawn. This 
 * 			continues until the max number of win zones is met.
 */ 

public class GaugeGame : MonoBehaviour {
	public int gaugeMode;						//0 = normal; 1 = hit all zone; 2 = hit all zones on one pass.
	public int numbWinZones;					//How many win zones does this gauge have?
	public float moveSpeed;						//Move speed of the ticker. 
	public float timeToAct;						//How long does the player have in completing the minigame
	public bool isActivated;					//Is the minigame active on the screen?
	public bool isStopped;						//Has the ticker been stopped by the player?

	private bool canContinue;					//Did the player land on a win zone?
	private bool wonGauge;						//Did the player win?
	private float origTimeToAct;				//Keeps track of the original gauge's time to act.
	private float xChangeForTicker;				//Determines how far the ticker moves right to left.
	private int numberOfSpawnedZones;			//Used in Gauge_3 to determine how many zones will spawn.
	private Transform gaugeForTicker;			//Stores the parent object of the gauge.

	void Start () {
		gaugeForTicker = gameObject.transform.parent;
		xChangeForTicker = gaugeForTicker.GetComponent<GenerateWinZones>().xChangeFromCenter;
		hideGame();
	}

	//Does the main chunck of the work.
	void Update () {
		if(isActivated == true)
		{
			if(Input.GetKeyDown("space") && isStopped == false)	//Stops the ticker
			{
				isStopped = true;
				switch(gaugeMode)	//Depending on the mode, a different method will be called.
				{
					case 0:
						Invoke("results",0.1f);
						break;
					case 1:
						Invoke("Gauge2Actions",0.1f);
						break;
					case 2:
						Invoke("Gauge3Actions",0.1f);
						break;
				}
			}
			else if(isStopped == false)	//Moves the ticker using the move speed provided
			{
				Vector3 newPos = new Vector3(moveSpeed,0f,0f);
				gameObject.transform.position += newPos;
				switchDirection();
			}

			//Counts down the amount of time the player has to complete the game
			int timeLeftInSeconds = (int)(timeToAct % 60);
			if(timeLeftInSeconds >= 1)
			{
				timeToAct -= Time.deltaTime;
				updateTimer(timeLeftInSeconds);
			}
			else
				results();
		}
	}
		
	//Stops the ticker in place and checks if the player has landed in a valid spot, if it still exists.
	void Gauge2Actions()
	{
		if(canContinue == true)
		{
			if(numbWinZones <= 0)
			{
				wonGauge = true;
				results();
			}
			else
			{
				canContinue = false;
				isStopped = false;
			}
		}
		else
			results();
	}

	//Stop the ticker in thw win zone. If the number of active zones != numbWinZones, more win zones will spawn.
	void Gauge3Actions()
	{
		if(canContinue == true)
		{
			int remainingZones = GameObject.FindGameObjectsWithTag("WinZoneGauge").Length;
			if(remainingZones <= 0)
			{
				GameObject currMinigame = GameObject.Find("Player").GetComponent<PlayerScript>().currentMinigame;
				numberOfSpawnedZones++;
				numbWinZones--;
				currMinigame.GetComponent<MinigameTrigger>().GenerateNewZonesForGauge3(numberOfSpawnedZones);
				canContinue = false;
				isStopped = false;

				if(numbWinZones <= 0)
				{
					wonGauge = true;
					results();
				}
			}
			else
			{
				canContinue = false;
				isStopped = false;
			}
		}
		else
			results();
	}

	//Checks if the player landed the ticker in a win zone. Differs depending on gauge type.
	void OnTriggerStay(Collider other)
	{
		switch(gaugeMode)
		{
			case 0:
				if(isStopped == true && other.tag == "WinZoneGauge")
					wonGauge = true;
				break;
			case 1:
				if(isStopped == true && other.tag == "WinZoneGauge")
				{
					numbWinZones--;
					Destroy(other.gameObject);
					canContinue = true;
				}
				break;
			case 2:
				if(isStopped == true && other.tag == "WinZoneGauge")
				{
					Destroy(other.gameObject);
					canContinue = true;
				}
				break;
		}
	}

	//Changes the direction of the ticker after it moving a certain disitance. IF the gauge is mode 2, the player fails instead.
	void switchDirection()
	{
		if(Mathf.Abs(gaugeForTicker.transform.position.x - gameObject.transform.position.x) >= xChangeForTicker)
			moveSpeed *= -1;
	}

	//Updates the GUI for how much time is left in the timer.
	void updateTimer(int timeLeft)
	{
		switch(timeLeft)
		{
			case 5:
			case 4:
			case 3:
			case 2:
			case 1:
			gaugeForTicker.GetChild(1).GetComponent<GaugeTimer>().changeSprite(timeLeft - 1);
				break;
		}
	}

	//Checks if the player won or lost, depending on what they landed on.
	void results()
	{
		if(wonGauge == true)
			GameObject.Find("Player").GetComponent<PlayerScript>().currentMinigame.GetComponent<MinigameTrigger>().endMinigame(true);
		else
			GameObject.Find("Player").GetComponent<PlayerScript>().currentMinigame.GetComponent<MinigameTrigger>().endMinigame(false);

		gaugeForTicker.GetComponent<GenerateWinZones>().deleteWinZones();
		resetValues();
		hideGame();
	}

	//Resets the values back to what they were in the prefab. Used when to start a new minigame.
	void resetValues()
	{
		isStopped = false;
		wonGauge = false;
		canContinue = false;
		timeToAct = origTimeToAct;
		gaugeForTicker.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
	}


	//Hides the gauge when the minigame is finished
	void hideGame()
	{
		for(int i = 0; i < gaugeForTicker.childCount; ++i)
		{
			if(gaugeForTicker.GetChild(i).GetComponent<SpriteRenderer>() != null)
				gaugeForTicker.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
		}

		isActivated = false;
	}

	//Allows for MinigameTrigger to easily modify the values of the gauge's speed, ticker, gauge type, and time to act.
	public void changeSpeedTimeAndNumbOfZones(float tickerSpeed, int winZoneCount, float time, int gaugeType)
	{
		numberOfSpawnedZones = 1;
		moveSpeed = tickerSpeed;
		numbWinZones = winZoneCount;
		timeToAct = time;
		origTimeToAct = time;
		gaugeMode = gaugeType;
		gameObject.transform.position = new Vector3(gaugeForTicker.transform.position.x,gaugeForTicker.transform.position.y, gameObject.transform.position.z);
	}

	//Displays the gauge when the minigame is activated
	public void displayGame()
	{
		for(int i = 0; i < gaugeForTicker.childCount; ++i)
		{
			if(gaugeForTicker.GetChild(i).GetComponent<SpriteRenderer>() != null)
				gaugeForTicker.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
		}

		//Randomely decides if the ticker will move left or right upon start
		if(Random.Range(1f,10f) >= 5f)
			moveSpeed *= -1;
		
		isActivated = true;
	}
}