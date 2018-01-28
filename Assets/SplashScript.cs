using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScript : MonoBehaviour {

    public List<Sprite> splashes;
    public Sprite endGameSplash;

    public float lifeTime = 5;

    public GameObject tint;

    private float splashStartAlpha;
    private float tintStartAlpha;

    // Use this for initialization
    void Start () {

        splashStartAlpha = transform.GetComponent<SpriteRenderer>().color.a;
        tintStartAlpha = tint.GetComponent<SpriteRenderer>().color.a;
    }
	
	// Update is called once per frame
	void Update () {

        lifeTime -= Time.deltaTime;
        if(lifeTime < 1)
        {
            Color color = transform.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Min(splashStartAlpha, lifeTime);

            transform.GetComponent<SpriteRenderer>().color = color;

            Color color2 = tint.GetComponent<SpriteRenderer>().color;
            color2.a = Mathf.Min(tintStartAlpha, lifeTime);

            tint.GetComponent<SpriteRenderer>().color = color2;
        }

        if(lifeTime <= 0)
        {
            GameObject.Destroy(this);
        }

	}

    public void setSplash(int index)
    {
        transform.GetComponent<SpriteRenderer>().sprite = splashes[index];
    }

    public void setEndSplash()
    {
        transform.GetComponent<SpriteRenderer>().sprite = endGameSplash;
    }
}
