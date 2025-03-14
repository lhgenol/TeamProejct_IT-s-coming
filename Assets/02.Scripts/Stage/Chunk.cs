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
        StartCoroutine(DelayedPlace());
    }

    IEnumerator DelayedPlace()
    {
        yield return null;
        PlaceObjects();
    }

    private void OnDisable()
    {
        if (MapManager.Instance == null || !Application.isPlaying)
        {
            Debug.LogWarning("[Chunk] OnDisable() - MapManager가 null이므로 CollectObjects()를 실행하지 않음");
            return;
        }

        CollectObjects();
    }

    IEnumerator DelayedCollect()
    {
        yield return null;
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
            MapManager.Instance.obstaclePool.GetFromPool(obj.Prefab, obj.spawnPosition, obj.spawnPosition);
        }
    }

    void PlaceStructure()
    {
        foreach (var obj in structurePosition)
        {
            MapManager.Instance.structurePool.GetFromPool(obj.Prefab, obj.spawnPosition, obj.spawnPosition);
        }
    }

    void PlaceItem()
    {
        foreach (var obj in itemPosition)
        {
            MapManager.Instance.itemPool.GetFromPool(obj.Prefab, obj.spawnPosition, obj.spawnPosition);
        }
    }

    void PlaceCoin()
    {
        foreach (var obj in coinPosition)
        {
            MapManager.Instance.itemPool.GetFromPool(themeData.itemList[0], obj, obj);// coin<Itme> [0]에 넣어줘야한다.
        }
    }

    public void CollectObjects()
    {
        if (MapManager.Instance == null)
        {
            Debug.LogWarning("[Chunk] CollectObjects() - MapManager가 null이므로 실행하지 않음");
            return;
        }

        CollectObstacle();
        CollectStructure();
        CollectItem();
        CollectCoin();
    }

    void CollectObstacle()
    {
        if (obstaclePosition == null) return;

        foreach (var obj in obstaclePosition)
        {
            if (obj == null) // null 체크
            {
                Debug.LogWarning($"obstaclePosition is null");
                continue;
            }

            if (obj.spawnPosition == null)
            {
                Debug.LogWarning($"{obj.Prefab.name} is null");
                continue;
            }

            if (obj.spawnPosition.childCount > 1)
            {
                DestroyAllChildren(obj.spawnPosition);
            }
            else if (obj.spawnPosition.childCount > 0) //스폰된 오브젝트가 있는지 확인
            {
                GameObject spawnedObstacle = obj.spawnPosition.GetChild(0).gameObject;
                MapManager.Instance.obstaclePool.ReturnToPool(spawnedObstacle.GetComponent<Obstacle>(), spawnedObstacle);
            }
        }
    }


    void CollectStructure()
    {
        if (structurePosition == null) return;

        foreach (var obj in structurePosition)
        {
            if (obj == null) // null 체크
            {
                Debug.LogWarning($"obstaclePosition is null");
                continue;
            }

            if (obj.spawnPosition == null)
            {
                Debug.LogWarning($"{obj.Prefab.name} is null");
                continue;
            }

            if (obj.spawnPosition.childCount > 1)
            {
                DestroyAllChildren(obj.spawnPosition);
            }
            else if (obj.spawnPosition.childCount > 0) // 🚨 실제 스폰된 오브젝트가 있는지 확인
            {
                GameObject spawnedStructure = obj.spawnPosition.GetChild(0).gameObject;
                MapManager.Instance.structurePool.ReturnToPool(spawnedStructure.GetComponent<Structure>(), spawnedStructure);
            }
        }
    }

    void CollectItem()
    {
        if (itemPosition == null) return;

        foreach (var obj in itemPosition)
        {
            if (obj == null) // null 체크
            {
                Debug.LogWarning($"itemPosition is null");
                continue;
            }

            if (obj.spawnPosition == null)
            {
                Debug.LogWarning($"{obj.Prefab.name} is null");
                continue;
            }

            if (obj.spawnPosition.childCount > 1)
            {
                DestroyAllChildren(obj.spawnPosition);
            }
            else if (obj.spawnPosition.childCount > 0) //  실제 스폰된 오브젝트가 있는지 확인
            {
                GameObject spawnedItem = obj.spawnPosition.GetChild(0).gameObject;
                MapManager.Instance.itemPool.ReturnToPool(spawnedItem.GetComponent<Item>(), spawnedItem);
            }
        }
    }

    void CollectCoin()
    {
        if (coinPosition == null) return;

        foreach (Transform coinTransform in coinPosition)
        {
            if (coinTransform == null)
            {
                Debug.LogWarning("[CollectCoin] coinPosition has null");
                continue;
            }

            if (coinTransform.childCount > 1)
            {
                DestroyAllChildren(coinTransform);
            }
            else if (coinTransform.childCount > 0)
            {
                GameObject coinObject = coinTransform.GetChild(0).gameObject;
                MapManager.Instance.itemPool.ReturnToPool(coinObject.GetComponent<Item>(), coinObject);
            }
        }
    }

    void DestroyAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public List<GameObject> GetPrefabList(int index)
    {
        if (themeData == null) return null;

        switch (index)
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
