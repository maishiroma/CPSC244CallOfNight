using UnityEngine;
using System.Collections;

//This controls how the cutscenes are played out

public class CutScene : MonoBehaviour {
	public Sprite[] textImages;						//The images used to represent the cutscene
	public GUIStyle helpTextFont = new GUIStyle();	//The text used to display "Press space to continue...."

	private int currentIndex;						//the current index being used in textImages

	//This is needed so that there isn't duplicate main cameras.
	void Start () {
		if(GameObject.Find("Player") != null)
			GameObject.Find("Player").GetComponent<PlayerScript>().destroyPlayer();

		//For the font. Temporary.
		helpTextFont.fontSize = 30;
		helpTextFont.normal.textColor = Color.white;

	}
	
	//The player can proceed through the cutscene at their own pace.
	void Update () {
		if(Input.GetKeyUp("space") == true)
		{
			if(currentIndex + 1 < textImages.Length)
			{
				//The next text is loaded
				currentIndex++;
				gameObject.GetComponent<SpriteRenderer>().sprite = textImages[currentIndex];
				if(currentIndex > textImages.Length)
					currentIndex = 0;
			}
			else
			{
				//The game loads the next area.
				GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().NextSection();
			}
		}
	}

	//Displays the help text to move on to the next image.
	void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( Screen.width / 1600.0f, Screen.height / 900.0f, 1.0f ) );
		GUI.Label(new Rect(0f, 850f, 50f, 50f),"Press space to continue...",helpTextFont);
	}

}