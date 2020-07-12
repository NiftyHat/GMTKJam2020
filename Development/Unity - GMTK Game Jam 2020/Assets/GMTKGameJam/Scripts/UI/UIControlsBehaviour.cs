using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControlsBehaviour : MonoBehaviour
{
	public void Restart() {
		GotoLevel(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quit() {
		Application.Quit();
	}

	public void NextLevel() {
		GotoLevel(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void GotoLevel(int x) {
		// todo fancy level transition here

		int nextLevel = x % SceneManager.sceneCountInBuildSettings;
		SceneManager.LoadScene(nextLevel);
	}
}
