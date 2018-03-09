using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

	public AudioSource music;
	public AudioSource winLvl;
	public AudioSource startLvl;
	public AudioSource finalWin;
	public AudioSource btnClick;
	public AudioSource destroy;
	public AudioSource towerBuilt;
	public AudioSource takeTower;
	public AudioSource deny;
	protected static Sounds instance;
	
	bool soundOn;

	// Use this for initialization
	void Awake () {
		ToggleSound(Prefs.GetMasterVolume() > 0);

		music.loop = true;

		//music.volume = 0.8f;
		Prepare();
	}

	public void ToggleSound()
	{
		ToggleSound(!soundOn);
	}

	public void ToggleSound(bool enabled)
	{
		Debug.Log("Toggle sound " + enabled);
		soundOn = enabled;
		music.mute = !enabled;
		finalWin.mute = !enabled;
		winLvl.mute = !enabled;
		btnClick.mute = !enabled;
		destroy.mute = !enabled;
		towerBuilt.mute = !enabled;
		takeTower.mute = !enabled;
		startLvl.mute = !enabled;
		deny.mute = !enabled;

		Prefs.SetMasterVolume(soundOn?1:0);
	}

	public void Prepare() {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	public static void PlayMusic() {
		if (instance != null) if (!instance.music.isPlaying) instance.music.Play();
	}

	public static void VolumeMusic(float volume) {
		if (instance != null) instance.music.volume = volume;
	}

	public static void StopMusic() {
		if (instance != null) if (!instance.music.isPlaying) instance.music.Play();
	}

	public static void PlayStartLevel() {
		if (instance != null) instance.startLvl.Play();
	}

	public static void PlayButtonClick() {
		if (instance != null) instance.btnClick.Play();
	}

	public static void PlayWinLevel() {
		if (instance != null) instance.winLvl.Play();
	}
	
	public static void PlayTowerBuild() {
		if (instance != null) instance.towerBuilt.Play();
	}

	public static void PlayTowerTake() {
		if (instance != null) instance.takeTower.Play();
	}

	public static void PlayDeny() {
		if (instance != null) instance.deny.Play();
	}
	
	public static void PlayDestroy() {
		if (instance != null) instance.destroy.Play();
	}
}
