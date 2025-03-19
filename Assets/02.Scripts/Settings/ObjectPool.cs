﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    public Dictionary<string, Queue<T>> poolDictionary = new Dictionary<string, Queue<T>>();
    public GameObject[] prefabs;
    public int initialSize = 3;

    private static ObjectPool<T> instance;

    protected virtual void Awake()
    {
    }
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
                GameObject obj = Instantiate(prefab);
                obj.name = prefab.name;
                obj.transform.SetParent(this.transform);
                obj.gameObject.SetActive(false);
                poolDictionary[prefab.name].Enqueue(obj.GetComponent<T>());
            }
        }
    }

    public GameObject GetFromPool(GameObject prefab, Transform spawnPosition, Transform newParent = null)
    {
        GameObject obj;
        if (poolDictionary.ContainsKey(prefab.name) && poolDictionary[prefab.name].Count > 0)
        {
            obj = poolDictionary[prefab.name].Dequeue().gameObject;
        }
        else
        {
            // 풀에 남은 오브젝트가 없으면 새로 생성
            obj = Instantiate(prefab);
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
