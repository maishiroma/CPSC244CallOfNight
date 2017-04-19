using UnityEngine;
using System.Collections;

//This is where the player moves to the next stage for Story Mode.
using UnityEngine.SceneManagement;

public class StageSectionSelect : MonoBehaviour {
	public bool arcadeVersion;
	public int currentStageIndex;
	public int currentSectionIndex;
	public int[] rentRequiredPerStage = new int[5];			//Contains the rent that's in the story mode.

	private string[,] stageList = new string[6,5];			//Contains the names all of the stages
	private Scene currentScene;

	/* So this is what the double array looks like:
	 * 			Col 0			Col 1			Col 2			Col3			Col4		
	 * 	Row 0	StoryScene_1	Stage1_Part1	Stage1_Part2	Stage1_Part3	Stage1_Results
	 * 	Row 1	StoryScene_2	Stage2_Part1	Stage2_Part2	Stage2_Part3	Stage2_Results
	 *  Row 2	...
	 *  Row 3	...
	 *  Row 4	...
	 * 
	 * stageList.GetLength(0) returns the length of the rows and stageList.GetLength(1) returns the length of the columns.
	 * 
	 * To debug specific stages, simply change the Stage_System's Stage Index from 0-5 (0 being the first stage and 5 being the last stage) and
	 * moving the player to the stage that you want to debug.
	 */

	//Initializes all of the stage names and rents for each stage.
	void Start () {
		if(arcadeVersion == false)
		{
			stageList[0,0] = "StoryScene_1";
			stageList[0,1] = "Stage1_Part1";
			stageList[0,2] = "Stage1_Part2";
			stageList[0,3] = "Stage1_Part3";
			stageList[0,4] = "Stage1_Results";
			stageList[1,0] = "StoryScene_2";
			stageList[1,1] = "Stage2_Part1";
			stageList[1,2] = "Stage2_Part2";
			stageList[1,3] = "Stage2_Part3";
			stageList[1,4] = "Stage2_Results";
			stageList[2,0] = "StoryScene_3";
			stageList[2,1] = "Stage3_Part1";
			stageList[2,2] = "Stage3_Part2";
			stageList[2,3] = "Stage3_Part3";
			stageList[2,4] = "Stage3_Results";
			stageList[3,0] = "StoryScene_4";
			stageList[3,1] = "Stage4_Part1";
			stageList[3,2] = "Stage4_Part2";
			stageList[3,3] = "Stage4_Part3";
			stageList[3,4] = "Stage4_Results";
			stageList[4,0] = "StoryScene_5";
			stageList[4,1] = "Stage5_Part1";
			stageList[4,2] = "Stage5_Part2";
			stageList[4,3] = "Stage5_Part3";
			stageList[4,4] = "Stage5_Results";
			stageList[5,0] = "StoryScene_6";
		}
	}

	//Prevents this object from ever getting destroyed, unless explicitly told to.
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	//Moves the player to the next section. If the player finishes all of the parts of the stage, StageComplete is run.
	public void NextSection()
	{
		if(arcadeVersion == true)	//If the player is in arcade mode, the game loads the arcade stage, rather than the main game.
		{
			if(currentScene.name != "ArcadeStage")
			{
				SceneManager.UnloadScene(currentScene.name);
				SceneManager.LoadScene("ArcadeStage");
				currentScene = SceneManager.GetActiveScene();
			}
			else
			{
				GameObject.Find("Player").transform.position = GameObject.FindGameObjectWithTag("Start").transform.position;
				GameObject.Find("HUD").GetComponent<SectionGUI>().currentRentDue = GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().rentRequirement;
				GameObject.Find("HUD").GetComponent<SectionGUI>().currentStageNumber = GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().numbRounds;
				GameObject.Find("HUD").GetComponent<SectionGUI>().displayHUD(true);
			}
		}
		else
		{
			if(currentSectionIndex < stageList.GetLength(1))
			{
				//If the player reaches the end of story mode, they are taken to the title screen
				if(currentStageIndex == 5 && currentSectionIndex == 0)
				{
					SceneManager.UnloadScene(currentScene.name);
					SceneManager.LoadScene("TitleScreen");

					if(GameObject.Find("Player") != null)
						GameObject.Find("Player").GetComponent<PlayerScript>().destroyPlayer();

					Destroy(gameObject);
				}
				else
				{
					currentSectionIndex++;
					if(currentSectionIndex == 4)
						GameObject.Find("HUD").GetComponent<SectionGUI>().inResults = true;
					
					SceneManager.UnloadScene(currentScene.name);
					SceneManager.LoadScene(stageList[currentStageIndex, currentSectionIndex]);
					currentScene = SceneManager.GetActiveScene();
				}
			}
		}
	}

	//Transitions the player into the next stage.
	public void NextStage()
	{
		currentStageIndex++;
		if(currentStageIndex < stageList.GetLength(0))
		{
			currentSectionIndex = 0;
			SceneManager.UnloadScene(currentScene.name);
			SceneManager.LoadScene(stageList[currentStageIndex,currentSectionIndex]);
			currentScene = SceneManager.GetActiveScene();
		}
	}

	//Transitions the player to the game over screen.
	public void goToGameOver()
	{
		SceneManager.UnloadScene(currentScene.name);
		SceneManager.LoadScene("GameOver");
		currentScene = SceneManager.GetActiveScene();
	}

	//Takes the player to the current arcade round's results
	public void arcadeRoundResults()
	{
		GameObject.Find("HUD").GetComponent<SectionGUI>().inResults = true;
		SceneManager.UnloadScene(currentScene.name);
		SceneManager.LoadScene("ArcadeResults");
		currentScene = SceneManager.GetActiveScene();
	}

	//Takes the player back to the last stage they've cleared. Called if the player gets a game over.
	public void retryStage()
	{
		SceneManager.UnloadScene(currentScene.name);

		if(arcadeVersion == true)
		{
			SceneManager.LoadScene("ArcadeStage");
			currentStageIndex = 1;
		}
		else
		{
			SceneManager.LoadScene(stageList[currentStageIndex,1]);
			currentSectionIndex = 1;
		}

		currentScene = SceneManager.GetActiveScene();
	}

	//This moves the player to the start spot once the level is loaded. It also determines the rent needed for the stage.
	void OnLevelWasLoaded(int level)
	{
		if(GameObject.FindGameObjectWithTag("Start") != null)
			GameObject.Find("Player").transform.position = GameObject.FindGameObjectWithTag("Start").transform.position;
			
		if(GameObject.Find("HUD") != null)
		{
			if(GameObject.Find("Player") != null)
			{
				if(GameObject.Find("Player").GetComponent<PlayerScript>().inArcadeMode == true)
				{
					GameObject.Find("HUD").GetComponent<SectionGUI>().currentRentDue = GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().rentRequirement;
					GameObject.Find("HUD").GetComponent<SectionGUI>().currentStageNumber = GameObject.Find("ArcadeGenerator").GetComponent<ArcadeMinigameTriggerGenerator>().numbRounds;
				}
				else
				{
					if(currentStageIndex < rentRequiredPerStage.Length)
						GameObject.Find("HUD").GetComponent<SectionGUI>().currentRentDue = rentRequiredPerStage[currentStageIndex];

					GameObject.Find("HUD").GetComponent<SectionGUI>().currentStageNumber = currentStageIndex + 1;
					GameObject.Find("HUD").GetComponent<SectionGUI>().currentSectionNumber = currentSectionIndex;
				}
			}
			GameObject.Find("HUD").GetComponent<SectionGUI>().displayHUD(true);
		}
	}
}
