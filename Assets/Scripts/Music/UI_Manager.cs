using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour {
    
    public void GameStart()
    {
		SceneManager.LoadScene("ModeSelection");
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void TitleScreen()
    {
		if(GameObject.Find("Stage_System") != null)
			Destroy(GameObject.Find("Stage_System"));

		SceneManager.LoadScene("TitleScreen");
    }
    
    public void StorySelect()
    {
		SceneManager.LoadScene("StoryScene_1");
    }

	public void RetryStage()
	{
		if(GameObject.Find("Stage_System") != null)
			GameObject.Find("Stage_System").GetComponent<StageSectionSelect>().retryStage();
	}
		
    public void ArcadeSelect()
    {
		SceneManager.LoadScene("ArcadeCutscene_1");
    }

    public void HelpScreen()
    {
		SceneManager.LoadScene("HowToPlay");
    }

}
