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

    public int tower1Count = 0;
    public int tower2Count = 0;
    public int tower3Count = 0;

    private int currentTier = 0;

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
		currentTowers.Add(tower1Count);
		currentTowers.Add(tower2Count);
		currentTowers.Add(tower3Count);
		UpdateNumbers();

        setTier(currentTier);
	}

    public void setTier(int tier)
    {
        GameObject[] houseSpots = GameObject.FindGameObjectsWithTag("HouseSpot");

        for (int i = 0; i < houseSpots.Length; i++)
        {
            HouseSpot houseSpot = houseSpots[i].GetComponent<HouseSpot>();

            houseSpot.SpawnTier(tier);
        }
    }


	 public static void UpdateNumbers()
	{
		if (instance.countTowerCircle) {
			int count = currentTowers[0];
			instance.countTowerCircle.text = count.ToString();
			instance.countTowerCircle.GetComponent<Transform>().parent.GetComponent<Button>().enabled = count > 0;
		}
		if (instance.countTowerArc) {
			int count = currentTowers[1];
			instance.countTowerArc.text = count.ToString();
			instance.countTowerArc.GetComponent<Transform>().parent.GetComponent<Button>().enabled = count > 0;
		}
		if (instance.countTowerLine) {
			int count = currentTowers[2];
			instance.countTowerLine.text = count.ToString();
			instance.countTowerLine.GetComponent<Transform>().parent.GetComponent<Button>().enabled = count > 0;
		}
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
			float countPowered = 0;
			foreach (GameObject house in houses)
			{
				HouseScript hs = house.GetComponent<HouseScript>();
				if (hs.IsPowered()) countPowered++;
				allPowered &= hs.IsPowered();
			}


			powerSlider.value = countPowered / houses.Length;
			if (allPowered) {
				timer += Time.deltaTime;
				if (timer >= powerUpTimeForSceneChange) {
                    Debug.Log("Full power");
                    changeTierOrScene();
                    timer = 0;
				}
			} else {
				timer = 0;
			}
		}
	}

    public void changeTierOrScene()
    {
        int maxTier = 0;
        
        // Check if next tier aviable
        GameObject[] houseSpots = GameObject.FindGameObjectsWithTag("HouseSpot");
        for (int i = 0; i < houseSpots.Length; i++)
        {
            maxTier = Mathf.Max(maxTier, houseSpots[i].GetComponent<HouseSpot>().houseTiers.Count - 1);
        }


        if(currentTier >= maxTier)
        {
            SceneManager.LoadScene("TEST");
        }
        else
        {
            currentTier++;
            setTier(currentTier);

            Debug.Log("NEX TIER! " + currentTier + " / " + maxTier);
        }




    }


}
