using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControlsBehaviour : MonoBehaviour
{

	public TMP_Text MuteButtonText;
	public Image MusicSprite;
	public MusicPlayer MusicPlayerRef;
	public AmbientPlayer AmbientPlayerRef;

	public Sprite MusicOn;
	public Sprite MusicOff;

	protected AmbientPlayer _ambientPlayer;

	void Awake() {
		if(FindObjectsOfType<MusicPlayer>().Length == 0) {
			Instantiate(MusicPlayerRef, transform.parent);
		}
		if(FindObjectsOfType<AmbientPlayer>().Length == 0) {
			_ambientPlayer = Instantiate(AmbientPlayerRef, transform.parent);
		}
		UpdateUI();
	}

	void Start()
	{
		_ambientPlayer.Play();
	}

	private void UpdateUI()
	{
		bool muted = FindObjectOfType<MusicPlayer>().Mute;
		MusicSprite.sprite = muted ? MusicOff : MusicOn;
		MuteButtonText.text = $"MUSIC\n{(FindObjectOfType<MusicPlayer>().Mute ? "OFF" : "ON")}";
	}

	public void Restart() {
		GotoLevel(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quit() {
		Application.Quit();
	}

	public void NextLevel() {
		if (_ambientPlayer != null)
		{
			_ambientPlayer.Stop();
		}
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
