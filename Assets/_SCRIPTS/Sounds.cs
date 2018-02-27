using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

	public AudioSource music;
	public AudioSource music1;
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
	void Start () {
		ToggleSound(Prefs.GetMasterVolume() > 0);

		music.volume = 0.8f;
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
		music1.mute = !enabled;
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
		instance = this;
	}

	public static void PlayMusic() {
		if (!instance.music.isPlaying) instance.music.Play();
	}

	public static void VolumeMusic(float volume) {
		instance.music.volume = volume;
	}

	public static void StopMusic() {
		if (!instance.music.isPlaying) instance.music.Play();
	}

	public static void PlayStartLevel() {
		instance.startLvl.Play();
	}

	public static void PlayButtonClick() {
		instance.btnClick.Play();
	}

	public static void PlayWinLevel() {
		instance.winLvl.Play();
	}
	
	public static void PlayTowerBuild() {
		instance.towerBuilt.Play();
	}

	public static void PlayTowerTake() {
		instance.takeTower.Play();
	}

	public static void PlayDeny() {
		instance.deny.Play();
	}
	
	public static void PlayDestroy() {
		instance.destroy.Play();
	}
}
