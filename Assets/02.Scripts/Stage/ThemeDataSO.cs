using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkTemplate
{
    public List<int> chunkIndexCombine; //  Inspector에서 쉽게 조합할 수 있도록 List<int> 사용
}

[CreateAssetMenu(fileName = "ThemeData", menuName = "ThemeData/themeData")]
public class ThemeDataSO : ScriptableObject
{
    public int index;
    public string themeName;
    public List<GameObject> itemList;
    public List<GameObject> obstacleList; // 사용할 프리팹 리스트
    public List<GameObject> structureList;
    public List<GameObject> chunkList;
    public List<ChunkTemplate> templates;
    public AudioClip BGM;
    public Material background;
}
