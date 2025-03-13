using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeData", menuName = "ThemeData/themeData")]
public class ThemeDataSO : ScriptableObject
{
    public int index;
    public string ThemeName;
    public List<GameObject> itemList;
    public List<GameObject> obstacleList; // 사용할 프리팹 리스트
    public List<GameObject> structureList;
    public List<GameObject> chunkList;
}
