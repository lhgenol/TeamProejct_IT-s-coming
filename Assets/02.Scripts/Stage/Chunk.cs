using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnData
{
    public Transform spawnPosition;  // 배치 위치
    [SerializeField] protected GameObject prefab; // Inspector에서 드롭다운으로 선택

    public GameObject Prefab => prefab;
}

[System.Serializable]
public class ObstacleSpawnData : SpawnData
{
}

[System.Serializable]
public class StuctureSpawnData : SpawnData
{
}


[System.Serializable]
public class ItemSpawnData : SpawnData
{
}




public class Chunk : MonoBehaviour
{
    public ThemeDataSO themeData;

    [Header("PlacePosition")]
    [SerializeField] private ObstacleSpawnData[] obstaclePosition;
    [SerializeField] private StuctureSpawnData[] structurePosition;
    [SerializeField] private ItemSpawnData[] itemPosition;
    [SerializeField] private Transform[] coinPosition;

    public void SpawnChunk()
    {

    }

    private void PlaceObject()
    {
        PlaceObstacle();
        //PlaceStructure();
        //PlaceItem();
        //PlaceCoin();
    }

    void PlaceObstacle()
    {
        foreach (var obs in obstaclePosition)
        {
            MapManager.Instance.obstaclePool.GetFromPool(obs.Prefab.GetComponent<Obstacle>(), obs.spawnPosition, this.transform);
        }
    }

    /*void PlaceStructure()
    {
        foreach (var obs in structurePosition)
        {
            MapManager.Instance.structurePool.GetFromPool(obs.Prefab.GetComponent<Structure>(), obs.spawnPosition, this.transform);
        }
    }*/

    /*void PlaceItem()
    {
        foreach (var item in itemPosition)
        {
            MapManager.Instance.itemPool.GetFromPool(item.Prefab.GetComponent<Item>(), item.spawnPosition, this.transform);
        }
    }*/

    void PlaceCoin()
    {
        foreach(var coin in coinPosition)
        {
            MapManager.Instance.itemPool.GetFromPool(itemPosition[0].Prefab.GetComponent<Item>(), coin, this.transform);// coin<Itme> 넣어줘야함
        }
    }

    public List<GameObject> GetPrefabList(int index)
    {
        if (themeData == null) return null;

        switch(index)
        {
            case 0:
                return themeData.obstacleList != null ? themeData.obstacleList : new List<GameObject>();
            case 1:
                return themeData.structureList != null ? themeData.structureList : new List<GameObject>();
            case 2:
                return themeData.itemList != null ? themeData.itemList : new List<GameObject>();
            default:
                return null;
        }
    }

    public List<GameObject> GetObstaclePrefabList()
    {
        return themeData != null ? themeData.obstacleList : null;
    }

    public List<GameObject> GetStructurePrefabList()
    {
        return themeData != null ? themeData.structureList : null; 
    }
}
