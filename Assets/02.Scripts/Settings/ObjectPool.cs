using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    public Dictionary<GameObject, Queue<T>> poolDictionary = new Dictionary<GameObject, Queue<T>>();
    public GameObject[] prefabs;
    public int initialSize = 3;

    protected virtual void Start()
    {
        InitObjectPool();
    }

    public void InitObjectPool()
    {

        foreach (GameObject prefab in prefabs)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                poolDictionary[prefab] = new Queue<T>();
            }

            for (int i = 0; i < initialSize; i++)
            {
                T obj = Instantiate(prefab).GetComponent<T>();
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(this.transform); // 처음에는 풀의 부모 아래에 배치
                poolDictionary[prefab].Enqueue(obj);
            }
        }
    }

    public T GetFromPool(GameObject prefab, Transform spawnPosition, Transform newParent = null)
    {
        T obj;
        if (poolDictionary.ContainsKey(prefab) && poolDictionary[prefab].Count > 0)
        {
            obj = poolDictionary[prefab].Dequeue();
        }
        else
        {
            // 풀에 남은 오브젝트가 없으면 새로 생성
            obj = Instantiate(prefab).GetComponent<T>();
        }

        if(newParent != null) obj.transform.SetParent(newParent); // 원하는 부모로 변경
        obj.transform.position = spawnPosition.position;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj, GameObject prefab)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(this.transform); // 다시 풀의 부모로 설정
        obj.transform.localPosition = Vector3.zero;

        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<T>();
        }
        poolDictionary[prefab].Enqueue(obj);
    }
}
