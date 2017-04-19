using UnityEngine;
using System.Collections;

//Used to keep track if the player got the section bonus

public class SectionScript : MonoBehaviour {
	public int numbOfMinigames;				//How many minigames are in the section
	public int numbGamesSucceeded;			//The number of minigames the player has won
	public double sectionBonus;				//The current section's bomus

	//Determines how many minigames there are in the section.
	void Start () {
		numbOfMinigames = GameObject.FindGameObjectsWithTag("Minigame").Length;
	}

	//Determins if the player has won the bonus for winning all of the minigames.
	public double DetermineBonus()
	{
		if(numbOfMinigames == numbGamesSucceeded)
			return sectionBonus;
		else
			return 0f;
	}

}
