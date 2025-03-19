using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnData), true)] //  모든 SpawnData 기반 클래스 적용 가능하게 변경
public class SpawnDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 현재 Chunk를 찾기
        Chunk chunk = property.serializedObject.targetObject as Chunk;
        if (chunk == null)
        {
            EditorGUI.LabelField(position, "Chunk not found!");
            return;
        }

        // 현재 필드가 Obstacle인지 Structure인지 확인
        List<GameObject> prefabList = null;
        if (property.type == nameof(ObstacleSpawnData)) //  Obstacle이면 obstacleList 사용
        {
            prefabList = chunk.GetPrefabList(0);
        }
        else if (property.type == nameof(StuctureSpawnData)) //  Structure이면 structureList 사용
        {
            prefabList = chunk.GetPrefabList(1);
        }
        else if (property.type == nameof(ItemSpawnData)) 
        {
            prefabList = chunk.GetPrefabList(2);
        }

        if (prefabList == null || prefabList.Count == 0)
        {
            EditorGUI.LabelField(position, "PrefabList not found in ThemeData!");
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
