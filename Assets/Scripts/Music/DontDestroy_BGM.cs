using UnityEngine;
using System.Collections;

public class DontDestroy_BGM : MonoBehaviour {

	// Prevent imminent destruction of BGM game object

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
