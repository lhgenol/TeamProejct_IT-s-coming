using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ObstacleSpawnData))] 
public class ObjectSpawnDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Chunk chunk = property.serializedObject.targetObject as Chunk;
        if (chunk == null)
        {
            EditorGUI.LabelField(position, "Chunk not found!");
            return;
        }

        // ThemeDataSO에서 PrefabListSO 가져오기
        List<GameObject> prefabList = chunk.GetPrefabList();
        if (prefabList == null)
        {
            EditorGUI.LabelField(position, "PrefabListSO not found in ThemeData!");
            return;
        }

        // ObjectSpawnData 내부 필드 찾기
        SerializedProperty spawnPositionProp = property.FindPropertyRelative("spawnPosition");
        SerializedProperty prefabProp = property.FindPropertyRelative("prefab");

        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        Rect posRect = new Rect(position.x, position.y, position.width, lineHeight);
        Rect dropdownRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);

        // Spawn Position 필드
        EditorGUI.PropertyField(posRect, spawnPositionProp, new GUIContent("Spawn Position"));

        // Prefab Dropdown 만들기
        List<string> options = new List<string>();
        int selectedIndex = 0;

        for (int i = 0; i < prefabList.Count; i++)
        {
            options.Add(prefabList[i].name);
            if (prefabList[i] == prefabProp.objectReferenceValue)
                selectedIndex = i;
        }

        selectedIndex = EditorGUI.Popup(dropdownRect, "Prefab", selectedIndex, options.ToArray());
        prefabProp.objectReferenceValue = prefabList[selectedIndex];

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2;
    }
}
