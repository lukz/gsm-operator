using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour   {


public static List<int> currentTowers = new List<int>(); 
	

	[SerializeField]
	private Text countTowerCircle;

	[SerializeField]
	private Text countTowerArc;

	[SerializeField]
	private Text countTowerLine;

	public static float shakePower = 0;

	[SerializeField]
	private Slider powerSlider;


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
		currentTowers.Add(3);
		currentTowers.Add(3);
		currentTowers.Add(3);
		UpdateNumbers();
	}


	 public static void UpdateNumbers()
	{

		instance.countTowerCircle.text = currentTowers[0].ToString();
		instance.countTowerArc.text = currentTowers[1].ToString();
		//instance.countTowerLine.text = currentTowers[2].ToString();
	}




	float timer;
	// Update is called once per frame
	void Update () {


		if (shakePower > 0.01f)
		{
			shakePower *= 0.6f;

			if (Random.Range(0, 1) > 0.5f) { 
				Camera.main.transform.position = new Vector3(Random.Range(shakePower / 2, shakePower), Random.Range(shakePower / 2, shakePower), Camera.main.transform.position.z);
			}
			else
			{
				Camera.main.transform.position = new Vector3(Random.Range(-shakePower / 2, -shakePower), Random.Range(-shakePower / 2, -shakePower), Camera.main.transform.position.z);
			}


		}
		else
		{
			shakePower = 0;
			Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
		}


		GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
		if (houses.Length > 0) {
			bool allPowered = true;
			int countPowered = 0;
			foreach (GameObject house in houses)
			{
				HouseScript hs = house.GetComponent<HouseScript>();
				if (hs.Powered) countPowered++;
				allPowered &= hs.Powered;
			}


			powerSlider.value = countPowered / houses.Length;


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
