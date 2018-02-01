using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GroundSpawner))]

public class CustomInspectorOfGroundSpawner : Editor {

	public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Spawn ground"))
        {
			GroundSpawner gs = target as GroundSpawner;
			gs.SpawnGround();
        }

        DrawDefaultInspector();
    }
}
