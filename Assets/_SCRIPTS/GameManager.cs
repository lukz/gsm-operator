using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static bool IS_ANDROID = Application.platform == RuntimePlatform.Android;
	public static bool IS_IOS = Application.platform == RuntimePlatform.IPhonePlayer;
	public static bool IS_MOBILE = IS_ANDROID || IS_IOS;
	// public static bool IS_MOBILE = true;

	public static bool canDoActions = true;

	public AudioSource music;
	public AudioSource winPhase;
	public AudioSource winLvl;
	public AudioSource btnClick;
	public AudioSource destroy;
	public AudioSource towerBuilt;
	public AudioSource deny;

	public GameObject splash;

	public static TowerSet currentTowers = new TowerSet();

	[SerializeField]
	private Text sphereTowerCount;
	[SerializeField]
	private Text coneTowerCount;
	[SerializeField]
	private Text rayTowerCount;
	public static float shakePower = 0;
	[SerializeField]
	private Slider powerSlider;
	private int currentLvl;
	private string currentLvlName;

	LVLsettings lvlmanager;
	private int currentTier = 0;
	public static GameManager instance = null;
	public float powerUpTimeForSceneChange;
	private bool splashShown = false;
	private bool soundOn = true;
	public bool restarting = false;
	[SerializeField]
	private float delayBetweenTowerRestart = 0.15f;
	private float timerTowerRestart = 0f;
	List<GameObject> towersToDestroy = new List<GameObject>();
	int currentlyDestroyedTower;
	private TowerSpawner towerspawner;
	public static GameObject towersContainer;
	float timer;

	public bool OPENlastLEVEL;

	private int MAXLVLS = 2;


	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			// TODO actual volume?
			toggleSound(Prefs.GetMasterVolume() > 0);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}
	void Start()
	{
		SceneManager.sceneLoaded += PrepareScene;
		SaveControl.instance.Load();
		towerspawner = GetComponent<TowerSpawner>();
		if (OPENlastLEVEL)
		{
			int lastUnlockedLevel = 0;
			if (SaveControl.instance.towersUsedToWin.Count > 0) // wygral chociaz 1 poziom
			{
				lastUnlockedLevel = SaveControl.instance.towersUsedToWin.Count;
				if (SaveControl.instance.towersUsedToWin.Count >= MAXLVLS)
				{
					lastUnlockedLevel = 0;
				}
			}
			string lvl = "LVL" + lastUnlockedLevel.ToString(); // zaladuj ostatni dostepny poziom
			SceneManager.LoadScene(lvl);

		}
		else
		{
			PrepareScene(SceneManager.GetSceneByName("main"), LoadSceneMode.Single);
		}
		canDoActions = true;

	}

	void PrepareScene(Scene scene, LoadSceneMode mode)
	{
		music.volume = 0.8f;
		canDoActions = true;
		splashShown = false;

		currentTier = 0;

		if (currentLvl == MAXLVLS + 1)
		{
			StartCoroutine(ChangeLvlTo1());
		}else {
			lvlmanager = GameObject.FindGameObjectWithTag("LVLmanager").GetComponent<LVLsettings>();
			currentLvl = lvlmanager.level;
			currentLvlName = lvlmanager.levelName;
			towersContainer = GameObject.FindGameObjectWithTag("TowersContainer");

			currentTowers.sphereCount = lvlmanager.GetSphereCount(currentTier);
			currentTowers.coneCount = lvlmanager.GetConeCount(currentTier);
			currentTowers.rayCount = lvlmanager.GetRayCount(currentTier);

			setTier(currentTier);
			UpdateNumbers();
		}
	}

	IEnumerator ChangeLvlTo1()
	{
		while (true)
		{
			yield return new WaitForSeconds(3);
			currentLvl = 1;
			SceneManager.LoadScene("LVL1");
			StopCoroutine(ChangeLvlTo1());
		}
	}

	public void Restart()
	{
		if (!canDoActions) return;
		if (!restarting)
		{
			GameObject[] towersTemp = GameObject.FindGameObjectsWithTag("Tower");
			towersToDestroy = new List<GameObject>();
			for (int i = 0; i < towersTemp.Length; i++)
			{
				if (towersTemp[i].GetComponent<TowerScript>().playerTower)
				{
					towersToDestroy.Add(towersTemp[i]);
				}
			}
			timerTowerRestart = delayBetweenTowerRestart;
			GameManager.instance.btnClick.Play();
			currentlyDestroyedTower = 0;
			if (towersToDestroy.Count > 0)
			{
				restarting = true;
			}

		}
	}
	public void toggleSound()
	{
		toggleSound(!soundOn);
	}

	public void toggleSound(bool enabled)
	{
		Debug.Log("Toggle sound " + enabled);
		soundOn = enabled;
		music.mute = !enabled;
		winPhase.mute = !enabled;
		winLvl.mute = !enabled;
		btnClick.mute = !enabled;
		destroy.mute = !enabled;
		towerBuilt.mute = !enabled;
		deny.mute = !enabled;

		Prefs.SetMasterVolume(soundOn?1:0);
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
		if (instance.sphereTowerCount)
		{
			int count = currentTowers.sphereCount;
			instance.sphereTowerCount.text = count.ToString();
			instance.sphereTowerCount.GetComponent<Transform>().parent.GetComponent<Button>().enabled = count > 0;
		}
		if (instance.coneTowerCount)
		{
			int count = currentTowers.coneCount;
			instance.coneTowerCount.text = count.ToString();
			instance.coneTowerCount.GetComponent<Transform>().parent.GetComponent<Button>().enabled = count > 0;
		}
		if (instance.rayTowerCount)
		{
			int count = currentTowers.rayCount;
			instance.rayTowerCount.text = count.ToString();
			instance.rayTowerCount.GetComponent<Transform>().parent.GetComponent<Button>().enabled = count > 0;
		}
		//instance.countTowerLine.text = currentTowers[2].ToString();
	}
	void ShakeScreen()
	{
		if (shakePower > 0.01f)
		{
			shakePower *= 0.6f;

			if (Random.Range(0, 1) > 0.5f)
			{
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
	}
	void HandleRestart()
	{
		if (restarting)
		{
			if (towersToDestroy.Count > 0)
			{
				timerTowerRestart -= Time.deltaTime;
				if (timerTowerRestart <= 0)
				{
					timerTowerRestart = delayBetweenTowerRestart;
					towerspawner.destroyTower(towersToDestroy[currentlyDestroyedTower]);
					currentlyDestroyedTower++;
					if (currentlyDestroyedTower >= towersToDestroy.Count)
					{
						restarting = false;
					}
				}
			}
		}
	}

	void Update()
	{

		ShakeScreen();
		HandleRestart();

		GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
		if (houses.Length > 0)
		{
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

			if (allPowered)
			{
				if (!splashShown)
				{
					Debug.Log("test! " + currentTier);
					splashShown = true;
					canDoActions = false;

					GameObject newSplash = Instantiate(splash);
					/*if (IsNextTierAviable())
					{
						newSplash.GetComponent<SplashScript>().setSplash(currentTier);
					}
					else
					{*/
					newSplash.GetComponent<SplashScript>().setEndSplash();
					//}
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
				music.volume = Mathf.Max(0.5f, (powerUpTimeForSceneChange - timer) * 0.8f);

				if (timer >= powerUpTimeForSceneChange)
				{
					Debug.Log("Full power");
					CheckSave();
					changeTierOrScene();
					timer = 0;
				}
			}
			else
			{
				timer = 0;
			}
		}
	}

	private void CheckSave()
	{
		GameObject[] towersTemp = GameObject.FindGameObjectsWithTag("Tower");
		if (currentLvl == SaveControl.instance.towersUsedToWin.Count)
		{
			SaveControl.instance.towersUsedToWin.Add(towersTemp.Length);
		}
		if (SaveControl.instance.towersUsedToWin[currentLvl] > towersTemp.Length)
			SaveControl.instance.towersUsedToWin[currentLvl] = towersTemp.Length;

		SaveControl.instance.Save();
	}

	public void changeTierOrScene()
	{
		CheckSave();
		/*int maxTier = 0;

		// Check if next tier aviable
		GameObject[] houseSpots = GameObject.FindGameObjectsWithTag("HouseSpot");
		for (int i = 0; i < houseSpots.Length; i++)
		{
			maxTier = Mathf.Max(maxTier, houseSpots[i].GetComponent<HouseSpot>().houseTiers.Count - 1);
		}*/


		//if (!IsNextTierAviable())
		//{

		NextScene(true);
		/*}
		else
		{
			currentTier++;
			currentTowers.sphereCount += lvlmanager.GetSphereCount(currentTier);
			currentTowers.coneCount += lvlmanager.GetConeCount(currentTier);
			currentTowers.rayCount += lvlmanager.GetRayCount(currentTier);

			setTier(currentTier);
		}*/
		UpdateNumbers();
	}

	/*public bool IsNextTierAviable()
	{
		int maxTier = 0;

		// Check if next tier aviable
		GameObject[] houseSpots = GameObject.FindGameObjectsWithTag("HouseSpot");
		Debug.Log("Spots found: " + houseSpots);
		for (int i = 0; i < houseSpots.Length; i++)
		{
			maxTier = Mathf.Max(maxTier, houseSpots[i].GetComponent<HouseSpot>().houseTiers.Count - 1);
		}

		Debug.Log("Max tier: " + currentTier + " / " + maxTier);

		return currentTier < maxTier;
	}*/


	public void NextScene(bool wonLvl = false)
	{
		if (currentLvl < SaveControl.instance.towersUsedToWin.Count)
		{
			if (currentLvl == MAXLVLS)
			{
				if (wonLvl)
				{
					currentLvl++;
					SceneManager.LoadScene("END");
				}
			}
			else
			{
				string nextScene = (++currentLvl).ToString();
				SceneManager.LoadScene("LVL" + nextScene);
			}
		}
	}

	public void PrevScene()
	{
		if (currentLvl > 0)
		{
			string nextScene = (--currentLvl).ToString();
			SceneManager.LoadScene("LVL" + nextScene);
		}
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
	void OnDestroy() {
		if (instance == this) {
        	Debug.Log("GameManager nuked");
			Prefs.Save();
			instance = null;
		}
    }
}
