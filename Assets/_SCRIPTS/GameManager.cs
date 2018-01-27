using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UI;
=======
using UnityEngine.SceneManagement;
>>>>>>> c0a0d39eccc0908b0610da33424f5a2c86407e2d

public class GameManager : MonoBehaviour   {

<<<<<<< HEAD
	public static int currentTowerCircle = 3; 
	public static int currentTowerArc = 3;
	public static int currentTowerLine = 3;


	[SerializeField]
	private Text countTowerCircle;

	[SerializeField]
	private Text countTowerArc;

	[SerializeField]
	private Text countTowerLine;

	

=======
	public float powerUpTimeForSceneChange = 2;
>>>>>>> c0a0d39eccc0908b0610da33424f5a2c86407e2d
	// Use this for initialization
	void Start () {
		
	}
	
<<<<<<< HEAD

=======
	float timer;
>>>>>>> c0a0d39eccc0908b0610da33424f5a2c86407e2d
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
