using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRocks : MonoBehaviour {
    public bool areBlocking = true;
    private SpriteRenderer renderer;

    // Use this for initialization
    void Start ()
    {
        renderer = transform.GetComponent<SpriteRenderer>();

        SetBlocking(areBlocking);

	}


    public void ToggleState()
    {
        SetBlocking(!areBlocking);
    }

    public void SetBlocking(bool shouldBlock)
    {
        areBlocking = shouldBlock;

        if(areBlocking)
        {
            GetComponent<Animator>().SetBool("on", true);
            gameObject.tag = "Blocker";
        } else {
            GetComponent<Animator>().SetBool("on", false);
            gameObject.tag = "Untagged";
        }

    }

    public void Show()
    {
        renderer.enabled = true;
    }

    public void Hide()
    {
        renderer.enabled = false;
    }
}
