using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {

    public GameObject powerZone;
    public List<GameObject> powered;
    public int id;

    public bool isBuildable;

    public bool isBuilded;

    // Use this for initialization
    void Start () {
        powered = new List<GameObject>();

        setPowerRotation(270);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onBuilded()
    {
        isBuilded = true;
		GameManager.instance.towerBuilt.Play();

        setPowerUps();
    }

    public void cantBuildFlash()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(FlashSprites(sprites, 3, 0.075f));
    }

    public void onDestroyed()
    {
        isBuilded = false;
		GameManager.instance.destroy.Play();
		for (var i = 0; i < powered.Count; i++)
        {
            powered[i].GetComponent<HouseScript>().powerDown();
        }
    }

    public void setPowerUps()
    {
        for (var i = 0; i < powered.Count; i++)
        {
            powered[i].GetComponent<HouseScript>().powerUp();
        }
    }

    void setPowerRotation(float angle)
    {
        powerZone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    IEnumerator FlashSprites(SpriteRenderer[] sprites, int numTimes, float delay, bool disable = false)
    {
        // number of times to loop
        for (int loop = 0; loop < numTimes; loop++)
        {
            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++)
            {
                if (disable)
                {
                    // for disabling
                    sprites[i].enabled = false;
                }
                else
                {
                    // for changing the alpha
                    
                    sprites[i].color = new Color(1, 0, 0, 1);
                }
            }

            // delay specified amount
            yield return new WaitForSeconds(delay);

            // cycle through all sprites
            for (int i = 0; i < sprites.Length; i++)
            {
                if (disable)
                {
                    // for disabling
                    sprites[i].enabled = true;
                }
                else
                {
                    // for changing the alpha
                    sprites[i].color = new Color(1, 1, 1, 1);
                }
            }

            // delay specified amount
            yield return new WaitForSeconds(delay);
        }
    }
}
