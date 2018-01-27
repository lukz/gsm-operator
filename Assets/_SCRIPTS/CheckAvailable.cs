using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAvailable : MonoBehaviour {

	[SerializeField]
	private int numberTower;

	[SerializeField]
	private GameObject tower;



	[SerializeField]
	private TowerSpawner towerspawner;

	public void SetTower()
	{
		if (GameManager.currentTowers[numberTower] > 0)
		{
			//TODO dopiero na budowe zmieniaj liczbe/.....
			GameManager.currentTowers[numberTower]--;
			GameManager.UpdateNumbers();
			towerspawner.spawn(tower);

		}
	}

}
