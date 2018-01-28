using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour   {


	public AudioSource music;
	public AudioSource winPhase;
	public AudioSource winLvl;
	public AudioSource btnClick;
	public AudioSource destroy;
	public AudioSource towerBuilt;
	public AudioSource deny;

    public GameObject splash;

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

    private int tower1aCount = 0;
	private int tower2aCount = 0;
	private int tower3aCount = 0;
	private int tower1bCount = 0;
	private int tower2bCount = 0;
	private int tower3bCount = 0;
	private int tower1cCount = 0;
	private int tower2cCount = 0;
	private int tower3cCount = 0;

	LVLsettings lvlmanager;

	private int currentTier = 0;

    public static GameManager instance = null;

	public float powerUpTimeForSceneChange = 2;

    private bool splashShown = false;

	private string nextScene = "TEST";

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
		lvlmanager = GameObject.FindGameObjectWithTag("LVLmanager").GetComponent<LVLsettings>();

		nextScene = lvlmanager.nextScene;

		currentTowers.Add(lvlmanager.tower1aCount);
		currentTowers.Add(lvlmanager.tower2aCount);
		currentTowers.Add(lvlmanager.tower3aCount);
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

        showForbiddenZones(false);
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

            float realProgress = (countPowered / houses.Length);
            float swim = Mathf.Sin(Time.timeSinceLevelLoad * 3f);
            if (realProgress == 0 || realProgress == 1) swim = 0;
            float currentPercent = Mathf.Clamp(realProgress + swim / 100f * 1.5f, 0, 1);
            powerSlider.value = Mathf.Lerp(powerSlider.value, currentPercent, Time.deltaTime * 5);

			if (allPowered) {
                if (!splashShown)
                {
                    Debug.Log("test! " + currentTier);
                    splashShown = true;
                    GameObject newSplash = Instantiate(splash);
                    if (IsNextTierAviable())
                    {
                        newSplash.GetComponent<SplashScript>().setSplash(currentTier);
                    }
                    else
                    {
                        newSplash.GetComponent<SplashScript>().setEndSplash();
                    }
                }


                if (timer == 0)
				{
					int maxTier = 0;

					// Check if next tier aviable
					GameObject[] houseSpots = GameObject.FindGameObjectsWithTag("HouseSpot");
					for (int i = 0; i < houseSpots.Length; i++)
					{
						maxTier = Mathf.Max(maxTier, houseSpots[i].GetComponent<HouseSpot>().houseTiers.Count - 1);
					}

					if (currentTier >= maxTier)
					{
						winLvl.Play();
					}
					else
					{
						winPhase.Play();
					}
				}

				timer += Time.deltaTime;
				music.volume = Mathf.Max(0.5f,(powerUpTimeForSceneChange - timer)*0.8f);

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

        splashShown = false;

		music.volume = 0.8f;
        int maxTier = 0;
        
        // Check if next tier aviable
        GameObject[] houseSpots = GameObject.FindGameObjectsWithTag("HouseSpot");
        for (int i = 0; i < houseSpots.Length; i++)
        {
            maxTier = Mathf.Max(maxTier, houseSpots[i].GetComponent<HouseSpot>().houseTiers.Count - 1);
        }


        if(!IsNextTierAviable())
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            currentTier++;
			if (currentTier == 1)
			{
				currentTowers[0] = (lvlmanager.tower1bCount);
				currentTowers[1] = (lvlmanager.tower2bCount);
				currentTowers[2] = (lvlmanager.tower3bCount);
			}
			else
			{
				currentTowers[0] = (lvlmanager.tower1cCount);
				currentTowers[1] = (lvlmanager.tower2cCount);
				currentTowers[2] = (lvlmanager.tower3cCount);
			}
			setTier(currentTier);
        }

		lvlmanager = GameObject.FindGameObjectWithTag("LVLmanager").GetComponent<LVLsettings>();
        currentTowers[0] = (lvlmanager.tower1aCount);
        currentTowers[1] = (lvlmanager.tower2aCount);
        currentTowers[2] = (lvlmanager.tower3aCount);

        nextScene = lvlmanager.nextScene;

		UpdateNumbers();


	}

	public bool IsNextTierAviable()
    {
        int maxTier = 0;

        // Check if next tier aviable
        GameObject[] houseSpots = GameObject.FindGameObjectsWithTag("HouseSpot");
        for (int i = 0; i < houseSpots.Length; i++)
        {
            maxTier = Mathf.Max(maxTier, houseSpots[i].GetComponent<HouseSpot>().houseTiers.Count - 1);
        }

        return currentTier < maxTier;
    }

    public void showForbiddenZones(bool show)
    {
        GameObject[] forbiddenZones = GameObject.FindGameObjectsWithTag("Forbidden");
        for (int i = 0; i < forbiddenZones.Length; i++)
        {
            SpriteRenderer sr = forbiddenZones[i].GetComponentInChildren<SpriteRenderer>();

            if (show)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
            else
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            }

        }

    }


}
