using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public string level;

	// Use this for initialization
	public void StartTheGame () {
        SceneManager.LoadScene(level);
	}
	
	
}
