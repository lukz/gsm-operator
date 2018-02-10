using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenScript : MonoBehaviour
{

    private List<GameObject> colidees;
    private FlashSpriteScript fss;
    private TowerScript towerScript;
    // Use this for initialization
    void Start()
    {
        colidees = new List<GameObject>();
        fss = GetComponent<FlashSpriteScript>();
        towerScript = transform.parent.GetComponent<TowerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (colidees.Count == 0)
        {
            towerScript.isBuildable = true;
            if (fss) {
                fss.disable();
            }
        }
        else
        {
            towerScript.isBuildable = false;
            if (fss) {
                fss.enable();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!colidees.Contains(other.gameObject))
        {
            colidees.Add(other.gameObject);
            FlashSpriteScript flashSpriteScript = other.gameObject.GetComponent<FlashSpriteScript>();
            if (flashSpriteScript) {
                flashSpriteScript.enable();
            }
        }

        if (transform.parent.GetComponent<TowerScript>().isBuilded && other.transform.parent.tag == "House")
        {
            transform.parent.GetComponent<TowerScript>().onDestroyed();
            GameObject.Destroy(transform.parent.gameObject);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (colidees.Contains(other.gameObject))
        {
            colidees.Remove(other.gameObject);
            FlashSpriteScript flashSpriteScript = other.gameObject.GetComponent<FlashSpriteScript>();
            if (flashSpriteScript) {
                flashSpriteScript.disable();
            }
        }
    }


}
