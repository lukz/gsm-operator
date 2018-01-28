using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpot : MonoBehaviour {

    public List<GameObject> houseTiers;

    public int asdas;

	// Use this for initialization
	void Start () {
        SpawnTier(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnTier(int tier)
    {
        if (tier >= houseTiers.Count) return;

        if (houseTiers[tier] == null) return;

        Clear();

        Instantiate(houseTiers[tier], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, transform); 
    }

    public void Clear()
    {
        HouseScript[] childreens = gameObject.GetComponentsInChildren<HouseScript>();

        for (int i = 0; i < childreens.Length; i++)
        {
            GameObject.Destroy(childreens[i].gameObject);
        }

    }

}
