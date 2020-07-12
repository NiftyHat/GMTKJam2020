using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControlsBehaviour : MonoBehaviour
{

	public TMP_Text MuteButtonText;
	public MusicPlayer MusicPlayerRef;

	void Awake() {
		if(FindObjectsOfType<MusicPlayer>().Length == 0) {
			Instantiate(MusicPlayerRef);
		}
		UpdateUI();
	}

	private void UpdateUI()
	{
		MuteButtonText.text = $"MUSIC\n{(FindObjectOfType<MusicPlayer>().Mute ? "OFF" : "ON")}";
	}

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

	public void ChangeMusic() {
		MusicPlayer m = FindObjectOfType<MusicPlayer>();
		m.Mute = !m.Mute; 
		UpdateUI();
	}
}
