using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveControl : MonoBehaviour {


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
	

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData();
		data.towersUsedToWin = towersUsedToWin;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		try {
			if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
				PlayerData data = (PlayerData)bf.Deserialize(file);
				towersUsedToWin = data.towersUsedToWin;
			}
		} catch (Exception ex) {
			Debug.LogError("Failed to load player info " + ex);
			File.Delete(Application.persistentDataPath + "/playerInfo.dat");
		}
	}
}

[Serializable]
class PlayerData
{
	public List<int> towersUsedToWin;

}