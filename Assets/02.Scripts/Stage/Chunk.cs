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
    [Header("Theme")]
    public ThemeDataSO themeData;

    [Header("PlacePosition")]
    [SerializeField] private ObstacleSpawnData[] obstaclePosition;
    [SerializeField] private StuctureSpawnData[] structurePosition;
    [SerializeField] private ItemSpawnData[] itemPosition;
    [SerializeField] private Transform[] coinPosition;

    private void OnEnable()
    {
        PlaceObjects();
    }

    private void OnDisable()
    {
        CollectObjects();
    }

    public void PlaceObjects()
    {
        PlaceObstacle();
        PlaceStructure();
        PlaceItem();
        PlaceCoin();
    }

    void PlaceObstacle()
    {
        foreach (var obj in obstaclePosition)
        {
            MapManager.Instance.obstaclePool.GetFromPool(obj.Prefab, obj.spawnPosition, this.transform);
        }
    }

    void PlaceStructure()
    {
        foreach (var obj in structurePosition)
        {
            MapManager.Instance.structurePool.GetFromPool(obj.Prefab, obj.spawnPosition, this.transform);
        }
    }

    void PlaceItem()
    {
        foreach (var obj in itemPosition)
        {
            MapManager.Instance.itemPool.GetFromPool(obj.Prefab, obj.spawnPosition, this.transform);
        }
    }

    void PlaceCoin()
    {
        foreach(var obj in coinPosition)
        {
            MapManager.Instance.itemPool.GetFromPool(itemPosition[0].Prefab, obj, this.transform);// coin<Itme> [0]에 넣어줘야한다.
        }
    }

    public void CollectObjects()
    {
        CollectObstacle();
        CollectStructure();
        CollectItem();
        CollectCoin();
    }

    void CollectObstacle()
    {
        foreach (var obj in obstaclePosition)
        {
            MapManager.Instance.obstaclePool.ReturnToPool(obj.Prefab.GetComponent<Obstacle>(), obj.Prefab);
        }
    }

    void CollectStructure()
    {
        foreach (var obj in structurePosition)
        {
            MapManager.Instance.structurePool.ReturnToPool(obj.Prefab.GetComponent<Structure>(), obj.Prefab);
        }
    }

    void CollectItem()
    {
        foreach (var obj in itemPosition)
        {
            MapManager.Instance.itemPool.ReturnToPool(obj.Prefab.GetComponent<Item>(), obj.Prefab);
        }
    }

    void CollectCoin()
    {
        List<GameObject> collectedCoins = new List<GameObject>();

        foreach (Transform coinTransform in coinPosition)
        {
            if (coinTransform.childCount > 0) 
            {
                GameObject coinObject = coinTransform.GetChild(0).gameObject; 
                collectedCoins.Add(coinObject);
            }
        }

        foreach (GameObject coin in collectedCoins)
        {
            MapManager.Instance.itemPool.ReturnToPool(coin.GetComponent<Item>(), coin);
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

    /*public List<GameObject> GetObstaclePrefabList()
    {
        return themeData != null ? themeData.obstacleList : null;
    }

    public List<GameObject> GetStructurePrefabList()
    {
        return themeData != null ? themeData.structureList : null; 
    }*/
}
