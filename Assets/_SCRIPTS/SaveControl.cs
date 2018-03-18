using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveControl : MonoBehaviour {

	[SerializeField]
	private string saveVersion;

	public static SaveControl instance;

	public List<int> towersUsedToWin = new List<int>();

	void Awake () {
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
	
	private bool retrySave;
	public void Save()
	{
		
		try {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo"+saveVersion+".dat", FileMode.OpenOrCreate);

			PlayerData data = new PlayerData();
			data.towersUsedToWin = towersUsedToWin;

			bf.Serialize(file, data);
			file.Close();
		} catch (Exception ex) {
			Debug.LogError("Failed to save player info " + ex);
			File.Delete(Application.persistentDataPath + "/playerInfo" + saveVersion + ".dat");
			if (retrySave) return;
			retrySave = true;
			Save();
			retrySave = false;
		}
	}

	public void Load()
	{
		try {
			if(File.Exists(Application.persistentDataPath + "/playerInfo" + saveVersion + ".dat"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/playerInfo" + saveVersion + ".dat", FileMode.Open);
				PlayerData data = (PlayerData)bf.Deserialize(file);
				towersUsedToWin = data.towersUsedToWin;
			}
		} catch (Exception ex) {
			Debug.LogError("Failed to load player info " + ex);
			File.Delete(Application.persistentDataPath + "/playerInfo" + saveVersion + ".dat");
		}
	}
}

[Serializable]
class PlayerData
{
	public List<int> towersUsedToWin;

}