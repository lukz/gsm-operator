using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour   {
public static int currentTowerCircle = 3; 
	public static int currentTowerArc = 3;
	public static int currentTowerLine = 3;


	[SerializeField]
	private Text countTowerCircle;

	[SerializeField]
	private Text countTowerArc;

	[SerializeField]
	private Text countTowerLine;


	public static GameManager instance = null;


	public float powerUpTimeForSceneChange = 2;

	void Awake()
	{

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
