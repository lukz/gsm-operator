using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Prefs {

	const string MASTER_VOLUME_KEY = "master-volume";
	public static void SetMasterVolume(float volume) {
		if (volume >= 0 && volume <= 1) {
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		} else {
			Debug.LogError("Master volume out of range [0,1], was " + volume);
		}
	}

	public static float GetMasterVolume() {
		PlayerPrefs.Save();
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY, 1);
	}

	public static void Save() {
		PlayerPrefs.Save ();
	}

	public static void Clear() {
		PlayerPrefs.DeleteAll ();
	}

	public static void SetDefaults() {
		SetMasterVolume(1);
	}
}
