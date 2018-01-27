using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerScript : MonoBehaviour
{

    private List<GameObject> powered;

    // Use this for initialization
    void Start()
    {
        powered = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!transform.parent.GetComponent<TowerScript>().isBuilded) return;
        if (other.transform.parent.gameObject.tag != "House") return;

        if (!powered.Contains(other.gameObject))
        {
            powered.Add(other.gameObject);

            other.transform.parent.GetComponent<HouseScript>().powerUp();
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (!transform.parent.GetComponent<TowerScript>().isBuilded) return;
        if (other.transform.parent.gameObject.tag != "House") return;

        if (powered.Contains(other.gameObject))
        {
            powered.Remove(other.gameObject);

            other.transform.parent.GetComponent<HouseScript>().powerDown();
        }
    }


}
