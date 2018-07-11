using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TowerScript : MonoBehaviour {
    
    public int id;

	public bool playerTower = true;

    public List<PowerOffset> powerOffsets;

    private bool isAttachedToTile = false;
    private Tileset tileset;
    private Tile tile;
    private GameObject pump;
    
    public SpriteRenderer sprite;

    private Animator animator;
    void Start() 
    {
        animator = GetComponent<Animator>();
    }

    public void InitAsNotPlayers()
    {
        playerTower = false;
        animator = GetComponent<Animator>();
        // hack to disable mine with id == 3
        if (id < 3)
        {
            Tileset tileSet = FindObjectOfType<Tileset>();
            Tile tile = tileSet.GetTileAt(transform.position);
            if (tile != null)
            {
                tile.Build(gameObject);
                //AttachToTile(tile);
            }
            else
            {
                Debug.Log("No tile for tower at " + transform.position);
            }
        }
    }

    public Tile DetachFromTile()
    {

        if (!isAttachedToTile) {
            //Debug.Log("Not attached to tile");
			Sounds.PlayDeny();
            return tile;
        }
        isAttachedToTile = false;

        Sounds.PlayDestroy();
        //for (var i = 0; i < powered.Count; i++)
        ///{
        //    powered[i].GetComponent<HouseScript>().powerDown();
        //}
        Tile t = tile;
        tile = null;
        if (pump != null) {
            SpriteRenderer sr = pump.GetComponentInChildren<SpriteRenderer>();
            sr.DOFade(0, .2f).OnComplete(() => {
                // Debug.Log("Pump nuked " + pump.GetInstanceID());
                Destroy(pump);
            });
            pump = null;
        }
        if (animator != null) {
            animator.SetBool("off", true);
        }

        ToggleRocks toggleRocks = t.GetComponentInChildren<ToggleRocks>();
        if (toggleRocks != null) toggleRocks.Show();

        return t;
    }

    public bool AttachToTile(Tile tile)
    {

        if (isAttachedToTile) {
            Debug.Log("Already attached to tile");
            return false;
        }
        isAttachedToTile = true;

        this.tile = tile;
        tileset = tile.Tileset;
        PowerUp();

		if(playerTower) {
            Sounds.PlayTowerBuild();
        }
        //for (var i = 0; i < powered.Count; i++)
        //{
        //    powered[i].GetComponent<HouseScript>().powerUp();
        //}
        if (tile != null && tile.HasEnergyField() && Application.isPlaying) {
           /* TowerSpawnerPro tsp = FindObjectOfType<TowerSpawnerPro>();
            pump = GameObject.Instantiate(tsp.GetPumpPrefab(), tile.transform.position, Quaternion.identity, tile.transform);
            SpriteRenderer sr = pump.GetComponentInChildren<SpriteRenderer>();
            sr.color = new Color(1, 1, 1, 0);
            sr.DOFade(1, .2f);*/
            // Debug.Log("Pump created " + pump.GetInstanceID());
        }
        
        if (animator != null) {
            animator.SetBool("off", false);
        }

        ToggleRocks toggleRocks = tile.GetComponentInChildren<ToggleRocks>();
        if (toggleRocks != null) toggleRocks.Hide();

        return true;
    }


    public void PowerUp() 
    {
        if (tile == null || tileset == null) {
            tile = transform.GetComponentInParent<Tile>();
            tileset = tile.Tileset;
        }
		int powerChange = 1;
		if (tile.powerLvl < 0)
		{
			powerChange = -1;
		}
        tileset.ChangeTilesPower(this, tile, powerChange, powerOffsets);
    }

    private float mix;
    private int flashColorId = Shader.PropertyToID("_FlashColor");
    private int flashMixId = Shader.PropertyToID("_FlashMix");

    public void FlashRed()
    {
        sprite.material.SetColor(flashColorId, new Color(1, 0, 0, 0));

        float flashDuration = .3f;

        DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    {
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    1,
                    flashDuration * 0.1f
                ).SetEase(Ease.OutSine)
            )
            .Append(
                DOTween.To(
                    () => mix,
                    v =>
                    { 
                        sprite.material.SetFloat(flashMixId, mix = v);
                    },
                    0,
                    flashDuration * 0.9f
                ).SetEase(Ease.InSine)
            );
    }

    void OnDestroy(){
        if (pump != null) {
            Destroy(pump);
            // Debug.Log("Pump nuked " + pump.GetInstanceID());
            pump = null;
        }
    }

    [System.Serializable]
    public class PowerOffset
    {
        public int x;
        public int y;
    }
}
