using UnityEngine;
using System.Collections;

//Defines the win zones and what they do. 

public class WinZoneBehavior : MonoBehaviour {
	public bool canMove = false;		//Determines if the win zone moves left and right
	public float xChangeForZone;		//MUST BE SET TO SAME VALUE AS XCHANGEFORTICKER IN GAUGEGAME!!!!
	public float moveSpeed;				//How fast will this gauge move, if it can?

	private Transform parentGauge;		//The gauge that the win zone is attatched to.
	private Transform gaugeGame;		//The object that has the GaugeGame script attatched to.

	//If the win zone can move around, it moves here
	void Update()
	{
		if(canMove == true)
		{
			if(gaugeGame.GetComponent<GaugeGame>().isStopped == false)
			{
				Vector3 newPos = new Vector3(moveSpeed,0f,0f);
				gameObject.transform.position += newPos;
				switchDirection();
			}
		}
	}

	//Changes the direction that the zone is moving.
	void switchDirection()
	{
		if(Mathf.Abs(parentGauge.transform.position.x - gameObject.transform.position.x) >= xChangeForZone)
			moveSpeed *= -1;
	}

	//This is to make sure that the win zones don't overlap. Not a true fix, but it kinda works.
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "WinZoneGauge")
		{
			if(canMove == true)
				moveSpeed *= -1;
			else
				parentGauge.GetComponent<GenerateWinZones>().rePosWinZone(gameObject);
		}
	}

	//This function helps set up the values needed. 
	public void helpSetUp(bool zoneMove, float zoneSpeed, GameObject other)
	{
		gameObject.transform.parent = other.transform;
		parentGauge = other.transform;
		gaugeGame = other.transform.GetChild(0);
		canMove = zoneMove;
		moveSpeed = zoneSpeed;
		xChangeForZone = parentGauge.GetComponent<GenerateWinZones>().xChangeFromCenter;

		//Randomely decides if the zone will move left or right upon start
		if(Random.Range(1f,10f) >= 5f)
			moveSpeed *= -1;
	}
}
