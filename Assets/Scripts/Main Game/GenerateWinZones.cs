using UnityEngine;
using System.Collections;

public class GenerateWinZones : MonoBehaviour {
	public GameObject winZoneToCopy;		//The gameobject used to generate the win zones
	public float xChangeFromCenter;			//How far the win zones will spawn from the center of the gauge. NEEDS TO BE SET BEFOREHAND!

	//Depending on the number of win zones, win zones are randomly placed onto the gauge.
	public void generateWinZones(int numbOfWinZones, bool randomSizes, float winZoneSize, float randMaxSize, float randMinSize, int gaugeType, bool zoneMove, float zoneSpeed)
	{
		float leftMostBound = this.gameObject.transform.position.x - xChangeFromCenter;
		float rightMostBound = this.gameObject.transform.position.x + xChangeFromCenter;
		for(int i = numbOfWinZones; i > 0; --i)
		{
			float newXPos = Random.Range(leftMostBound,rightMostBound);
			Vector3 newPos = new Vector3(newXPos,this.gameObject.transform.position.y,4.5f);
			GameObject newWinZone = (GameObject)Instantiate(winZoneToCopy,newPos,Quaternion.identity);
			newWinZone.GetComponent<WinZoneBehavior>().helpSetUp(zoneMove,zoneSpeed,gameObject);

			if(randomSizes == true)
			{
				float newXSize = Random.Range(randMinSize,randMaxSize);
				newWinZone.transform.localScale += new Vector3(newXSize,0f,0f);
			}

			StartCoroutine(WaitAndCheckForCollision());
		}
	}

	//Deletes all of the win zones that are currently on the gauge.
	public void deleteWinZones()
	{
		GameObject[] winZones = GameObject.FindGameObjectsWithTag("WinZoneGauge");
		for(int i = 0; i < winZones.Length; ++i)
			Destroy(winZones[i]);
	}

	//Repositions a win zone. Called if there's two win zones overlapping.
	public void rePosWinZone(GameObject currWinZone)
	{
		float leftMostBound = this.gameObject.transform.position.x - xChangeFromCenter;
		float rightMostBound = this.gameObject.transform.position.x + xChangeFromCenter;
		float newXPos = Random.Range(leftMostBound,rightMostBound);
		Vector3 newPos = new Vector3(newXPos,this.gameObject.transform.position.y,4.5f);
		currWinZone.transform.position = newPos;

	}

	//This is called so that the script in the win zones can activate.
	IEnumerator WaitAndCheckForCollision()
	{
		yield return new WaitForSeconds(1f);
	}

}