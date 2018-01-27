using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float powerUpTimeForSceneChange = 2;
	// Use this for initialization
	void Start () {
		
	}
	
	float timer;
	// Update is called once per frame
	void Update () {
		GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
		if (houses.Length > 0) {
			bool allPowered = true;		
			foreach (GameObject house in houses)
			{
				HouseScript hs = house.GetComponent<HouseScript>();
				allPowered &= hs.Powered;
			}

			if (allPowered) {
				timer += Time.deltaTime;
				if (timer >= powerUpTimeForSceneChange) {
					Debug.Log("Full power");
					SceneManager.LoadScene("TEST");
					timer = 0;
				}
			} else {
				timer = 0;
			}
		}
	}
}
