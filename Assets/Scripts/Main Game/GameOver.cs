using UnityEngine;
using System.Collections;

//Simply deletes the Stage Music, the player and its children when the player reaches the GameOver Screen.

public class GameOver : MonoBehaviour {

	void Start () {
		if(GameObject.Find("HUD") != null)
			Destroy(GameObject.Find("HUD"));
		if(GameObject.Find("Player") != null)
			GameObject.Find("Player").GetComponent<PlayerScript>().destroyPlayer();
		if(GameObject.Find("BGM_Stage"))
			Destroy(GameObject.Find("BGM_Stage"));
	}
}
