using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static bool IS_ANDROID = Application.platform == RuntimePlatform.Android;
	public static bool IS_IOS = Application.platform == RuntimePlatform.IPhonePlayer;
	public static bool IS_MOBILE = IS_ANDROID || IS_IOS;
	// public static bool IS_MOBILE = true;

	public static bool canDoActions = true;

	public Sounds sounds;

	public GameObject splash;
    public GameObject tint;

    [SerializeField]
	public static float shakePower = 0;

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

	private int MAXLVLS = 2;


	private int firstLockedButton = 0;
	public EventTriggerProxy[] towerButtons = new EventTriggerProxy[5];
	private List<ButtonTowerPair> buildTowers = new List<ButtonTowerPair>();

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

	void Start()
	{
		DOTween.Init(true, true, LogBehaviour.Verbose);

		SceneManager.sceneLoaded += PrepareScene;
		SaveControl.instance.Load();
		towerspawner = GetComponent<TowerSpawnerPro>();
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
		sounds.Prepare();
		
		Sounds.PlayMusic();
		canDoActions = true;

		UnlockNextTower();
	}

	public void TowerBuild(EventTriggerProxy button, GameObject tower) {
		buildTowers.Add(new ButtonTowerPair(button, tower));
		UnlockNextTower();
	}

	void LockCurrentTower(){
		Debug.Log("Lock " + (firstLockedButton -1));
		if (firstLockedButton > 0) {
			towerButtons[--firstLockedButton].Lock();
		}
	}

	void UnlockNextTower(){
		Debug.Log("Unlock " + (firstLockedButton));
		towerButtons[firstLockedButton++].Unlock();
	}

	void PrepareScene(Scene scene, LoadSceneMode mode)
	{
		canDoActions = true;
		tintShwon = false;

		if (currentLvl == MAXLVLS + 1)
		{
			StartCoroutine(ChangeLvlTo1());
		}else {
			lvlmanager = GameObject.FindGameObjectWithTag("LVLmanager").GetComponent<LVLsettings>();
			currentLvl = lvlmanager.level;
			towersContainer = GameObject.FindGameObjectWithTag("TowersContainer");
		}

		Sounds.PlayStartLevel();


        GameObject newSplash = Instantiate(splash);
        newSplash.GetComponent<YearSplashScript>().ShowSplash(currentLvl);

        showWhiteTint(false);
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
	
		towerspawner.ReturnTower();
		Sounds.PlayButtonClick();
		// timerTowerRestart = delayBetweenTowerRestart;
		if (buildTowers.Count > 0) {
			LockCurrentTower();
			ButtonTowerPair item = buildTowers[buildTowers.Count -1];
			buildTowers.Remove(item);
			towerspawner.ReturnTower(item.button, item.tower, false);
		
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

	void Update()
	{

		ShakeScreen();

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
				if (!tintShwon)
				{
					//splashShown = true;
					canDoActions = false;
                    tintShwon = true;

                    //GameObject newSplash = Instantiate(splash);

                    //newSplash.GetComponent<SplashScript>().setEndSplash();
                    showWhiteTint(true);
				}


				if (timer == 0)
				{
					Sounds.PlayWinLevel();
				}

				timer += Time.deltaTime;
				Sounds.VolumeMusic(Mathf.Max(0.5f, (powerUpTimeForSceneChange - timer) * 0.8f));

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

    private void showWhiteTint(bool fadeIn)
    {
        GameObject newTint = Instantiate(tint);

        if(fadeIn) {
            newTint.GetComponent<FullScreenTintScript>().fadeIn(1f);
        } else
        {
            newTint.GetComponent<FullScreenTintScript>().fadeOut();
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

		NextScene(true);
	}


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

	void OnDestroy() {
		if (instance == this) {
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
		public ButtonTowerPair(EventTriggerProxy button, GameObject tower) {
			this.button = button;
			this.tower = tower;
		}
    }
}
