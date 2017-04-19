using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//A special script only for the cutscene in Arcade Mode

public class ArcadeCutscene : MonoBehaviour {
	public GUIStyle helpTextFont = new GUIStyle();	//The text used to display "Press space to continue...."

	void Start()
	{
		//For the font. Temporary.
		helpTextFont.fontSize = 30;
		helpTextFont.normal.textColor = Color.white;
	}

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyUp("space") == true)
			SceneManager.LoadScene("ArcadeCutscene_2");
	}

	//Displays the help text to move on to the next image.
	void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( Screen.width / 1600.0f, Screen.height / 900.0f, 1.0f ) );
		GUI.Label(new Rect(0f, 850f, 50f, 50f),"Press space to continue...",helpTextFont);
	}
}
