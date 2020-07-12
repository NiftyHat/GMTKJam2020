using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
	AudioSource music;
	void Awake() {
		if(FindObjectsOfType<MusicPlayer>().Length > 1) {
			Destroy(this);
			return;
		} 

		DontDestroyOnLoad(gameObject);
		music = GetComponent<AudioSource>();
	}

	bool isMuted = false;
	float soundMultiplier = 1;
	float volume = 1;

	void Update() {

		if(isMuted){
			if(soundMultiplier > 0.01) soundMultiplier = Mathf.Lerp(soundMultiplier, 0, 0.2f);
			else soundMultiplier = 0;
		} else {
			if(soundMultiplier < 0.99) soundMultiplier = Mathf.Lerp(soundMultiplier, 1, 0.2f);
			else soundMultiplier = 1;
		}

		if(music.isPlaying && soundMultiplier <= 0) music.Pause();
		else if(!music.isPlaying && soundMultiplier > 0) music.UnPause();

		music.volume = soundMultiplier * volume;
	}

	public void Play() {
		music.Play();
	}

	public bool Mute {
		get {
			return isMuted;
		}
		set {
			isMuted = value;
		}
	}

}
