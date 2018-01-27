//c# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HouseSpot))]

public class CustomInspecotrOfHouseScript : Editor
{
    
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Build Object"))
        {
            ((HouseSpot)target).SpawnTier(((HouseSpot)target).asdas);
        }

        DrawDefaultInspector();
    }
}
