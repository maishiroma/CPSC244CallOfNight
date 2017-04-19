using UnityEngine;
using System.Collections;

//Used to display a graphic in how luch time the player has to complete a game.

public class GaugeTimer : MonoBehaviour {
	public Sprite[] countNumberPics = new Sprite[5];		//Contains the graphic representing the time left.
	private int currentIndex = 0;							//The current index that's on the Sprite array

	//Acts as a GUI in that the time left is represented by a graphical.
	public void changeSprite(int changedIndex)
	{
		if(currentIndex != changedIndex && changedIndex < countNumberPics.Length)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = countNumberPics[changedIndex];
			currentIndex = changedIndex;
			if(currentIndex > countNumberPics.Length)
				currentIndex = 0;
		}
	}
}
