using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAvailable : MonoBehaviour {


	[SerializeField]
	private GameObject tower;
	private int towerId = -1;

    public GameObject rotator;

	[SerializeField]
	private TowerSpawner towerspawner;
	void Start () {
		if (tower == null) {
			Debug.LogError("Tower prefab not assigned");
			return;
		}
        TowerScript ts = tower.GetComponent<TowerScript>();
		if (ts == null) {
			Debug.LogError("Tower prefab doesnt have TowerScript");
		} else {
			towerId = ts.id;
		}
    }

	public void SetTower()
	{
		if (GameManager.currentTowers[towerId] > 0)
		{
            float angle = 0;
            if(rotator != null)
            {
                angle = rotator.GetComponent<RotateIcon>().angle;
            }
			towerspawner.spawn(tower, angle);
        }
	}
}
