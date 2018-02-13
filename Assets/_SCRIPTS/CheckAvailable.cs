using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAvailable : MonoBehaviour {


	[SerializeField]
	private GameObject tower;

    public GameObject rotator;

	[SerializeField]
	private TowerSpawner towerspawner;
	private TowerScript ts;
	void Start () {
		if (tower == null) {
			Debug.LogError("Tower prefab not assigned");
			return;
		}
        ts = tower.GetComponent<TowerScript>();
		if (ts == null) {
			Debug.LogError("Tower prefab doesnt have TowerScript");
		}
    }

	public void StartBuildingTowerClick()
	{
		if (GameManager.IS_MOBILE) return;
		Debug.Log("StartBuildingTowerClick");
		if (GameManager.currentTowers[ts.id] > 0)
		{
            float angle = 0;
            if(rotator != null)
            {
                angle = rotator.GetComponent<RotateIcon>().angle;
            }
			towerspawner.spawn(tower, angle);
        }
	}

	public void StartBuildingTowerDrag()
	{
		if (!GameManager.IS_MOBILE) return;
		Debug.Log("StartBuildingTowerDrag");
		if (GameManager.currentTowers[ts.id] > 0)
		{
            float angle = 0;
            if(rotator != null)
            {
                angle = rotator.GetComponent<RotateIcon>().angle;
            }
			towerspawner.spawn(tower, angle);
        }
	}

	public void FinishBuildingTowerDrag() {
		if (!GameManager.IS_MOBILE) return;
		Debug.Log("FinishBuildingTowerDrag");
		towerspawner.PlaceTowerOrCancel();
	}
}
