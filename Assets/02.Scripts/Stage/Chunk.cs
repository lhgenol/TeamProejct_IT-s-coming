using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
    public Transform chunkTail;

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
            if (obj != null && obj.spawnPosition != null) MapManager.Instance.obstaclePool.GetFromPool(obj.Prefab, obj.spawnPosition, obj.spawnPosition);
        }
    }

    void PlaceStructure()
    {
        foreach (var obj in structurePosition)
        {
            if (obj != null && obj.spawnPosition != null) MapManager.Instance.structurePool.GetFromPool(obj.Prefab, obj.spawnPosition, obj.spawnPosition);
        }
    }

    void PlaceItem()
    {
        foreach (var obj in itemPosition)
        {
            if (obj != null && obj.spawnPosition != null) MapManager.Instance.itemPool.GetFromPool(obj.Prefab, obj.spawnPosition, obj.spawnPosition);
        }
    }

    void PlaceCoin()
    {
        foreach (var obj in coinPosition)
        {
            if (obj != null) MapManager.Instance.itemPool.GetFromPool(themeData.itemList[0], obj, obj);// coin<Itme> [0]에 넣어줘야한다.
        }
    }

    public void CollectObjects()
    {
        if (MapManager.Instance == null)
        {
            return;
        }

        CollectObjects<Obstacle>(this.transform, MapManager.Instance.obstaclePool);
        CollectObjects<Structure>(this.transform, MapManager.Instance.structurePool);
        CollectObjects<Item>(this.transform, MapManager.Instance.itemPool);
        CollectCoins();
    }

    void CollectObjects<T>(Transform parentTransform, ObjectPool<T> pool) where T : MonoBehaviour
    {
        if (parentTransform == null) return;

        T[] objects = parentTransform.GetComponentsInChildren<T>(true); // 모든 자식 T 객체 가져오기

        foreach (T obj in objects)
        {
            if (obj == null) continue;
            pool.ReturnToPool(obj,obj.gameObject); // 풀로 반환
        }
    }

    void CollectCoins()
    {
        if (coinPosition == null) return;

        foreach (Transform coinTransform in coinPosition)
        {
            if (coinTransform.childCount > 0)
            {
                Item spawnedCoin = coinTransform.GetComponentInChildren<Item>();
                if (spawnedCoin != null)
                {
                    MapManager.Instance.itemPool.ReturnToPool(spawnedCoin,spawnedCoin.gameObject);
                }
            }
        }
    }


    void CollectObstacle()
    {
        if (obstaclePosition == null) return;

        foreach (var obj in obstaclePosition)
        {
            if (obj == null)
            {
                continue;
            }

            if (obj.spawnPosition == null)
            {
                continue;
            }

            if (obj.spawnPosition.childCount > 1)
            {
                DestroyAllChildren(obj.spawnPosition);
            }
            else if (obj.spawnPosition.childCount > 0)
            {
                Obstacle spawnedObstacle = obj.spawnPosition.GetComponentInChildren<Obstacle>();

                if (spawnedObstacle != null)
                {
                    GameObject gameObject = spawnedObstacle.gameObject;
                    MapManager.Instance.obstaclePool.ReturnToPool(gameObject.GetComponent<Obstacle>(), gameObject);
                }
                else DestroyAllChildren(obj.spawnPosition);
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
                continue;
            }

            if (obj.spawnPosition == null)
            {
                continue;
            }

            if (obj.spawnPosition.childCount > 1)
            {
                DestroyAllChildren(obj.spawnPosition);
            }
            else if (obj.spawnPosition.childCount > 0)
            {
                Structure spawnedStructure = obj.spawnPosition.GetComponentInChildren<Structure>();

                if (spawnedStructure != null)
                {
                    GameObject gameObject = spawnedStructure.gameObject;
                    MapManager.Instance.structurePool.ReturnToPool(gameObject.GetComponent<Structure>(), gameObject);
                }
                else DestroyAllChildren(obj.spawnPosition);
            }
        }
    }

    void CollectItem()
    {
        if (itemPosition == null) return;

        foreach (var obj in itemPosition)
        {
            if (obj == null)
            {
                continue;
            }

            if (obj.spawnPosition == null)
            {
                continue;
            }

            if (obj.spawnPosition.childCount > 1)
            {
                DestroyAllChildren(obj.spawnPosition);
            }
            else if (obj.spawnPosition.childCount > 0)
            {
                Item spawnedItem = obj.spawnPosition.GetComponentInChildren<Item>();

                if (spawnedItem != null)
                {
                    GameObject gameObject = spawnedItem.gameObject;
                    MapManager.Instance.itemPool.ReturnToPool(gameObject.GetComponent<Item>(), gameObject);
                }
                else DestroyAllChildren(obj.spawnPosition);
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
                continue;
            }

            if (coinTransform.childCount > 1)
            {
                DestroyAllChildren(coinTransform);
            }
            else if (coinTransform.childCount > 0)
            {
                GameObject coinObject = coinTransform.GetComponentInChildren<Item>().gameObject;
                if (coinObject != null) MapManager.Instance.itemPool.ReturnToPool(coinObject.GetComponent<Item>(), coinObject);
                else DestroyAllChildren(coinTransform);
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
}
