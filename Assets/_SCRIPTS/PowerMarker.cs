using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMarker : MonoBehaviour {
	 public Sprite powerOn;
    public Sprite powerOff;
    
	public bool showIfNoPower = false;
		
	public int power = 0;

	public int requiredPower = 0;

    private List<GameObject> markers = new List<GameObject>();
    private List<SpriteRenderer> markerRenderers = new List<SpriteRenderer>();

    public void SetPower(int power) 
    {
        if (power < 0) power = 0;
        if (this.power == power) return;
		this.power = power;
		Debug.Log("power = " + power);
        if (requiredPower == 0) {
            CreateMarkers(power);
        }
        foreach (var sr in markerRenderers)
        {
            sr.sprite = powerOff;
        }
        switch(power) {
            case 1: {
                markerRenderers[0].sprite = powerOn;
            } break;
            case 2: {
                markerRenderers[0].sprite = powerOn;
                markerRenderers[1].sprite = powerOn;
            } break;
            default: {
                markerRenderers[0].sprite = powerOn;
                markerRenderers[1].sprite = powerOn;
                markerRenderers[2].sprite = powerOn;
            } break;
        }
	}

	public void SetRequiredPower(int requiredPower) 
    {
        if (requiredPower < 0) requiredPower = 0;
        if (requiredPower > 3) requiredPower = 3;
        if (this.requiredPower == requiredPower) return;

        this.requiredPower = requiredPower;
        Debug.Log("req power = " + requiredPower);

        CreateMarkers(requiredPower);
    }

    private void CreateMarkers(int count) {
        foreach(var go in markers) {
            GameObject.Destroy(go);
        }
        markers.Clear();
        float y = .4f;
        // added in light up order
        switch(count) {
            case 1: {
                CreateMarker("marker1", 0f, y);
            } break;
            case 2: {
                CreateMarker("marker1", .1f, y);
                CreateMarker("marker2", -.1f, y);
            } break;
            case 3: {
                CreateMarker("marker1", .2f, y);
                CreateMarker("marker2", 0f, y);
                CreateMarker("marker3", -.2f, y);
            } break;
        }
    }

    private void CreateMarker(string name, float x, float y) 
    {
        Debug.Log("Create marker at " + x + ", " + y);
        GameObject m = new GameObject(name);
        m.transform.parent = transform;
        m.transform.position = new Vector2(transform.position.x +  x, transform.position.y + y);
        float scale = 2;
        m.transform.localScale = new Vector3(scale, scale, scale);
        markers.Add(m);

        SpriteRenderer sr = m.AddComponent<SpriteRenderer>();
        sr.sprite = powerOff;
        sr.sortingLayerName = "GUI";
        sr.sortingOrder = 0;

        markerRenderers.Add(sr);
    }
}
