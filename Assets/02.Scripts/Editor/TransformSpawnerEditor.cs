using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TransformSpawner))]
public class TransformSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TransformSpawner spawner = (TransformSpawner)target;

        if (GUILayout.Button("Spawn Objects"))
        {
            spawner.SpawnObjects();
        }
    }
}