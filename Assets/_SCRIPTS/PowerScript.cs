using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerScript : MonoBehaviour
{

    

    // Use this for initialization
    void Start()
    {
        // Zmienić on trigger enter żeby jak już się zbuduje to ustawiało odpowiednio
        // Dodać listę 
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (!transform.parent.GetComponent<TowerScript>().isBuilded) return;
        if (other.transform.parent.gameObject.tag != "House") return;

        List<GameObject> powered = transform.parent.GetComponent<TowerScript>().powered;

        if (!powered.Contains(other.transform.parent.gameObject))
        {
            powered.Add(other.transform.parent.gameObject);

            // Update for already builded
            if(transform.parent.GetComponent<TowerScript>().isBuilded)
            {
                transform.parent.GetComponent<TowerScript>().setPowerUps();
            }
            //other.transform.parent.GetComponent<HouseScript>().powerUp();
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.gameObject.tag != "House") return;

        List<GameObject> powered = transform.parent.GetComponent<TowerScript>().powered;
        
        if (powered.Contains(other.transform.parent.gameObject))
        {
            powered.Remove(other.transform.parent.gameObject);
        }
    }


}
