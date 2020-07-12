using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientPlayer : MonoBehaviour
{
	AudioSource ambient;
	void Awake() {
		if(FindObjectsOfType<MusicPlayer>().Length > 1) {
			Destroy(this);
			return;
		} 

		DontDestroyOnLoad(gameObject);
		ambient = GetComponent<AudioSource>();
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

		if(ambient.isPlaying && soundMultiplier <= 0) ambient.Pause();
		else if(!ambient.isPlaying && soundMultiplier > 0) ambient.UnPause();

		ambient.volume = soundMultiplier * volume;
	}

	public void Play() {
		ambient.Play();
	}

	public bool Mute {
		get {
			return isMuted;
		}
		set {
			isMuted = value;
		}
	}

	public void Stop()
	{
		ambient.Stop();
	}
}
