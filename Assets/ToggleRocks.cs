using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRocks : MonoBehaviour {



	[SerializeField]
	private Sprite sprite_off;

	[SerializeField]
	private Sprite sprite_on;

    public bool areBlocking = true;
    private SpriteRenderer renderer;

    // Use this for initialization
    void Start ()
    {
        renderer = transform.GetComponent<SpriteRenderer>();

        SetBlocking(areBlocking);

	}
	
	// Update is called once per frame
	void Update () {
		
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
            renderer.sprite = sprite_on;
            gameObject.tag = "Blocker";
        } else {
            renderer.sprite = sprite_off;
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
