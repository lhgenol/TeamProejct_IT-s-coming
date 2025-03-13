using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleSpawnData
{
    public Transform spawnPosition;  // 배치 위치
    [SerializeField] private GameObject prefab; // Inspector에서 드롭다운으로 선택

    public GameObject Prefab => prefab;
}

[System.Serializable]
public class ItemSpawnData
{
    public Transform spawnPosition;  
    [SerializeField] private GameObject prefab; 

    public GameObject Prefab => prefab;
}

public class Chunk : MonoBehaviour
{
    public ThemeDataSO themeData;

    [Header("PlacePosition")]
    [SerializeField] private ObstacleSpawnData[] obstaclePosition;
    [SerializeField] private ItemSpawnData[] itemPosition;
    [SerializeField] private Transform[] coinPosition;

    public void SpawnChunk()
    {

    }

    private void PlaceObject()
    {
        foreach(var obs in obstaclePosition)
        {
            MapManager.Instance.obstaclePool.GetFromPool(obs.Prefab, obs.spawnPosition);
        }
    }

    void PlaceObstacle()
    {

    }



    public List<GameObject> GetPrefabList()
    {
        return themeData != null ? themeData.obstacleList : null;
    }
}
