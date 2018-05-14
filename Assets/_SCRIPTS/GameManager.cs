using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{

	public static bool SOFTLAUNCH = false; //TODO REMEMBER MAKE THIS FALSE before official final release



	[SerializeField]
	private GameObject powerStickerPrefab;

	public static bool IS_ANDROID = Application.platform == RuntimePlatform.Android;
	public static bool IS_IOS = Application.platform == RuntimePlatform.IPhonePlayer;
	public static bool IS_MOBILE = IS_ANDROID || IS_IOS;
	//	 public static bool IS_MOBILE = true;

	public static bool canDoActions = true;


	public GameObject splash;
	public GameObject tint;

	[SerializeField]
	public static float shakePower = 0;

	[SerializeField]
	private GameObject restartFlareFx;

	[SerializeField]
	private Slider powerSlider;
	private int currentLvl;

	LVLsettings lvlmanager;
	public static GameManager instance = null;
	public float powerUpTimeForSceneChange;
	private bool tintShwon = false;

	// List<GameObject> towersToDestroy = new List<GameObject>();
	int currentlyDestroyedTower;
	private TowerSpawnerPro towerspawner;
	public static GameObject towersContainer;
	float timer;

	public bool OPENlastLEVEL;


	private float timeOnLevel = 0;
	private float backs = 0;

	[SerializeField]
	private int lastLevelId;// how many lvls in game, minus something


	private int firstLockedButton = 0;
	public EventTriggerProxy[] towerButtons = new EventTriggerProxy[5];
	private List<ButtonTowerPair> buildTowers = new List<ButtonTowerPair>();

	public Button nextLevelButton;
	public Button prevLevelButton;
	public Button restartButton;

	public GameObject towerDragTutorial;
	private GameObject towerDragTutorialInstance;

	private int powerNeeded = 0;
	private GameObject[] houses;
	private List<PowerSticker> powerStickers = new List<PowerSticker>();

	[SerializeField]
	private GameObject topPowerStickerPosition;
	[SerializeField]
	private GameObject bottomStickerPosition;
	[SerializeField]
	private GameObject winAnimationPowerBar;

	private bool preparedScene = false;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Transform oldSplash = transform.Find("YearSplash");
			if (oldSplash != null)
			{
				oldSplash.gameObject.SetActive(true);
			}
			Input.multiTouchEnabled = false;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			instance.Reset();
			Destroy(gameObject);
		}
	}

	void Start()
	{
		if (IS_MOBILE)
		{
			float aspect = Screen.width / (float)Screen.height;
			Screen.SetResolution(Mathf.FloorToInt(600 * aspect), 600, true);
		}

		Application.targetFrameRate = 30;
		DOTween.Init(true, true, LogBehaviour.Verbose);

		SceneManager.sceneLoaded += PrepareScene;
		SaveControl.instance.Load();
		towerspawner = GetComponent<TowerSpawnerPro>();

		if (OPENlastLEVEL)
		{
			int lastUnlockedLevel = SaveControl.instance.LastWonLevel();
			if (lastUnlockedLevel >= lastLevelId)
			{
				lastUnlockedLevel = 0;
			}
			string lvl = "LVL" + lastUnlockedLevel; // zaladuj ostatni dostepny poziom
			SceneManager.LoadScene(lvl);

		}
		else
		{
			PrepareScene(SceneManager.GetSceneByName("main"), LoadSceneMode.Single);
		}


		canDoActions = true;
	}

	void Reset()
	{
		towerspawner.Reset();
		foreach (var tb in towerButtons)
		{
			tb.Reset();
		}
	}

	public void TowerBuild(EventTriggerProxy button, GameObject tower)
	{
		buildTowers.Add(new ButtonTowerPair(button, tower));
		UnlockNextTower();
	}
	void LockCurrentTower()
	{
		if (firstLockedButton - 1 <= lvlmanager.allTowers)
		{
			restartFlareFx.SetActive(false);
		}

		Debug.Log("Lock " + (firstLockedButton - 1));
		if (firstLockedButton > 0)
		{
			--firstLockedButton;
			if (firstLockedButton <= 4)
			{
				towerButtons[firstLockedButton].Lock();
			}
			if (firstLockedButton <= 1)
			{
				restartButton.interactable = false;
			}
		}
	}
	void UnlockNextTower()
	{
		if (firstLockedButton >= lvlmanager.allTowers)
		{
			restartFlareFx.SetActive(true);
		}

		if (firstLockedButton < 5)
		{
			towerButtons[firstLockedButton].Unlock();
		}
		if (firstLockedButton > 0)
		{
			restartButton.interactable = true;
		}
		firstLockedButton++;
		firstLockedButton = Mathf.Min(firstLockedButton, 6);
		restartButton.interactable = true;
	}

	void PrepareScene(Scene scene, LoadSceneMode mode)
	{

		buildTowers = new List<ButtonTowerPair>();
		firstLockedButton = 0;
		canDoActions = true;
		tintShwon = false;

		if (currentLvl == lastLevelId + 1)
		{
			if (!SOFTLAUNCH)
				Invoke("ChangeLvlTo1", 15f);
		}
		else
		{
			lvlmanager = GameObject.FindGameObjectWithTag("LVLmanager").GetComponent<LVLsettings>();
			currentLvl = lvlmanager.level;
			towersContainer = GameObject.FindGameObjectWithTag("TowersContainer");
			towerspawner.towerContainer = towersContainer;
			towerspawner.tileset = GameObject.FindGameObjectWithTag("Tileset").GetComponent<Tileset>();

			for (int i = 0; i < 5; i++)
			{
				towerButtons[i].Lock();
			}
			Transform oldSplash = transform.Find("YearSplash");
			if (oldSplash != null)
			{
				Destroy(oldSplash.gameObject);
			}
			GameObject newSplash = Instantiate(splash);
			newSplash.GetComponent<YearSplashScript>().ShowSplash(lvlmanager.levelName);

			UnlockNextTower();
			prevLevelButton.interactable = currentLvl > 0;
			nextLevelButton.interactable = currentLvl < SaveControl.instance.LastWonLevel();
			restartButton.interactable = false;
			timeOnLevel = 0;
			backs = 0;

			// add and nuke tutorial as needed
			// if (currentLvl == 0 && !nextLevelButton.interactable) {
			if (currentLvl == 0)
			{
				towerDragTutorialInstance = GameObject.Instantiate(towerDragTutorial);

				// conflicts with Analytics
				UnityEngine.EventSystems.EventTrigger trigger = towerButtons[0].GetComponent<UnityEngine.EventSystems.EventTrigger>();
				UnityEngine.EventSystems.EventTrigger.Entry entry = new UnityEngine.EventSystems.EventTrigger.Entry();
				entry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
				entry.callback.AddListener((data) =>
				{
					if (towerDragTutorialInstance != null)
					{
						GameObject.Destroy(towerDragTutorialInstance);
						towerDragTutorialInstance = null;
					}
				});
				trigger.triggers.Add(entry);
			}
		}
		Sounds.PlayStartLevel();


		showWhiteTint(false);

		houses = GameObject.FindGameObjectsWithTag("House");

		powerNeeded = 0;
		foreach (GameObject house in houses)
		{
			HouseScript hs = house.GetComponent<HouseScript>();
			powerNeeded += hs.requiredPower;
		}

		for (int i = 0; i < powerStickers.Count; i++)
		{
			GameObject.Destroy(powerStickers[i].gameObject);
		}
		powerStickers = new List<PowerSticker>();
		for (int i = powerNeeded-1; i >=0 ; i--)
		{
			PowerSticker ps = GameObject.Instantiate(powerStickerPrefab).GetComponent<PowerSticker>();
			ps.transform.parent = gameObject.transform;
			
			if (i == 0)
			{
				powerStickers.Add(ps);
				ps.transform.position = topPowerStickerPosition.transform.position;
			}
			else
			{
				powerStickers.Add(ps);
				float dist = topPowerStickerPosition.transform.position.y - bottomStickerPosition.transform.position.y;
				float po = topPowerStickerPosition.transform.position.y - dist / (powerNeeded) * (i);
				Vector3 pos = new Vector3(topPowerStickerPosition.transform.position.x, po, topPowerStickerPosition.transform.position.z);
				ps.transform.position = pos;
			}



		}
		preparedScene = true;
		winAnimationPowerBar.SetActive(false);
	}

	void ChangeLvlTo1()
	{
		currentLvl = 1;
		SceneManager.LoadScene("LVL1");
	}
	public void Restart()
	{
		if (!canDoActions) return;

		towerspawner.ReturnTower();
		Sounds.PlayButtonClick();
		// timerTowerRestart = delayBetweenTowerRestart;
		if (buildTowers.Count > 0)
		{
			LockCurrentTower();
			ButtonTowerPair item = buildTowers[buildTowers.Count - 1];
			buildTowers.Remove(item);
			towerspawner.ReturnTower(item.button, item.tower, false);
			backs++;

            towerspawner.tileset.ToggleRocks();
        }
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

	private float cachedProgress;
	private bool allPowered;

	void Update()
	{
		timeOnLevel += Time.deltaTime;
		ShakeScreen();


		if (houses.Length > 0)
		{
			allPowered = false;
			float countPowered = 0;
			if (!preparedScene) return;
			foreach (GameObject house in houses)
			{
				HouseScript hs = house.GetComponent<HouseScript>();
				countPowered += Mathf.Min(hs.powered, hs.requiredPower);
			}
			if (countPowered >= powerNeeded) allPowered = true;

			//TODO czekaj na zmiane i sprawdz ile sie zmienilo
			for (int i = powerNeeded-1; i>=0; i--)
			{

				if (i < countPowered && !powerStickers[i].isPowered)
				{
					powerStickers[i].PowerUp();
				}
                else if(i >= countPowered && powerStickers[i].isPowered)
                {
                    powerStickers[i].PowerDown();
                }
			}

			float realProgress = (countPowered / powerNeeded)*0.85f;
			float swim = Mathf.Sin(Time.timeSinceLevelLoad * 1f);
			if (realProgress == 0 || realProgress == 1) swim = 0;
			float currentPercent = Mathf.Clamp(realProgress + swim / 100f * 1.5f, 0, 1);
			powerSlider.value = Mathf.Lerp(powerSlider.value, currentPercent, Time.deltaTime * 5);

			if (realProgress > cachedProgress)
			{
				powerSlider.gameObject.GetComponentInChildren<SliderFillScript>().FlashSlider();
			}
			cachedProgress = realProgress;

			if (allPowered)
			{
				if (!tintShwon)
				{
					winAnimationPowerBar.SetActive(true);
					//splashShown = true;
					canDoActions = false;
					tintShwon = true;

					//GameObject newSplash = Instantiate(splash);

					//newSplash.GetComponent<SplashScript>().setEndSplash();
					showWhiteTint(true);
					restartFlareFx.SetActive(false);

				}


				if (timer == 0)
				{
					Sounds.PlayWinLevel();
				}

				timer += Time.deltaTime;
				Sounds.VolumeMusic(Mathf.Max(0f, (powerUpTimeForSceneChange - timer * 3f)));

				if (timer >= powerUpTimeForSceneChange)
				{
					//Debug.Log("Full power");
					string nameEvent = "levelWin_" + lvlmanager.level.ToString();

					Analytics.CustomEvent(nameEvent, new Dictionary<string, object>
		{
			{ "timeToWin", timeOnLevel },
			{ "backs", backs }
		});

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

	private void showWhiteTint(bool fadeIn)
	{
		GameObject newTint = Instantiate(tint);

		if (fadeIn)
		{
			newTint.GetComponent<FullScreenTintScript>().fadeIn(1f);
		}
		else
		{

			newTint.GetComponent<FullScreenTintScript>().fadeOut();
		}

	}
	private void CheckSave()
	{
		GameObject[] towersTemp = GameObject.FindGameObjectsWithTag("Tower");
		int playerTowersCount = 0;
		foreach (var go in towersTemp)
		{
			TowerScript ts = go.GetComponent<TowerScript>();
			if (ts.playerTower)
			{
				playerTowersCount++;
			}
		}
		SaveControl.instance.UpdateTowers(currentLvl, playerTowersCount);
		SaveControl.instance.Save();
	}
	public void changeTierOrScene()
	{
		preparedScene = false;
		CheckSave();

		NextScene(true);
	}
	public void NextScene(bool wonLvl = false)
	{
		if (!wonLvl)
		{
			// already changing
			if (allPowered) return;


		}
		if (currentLvl < SaveControl.instance.LastWonLevel())
		{
			if (currentLvl == lastLevelId)
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
				Sounds.VolumeMusic(0);
			}
		}
	}
	public void PrevScene()
	{

		if (allPowered) return;

		if (currentLvl > 0)
		{
			string nextScene = (--currentLvl).ToString();
			Sounds.VolumeMusic(0);
			SceneManager.LoadScene("LVL" + nextScene);
		}
	}

	void OnDestroy()
	{
		if (instance == this)
		{
			Debug.Log("GameManager nuked");
			Prefs.Save();
			instance = null;
		}
	}

	[System.Serializable]
	class ButtonTowerPair
	{
		public EventTriggerProxy button;
		public GameObject tower;
		public ButtonTowerPair(EventTriggerProxy button, GameObject tower)
		{
			this.button = button;
			this.tower = tower;
		}
	}


}
