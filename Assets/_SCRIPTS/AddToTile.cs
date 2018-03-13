using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class AddToTile : MonoBehaviour {
	
#if UNITY_EDITOR
	Transform prevParent = null;
	bool justAdded = true;
	void Start () {
		// we want to know when hierarchy changes, ie when prefab is dropped in the scene
		EditorApplication.hierarchyWindowChanged += HierarchyChanged;
	}
	
    void HierarchyChanged()
    {
		if (EditorApplication.isPlaying) return; // otherwise gets called ALWAYS in the editor
		// called when we are destroyed as well
		// unity...
		if (this == null) {
			EditorApplication.hierarchyWindowChanged -= HierarchyChanged;
			return;
		}
		if (transform.parent == null) {
			prevParent = transform.parent;
			// stuff is added to root when we start dragging
			if (justAdded) {
				justAdded = false;
			} else {
				AttachToTileOrDestroy();
			}
		} else if (transform.parent == prevParent) {
			// we didnt change
		} else {
			// parent changed
			prevParent = transform.parent;
			AttachToTileOrDestroy();
		}
		
    }

	void AttachToTileOrDestroy() 
	{
		Debug.Log("Attach? " + transform.parent);
		Tileset tileSet = FindObjectOfType<Tileset>();
		if (tileSet == null) {
			Debug.LogError("No tileset!");
			return;
		}
		Debug.Log("Got tileset!");
		Tile at = tileSet.GetTileAt(transform.position);
		if (at == null) {
			Debug.LogError("No tile at drop location!");
			DestroyImmediate(gameObject);
			return;
		}
		if (!at.gameObject.activeInHierarchy) {
			Debug.LogError(at + " not active !");
			DestroyImmediate(gameObject);
			return;
		}
		// add it to the tile
		transform.parent = at.transform;
		transform.position = at.transform.position;
	}
#endif
}
