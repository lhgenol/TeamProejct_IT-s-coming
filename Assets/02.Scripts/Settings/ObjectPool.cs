using System.Collections.Generic;
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
        /*if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복된 오브젝트 제거
        }*/
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
            // 큐에서 오브젝트 가져오기
            obj = poolDictionary[prefab.name].Dequeue().gameObject;

            // 가져온 오브젝트가 null인지 확인 (삭제되었을 가능성 체크)
            if (obj == null)
            {
                Debug.LogWarning($"[ObjectPool] {prefab.name} 풀에서 가져온 오브젝트가 이미 삭제됨. 새로 생성합니다.");
                obj = Instantiate(prefab);
                obj.name = prefab.name;
            }
        }
        else
        {
            // 풀에 남은 오브젝트가 없으면 새로 생성
            obj = Instantiate(prefab);
            obj.name = prefab.name;
        }

        if (newParent != null) obj.transform.SetParent(newParent); 
        obj.transform.position = spawnPosition.position;
        obj.gameObject.SetActive(true);

        return obj;
    }

    /*public GameObject GetFromPool(GameObject prefab, Transform spawnPosition, Transform newParent = null)
    {
        if (prefab == null)
        {
            Debug.LogError("[ObjectPool] GetFromPool() - prefab이 null입니다!");
            return null;
        }

        GameObject obj;

        if (poolDictionary.ContainsKey(prefab.name) && poolDictionary[prefab.name].Count > 0)
        {
            obj = poolDictionary[prefab.name].Dequeue().gameObject;

            // 🚨 원본 프리팹을 참조하는 경우 방지
            if (PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                Debug.LogError($"[ObjectPool] {obj.name}은 원본 프리팹입니다! 새로 인스턴스화합니다.");
                obj = Instantiate(prefab);
            }
        }
        else
        {
            obj = Instantiate(prefab); // ✅ 새 인스턴스 생성
            obj.name = prefab.name + " (clone)";
        }

        if (newParent != null)
            obj.transform.SetParent(newParent, false); // ✅ 부모 설정 (로컬 좌표 유지)

        obj.transform.position = spawnPosition.position;
        obj.SetActive(true);

        return obj;
    }*/

    public void ReturnToPool(T obj, GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab.name))
        {
            Debug.LogWarning($"ReturnToPool: {prefab.name} is not exist");
            if(prefab != null) Destroy(prefab);
            return;
        }

        obj.transform.SetParent(this.transform); // 다시 풀의 부모로 설정
        obj.transform.localPosition = Vector3.zero;
        obj.gameObject.SetActive(false);
        poolDictionary[prefab.name].Enqueue(obj);
    }
}
