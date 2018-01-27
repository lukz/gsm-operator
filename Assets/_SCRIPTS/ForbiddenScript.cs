using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenScript : MonoBehaviour
{

    private List<GameObject> colidees;

    // Use this for initialization
    void Start()
    {
        colidees = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (colidees.Count == 0)
        {
            transform.parent.GetComponent<TowerScript>().isBuildable = true;

        }
        else
        {
            transform.parent.GetComponent<TowerScript>().isBuildable = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!colidees.Contains(other.gameObject))
        {
            colidees.Add(other.gameObject);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (colidees.Contains(other.gameObject))
        {
            colidees.Remove(other.gameObject);
        }
    }


}
