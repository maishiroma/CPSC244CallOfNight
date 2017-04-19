using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour {

    private static Singleton instance = null;
    public static Singleton Instance
    {
        get { return instance; }
    }

    void Update()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
		// if (SceneManager.GetActiveScene().name == "FirstStage" || SceneManager.GetActiveScene().name == "ArcadeMode")
		if (SceneManager.GetActiveScene().name == "StoryScene_1" || SceneManager.GetActiveScene().name == "ArcadeCutscene_2")
        {
			if (GameObject.Find("menu_bgm"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
