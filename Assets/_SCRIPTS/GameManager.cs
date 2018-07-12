using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
//using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{


	public static bool IS_ANDROID = Application.platform == RuntimePlatform.Android;
	public static bool IS_IOS = Application.platform == RuntimePlatform.IPhonePlayer;
	public static bool IS_MOBILE = IS_ANDROID || IS_IOS;
	//	 public static bool IS_MOBILE = true;

	public static bool canDoActions = true;


	public GameObject splash;
	public GameObject tint;

	[SerializeField]
	private GameObject leftPanel;


	[SerializeField]
	private GameObject[] blooms;
	[SerializeField]
	private GameObject[] vignettes;
	[SerializeField]
	GameObject ground;
	[SerializeField]
	Sprite[] grounds;

	[SerializeField]
	public static float shakePower = 0;

	[SerializeField]
	private GameObject restartFlareFx;


	private int currentLvl;

	LVLsettings lvlmanager;
	public static GameManager instance = null;
	private float powerUpTimeForSceneChange;
	private bool tintShwon = false;

	// List<GameObject> towersToDestroy = new List<GameObject>();
	int currentlyDestroyedTower;
	private TowerSpawnerPro towerspawner;
	public static GameObject towersContainer;
	float timer;

	public bool OPENlastLEVEL;

	public float delayPowerFx;
	public float delayPowerLargeFx;
	public float timeOnLevel = 0;
	private float backs = 0;

	[SerializeField]
	private int lastLevelId;// how many lvls in game, minus something


	private int firstLockedButton = 0;
	public EventTriggerProxy towerButton;
	private List<ButtonTowerPair> buildTowers = new List<ButtonTowerPair>();

	public Button nextLevelButton;
	public Button prevLevelButton;
	public Button restartButton;

	public GameObject towerDragTutorial;
	public Transform towerDragTutorialContainer;
	private GameObject towerDragTutorialInstance;


	private GameObject[] houses;


	private bool preparedScene = false;

	public BoxController boxController;

	private GameObject lBloom;
	private GameObject rBloom;
	private GameObject cloudsVignette;

	[SerializeField]
	private GameObject MenuBtnHolder;

	private float restartButtonLockTimer = 0f;

	public  GameObject CrystalPrefab;
	public GameObject powerUpFlashEffect;
	public  GameObject WaterPrefab;
	public GameObject waterUpFlashEffect;

	private List<int[][]> backHistory = new List<int[][]>();
	private List<List<List<int>>> houseHistory = new List<List<List<int>>>();
	private bool savedOriginal;
	public float timeToSave = 0;

	public Sprite powerOn;
	public Sprite powerOff;
	public Sprite waterOff;
	public Sprite waterOn;


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
		towerButton.Reset();
	}

	public void ReturnTower()
	{
		boxController.ReturnTower();
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
		boxController.PrevTower();
		// boxController.ReturnTower();
		//Debug.Log("Lock " + (firstLockedButton - 1));
		// if (firstLockedButton > 0)
		// {
		// 	--firstLockedButton;
		// 	if (firstLockedButton <= 4)
		// 	{
		// 		towerButtons[firstLockedButton].Lock();
		// 	}
		// 	if (firstLockedButton <= 1)
		// 	{
		// 		restartButton.interactable = false;
		// 	}
		// }
	}
	void UnlockNextTower()
	{
		if (firstLockedButton >= lvlmanager.allTowers)
		{
			restartFlareFx.SetActive(true);
		}

		// if (firstLockedButton < 5)
		// {
		// 	towerButtons[firstLockedButton].Unlock();
		// }
		boxController.NextTower();
		firstLockedButton++;
		firstLockedButton = Mathf.Min(firstLockedButton, 6);
	}

	void PrepareScene(Scene scene, LoadSceneMode mode)
	{

		buildTowers = new List<ButtonTowerPair>();
		firstLockedButton = 0;
		canDoActions = true;
		tintShwon = false;

		if (currentLvl == lastLevelId + 1)
		{
			//	Invoke("ChangeLvlTo1", 15f);
		}
		else
		{
			lvlmanager = GameObject.FindGameObjectWithTag("LVLmanager").GetComponent<LVLsettings>();
			currentLvl = lvlmanager.level;
			towersContainer = GameObject.FindGameObjectWithTag("TowersContainer");
			towerspawner.towerContainer = towersContainer;
			towerspawner.tileset = GameObject.FindGameObjectWithTag("Tileset").GetComponent<Tileset>();
			savedOriginal = false;
			for (int h = 0; h < backHistory.Count; h++)
			{
				for (int i = 0; i < backHistory[h].Length; i++)
				{
					
						backHistory[h][i] = null;
					
				}
				
			}
			houseHistory.Clear();
			backHistory.Clear();

			boxController.Restart(lvlmanager);
			// for (int i = 0; i < 5; i++)
			// {
			// 	   towerButtons[i].Lock();
			// }
			Transform oldSplash = transform.Find("YearSplash");
			if (oldSplash != null)
			{
				Destroy(oldSplash.gameObject);
			}
			GameObject newSplash = Instantiate(splash);
			newSplash.GetComponent<YearSplashScript>().ShowSplash(lvlmanager.levelName);
			newSplash.GetComponent<YearSplashScript>().bg.transform.localScale = new Vector3(GetComponent<CameraResize>().widX, 7);

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
				towerDragTutorialInstance = GameObject.Instantiate(towerDragTutorial, towerDragTutorialContainer);

				// conflicts with Analytics
				UnityEngine.EventSystems.EventTrigger trigger = towerButton.GetComponent<UnityEngine.EventSystems.EventTrigger>();
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


		showWhiteTint(false, 0);

		houses = GameObject.FindGameObjectsWithTag("House");


		preparedScene = true;

		GameObject tempS;
		GameObject vig;

		if (lBloom)
		{
			GameObject.Destroy(lBloom);
			GameObject.Destroy(cloudsVignette);
		}

		if (currentLvl <= 7 || currentLvl > 44)
		{
			ground.GetComponent<SpriteRenderer>().sprite = grounds[0];

			if (currentLvl > 44)
				ground.GetComponent<SpriteRenderer>().sprite = grounds[3];

			tempS = blooms[0];
			vig = vignettes[0];
		}
		else
		{
			if (currentLvl < 12)
			{
				ground.GetComponent<SpriteRenderer>().sprite = grounds[1];
				vig = vignettes[1];
				tempS = blooms[1];
			}
			else
			{
				ground.GetComponent<SpriteRenderer>().sprite = grounds[2];
				vig = vignettes[2];
				tempS = blooms[2];
			}
		}
		lBloom = GameObject.Instantiate(tempS, transform);
		cloudsVignette = GameObject.Instantiate(vig, transform);
		cloudsVignette.transform.localPosition = new Vector3(-0.2f, -1f, 10f);
		lBloom.transform.position = new Vector3(0, 0, 0);
		lBloom.transform.localScale = new Vector3(GetComponent<CameraResize>().widX, 8,1);

	}

	void ChangeLvlTo1()
	{
		currentLvl = 1;
		SceneManager.LoadScene("LVL1");
	}

	private void SaveHistory()
	{

		int[][] state = new int[4][];
		for (int i = 0; i < 4; i++)
		{
			state[i] = new int[5];
			for (int j = 0; j < 5; j++)
			{
				state[i][j] = towerspawner.tileset.tiles[i].row[j].GetComponent<Tile>().powerLvl;
			}
		}
		backHistory.Add(state);

		List<List<int>> housesStates = new List<List<int>>();
		for (int i = 0; i < houses.Length; i++)
		{
			List<int> tempA = new List<int>();
			List<int> tempB = houses[i].GetComponent<HouseScript>().currentResources;
			for (int n = 0; n < tempB.Count; n++)
			{
				tempA.Add(tempB[n]);
			}
			housesStates.Add(tempA);
		}
		houseHistory.Add(housesStates);
		Debug.Log(houseHistory.Count);

	}

	public void Restart()
	{
		if (!canDoActions || restartButtonLockTimer > 0) return;

		towerspawner.ReturnTower();
		Sounds.PlayButtonClick();

		if (buildTowers.Count > 0)
		{
			LockCurrentTower();
			ButtonTowerPair item = buildTowers[buildTowers.Count - 1];
			buildTowers.Remove(item);
			towerspawner.ReturnTower(item.button, item.tower);
			backs++;

			towerspawner.tileset.ToggleRocks();

			firstLockedButton--;


			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					towerspawner.tileset.tiles[i].row[j].GetComponent<Tile>().PowerChange(null,backHistory[(backHistory.Count - 2)][i][j],true);
				}
			}
			backHistory.RemoveAt(backHistory.Count - 1);

			for (int i = 0; i < houses.Length; i++)
			{
				List<int> tempA = houseHistory[houseHistory.Count - 2][i];
				List<int> tempB = new List<int>();
				for (int n = 0; n < tempA.Count; n++)
				{
					tempB.Add(tempA[n]);
				}
				houses[i].GetComponent<HouseScript>().currentResources = tempB;
				houses[i].GetComponent<HouseScript>().UpdateResources(0);
			}
			houseHistory.RemoveAt(houseHistory.Count-1);
		}
	}

	public bool RestartAll()
	{
		if (!canDoActions) return false;
		if (buildTowers.Count == 0)
		{
			return towerspawner.ReturnTower();
		}
		towerspawner.ReturnTower();
		while (buildTowers.Count > 0)
		{
			LockCurrentTower();
			ButtonTowerPair item = buildTowers[buildTowers.Count - 1];
			buildTowers.Remove(item);
			towerspawner.ReturnTower(item.button, item.tower);
			backs++;

			towerspawner.tileset.ToggleRocks();
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				towerspawner.tileset.tiles[i].row[j].GetComponent<Tile>().PowerChange(null, backHistory[0][i][j], true);
			}
		}
		backHistory.Clear();

		for (int i = 0; i < houses.Length; i++)
		{
			List<int> tempA = houseHistory[0][i];
			List<int> tempB = new List<int>();
			for (int n = 0; n < tempA.Count; n++)
			{
				tempB.Add(tempA[n]);
			}
			houses[i].GetComponent<HouseScript>().currentResources = tempB;
			houses[i].GetComponent<HouseScript>().UpdateResources(0);
		}
		houseHistory.Clear() ;
		return true;

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
		HideMenu();
		if (timeOnLevel > 0.2f)
		{
			if (!savedOriginal)
			{
				SaveHistory();
				savedOriginal = true;
			}
		}

		if (restartButtonLockTimer > 0)
		{
			restartButtonLockTimer -= Time.deltaTime;
			if (restartButtonLockTimer <= 0 && firstLockedButton > 1)
			{
				restartButton.interactable = true;
			}
		}
		if(timeToSave > 0)
		{
			timeToSave -= Time.deltaTime; ;
			if (timeToSave <= 0)
			{
				SaveHistory();
			}
		}

		if (houses != null)
		{
			if (houses.Length > 0)
			{
				allPowered = true;
				if (!preparedScene) return;
				foreach (GameObject house in houses)
				{
					HouseScript hs = house.GetComponent<HouseScript>();
					if (!hs.IsPowered()) allPowered = false;
				}

				if (allPowered)
				{
					if (!tintShwon)
					{
						//splashShown = true;
						canDoActions = false;
						tintShwon = true;

						//GameObject newSplash = Instantiate(splash);

						//newSplash.GetComponent<SplashScript>().setEndSplash();
						float whiteTintDelay = HouseScript.currentMarkerDelay + 2.5f;
						showWhiteTint(true, whiteTintDelay);

						restartFlareFx.SetActive(false);

						FinishedTweens();

						powerUpTimeForSceneChange = whiteTintDelay + 2.5f;

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

						/*Analytics.CustomEvent(nameEvent, new Dictionary<string, object>
		{
			{ "timeToWin", timeOnLevel },
			{ "backs", backs }
		});*/

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
	}

	private void FinishedTweens()
	{
		for (int i = 0; i < towerspawner.tileset.tiles.Count; i++)
		{
			for (int j = 0; j < towerspawner.tileset.tiles[i].row.Count; j++)
			{
				HouseScript marker = towerspawner.tileset.tiles[i].row[j].GetComponentInChildren<HouseScript>();
				if (marker != null) marker.MapFinishedTween();
			}

		}
	}

	private void showWhiteTint(bool fadeIn, float delay)
	{
		GameObject newTint = Instantiate(tint);

		if (fadeIn)
		{
			newTint.GetComponent<FullScreenTintScript>().fadeIn(delay);
		}
		else
		{

			newTint.GetComponent<FullScreenTintScript>().fadeOut(delay);
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

	public void ShowMenu()
	{
		if (MenuBtnHolder.transform.localPosition.x >= 0)
		{
			MenuBtnHolder.transform.DOKill();
			MenuBtnHolder.transform.DOLocalMoveX(-136, 0.2f);
		}
		else
		{
			MenuBtnHolder.transform.DOKill();
			MenuBtnHolder.transform.DOLocalMoveX(82, 0.2f);
		}
	}


	public void HideMenu()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			for (int i = 0; i < hit.Length; i++)
			{
				if (hit[i].collider != null && hit[i].collider.tag.Equals("BtnHolder"))
				{
					// hit bmenu, dont hide it
					return;
				}
			}
			MenuBtnHolder.transform.DOKill();
			MenuBtnHolder.transform.DOLocalMoveX(-136, 0.2f);
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

	public void lockRestartFor(float time)
	{
		restartButtonLockTimer = Mathf.Max(restartButtonLockTimer, time);
		restartButton.interactable = false;
	}


}
