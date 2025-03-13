using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    public Dictionary<string, Queue<T>> poolDictionary = new Dictionary<string, Queue<T>>();
    public GameObject[] prefabs;
    public int initialSize = 3;

    protected virtual void Start()
    {
        InitObjectPool();
    }

    protected void InitObjectPool()
    {

        foreach (GameObject prefab in prefabs)
        {
            if (!poolDictionary.ContainsKey(prefab.name))
            {
                poolDictionary[prefab.name] = new Queue<T>();
            }

            for (int i = 0; i < initialSize; i++)
            {
                T obj = Instantiate(prefab).GetComponent<T>();
                obj.name = prefab.name;
                obj.transform.SetParent(this.transform);
                obj.gameObject.SetActive(false);
                poolDictionary[prefab.name].Enqueue(obj);
            }
        }
    }

    public T GetFromPool(GameObject prefab, Transform spawnPosition, Transform newParent = null)
    {
        T obj;
        if (poolDictionary.ContainsKey(prefab.name) && poolDictionary[prefab.name].Count > 0)
        {
            obj = poolDictionary[prefab.name].Dequeue();
        }
        else
        {
            // 풀에 남은 오브젝트가 없으면 새로 생성
            obj = Instantiate(prefab).GetComponent<T>();
            obj.name = prefab.name;
        }

        if(newParent != null) obj.transform.SetParent(newParent); 
        obj.transform.position = spawnPosition.position;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj, GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab.name))
        {
            Debug.LogWarning($"ReturnToPool: {prefab.name} is not exist");
            Destroy(obj.gameObject);
            return;
        }

        obj.transform.SetParent(this.transform); // 다시 풀의 부모로 설정
        obj.transform.localPosition = Vector3.zero;
        obj.gameObject.SetActive(false);
        poolDictionary[prefab.name].Enqueue(obj);
    }
}
