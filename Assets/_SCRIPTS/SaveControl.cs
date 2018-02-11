﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveControl : MonoBehaviour {


	public static SaveControl instance;

	public List<int> towersUsedToWin = new List<int>();

	void Awake () {
			instance = this;
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
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			towersUsedToWin = data.towersUsedToWin;
		}
	}
}

[Serializable]
class PlayerData
{
	public List<int> towersUsedToWin;

}