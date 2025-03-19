using System.Collections;
using System.Collections.Generic;
//using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [Header("Themes")]
    public ThemeDataSO[] themeData;

    [Header("SpawnInfo")]
    public Transform spawnPosition;
    public Transform chunkContainer;
    public Transform startingPosition;
    [SerializeField] private int themeChangeMinThreshold = 3;
    [SerializeField] private int themeChangeMaxThreshold = 5;
    private float chunkLenghth = 60f;

    [Header("StartingTemplates")]
    public ChunkTemplate[] startingTemaplates;

    ThemeDataSO curTheme;
    private int curThemeIndex = 0;
    ChunkTemplate curTemplate;
    private int curTemplateIndex;

    Queue<GameObject> chunkQueue = new Queue<GameObject>();
    GameObject lastChunk;
    private int leftTemplate;

    private void Awake()
    {
        MapManager.Instance.chunkSpawner = this;
    }

    private void Start()
    {
        this.themeData = MapManager.Instance.themeData;
        Init();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chunk"))
        {
            if (chunkQueue.Count <= 0)
            {
                SpawnTemplet();
            }

            SpawnChunk(chunkQueue.Dequeue());
        }
    }

    void SpawnChunk(GameObject chunk)
    {
        lastChunk = MapManager.Instance.chunkPool.GetFromPool(chunk, spawnPosition, chunkContainer);
        spawnPosition = lastChunk.GetComponent<Chunk>().chunkTail;
    }

    void Init()
    {
        if (spawnPosition == null)
        {
            spawnPosition = new GameObject("SpawnPosition").transform;
            spawnPosition.SetParent(this.transform);
        }

        leftTemplate = RandomThemeChangeThreshold();
        curThemeIndex = 0;
        curTheme = themeData[curThemeIndex];
        chunkContainer.position = Vector3.zero;

        if (startingPosition == null)
        {
            startingPosition = new GameObject("StartingPosition").transform;
            startingPosition.SetParent(this.transform);
            startingPosition.position = new Vector3(0, 0, -20);
        }

        spawnPosition.position = startingPosition.position;
        PlaceStartingTemplate();
        SpawnTemplet();
    }


    void PlaceStartingTemplate()
    {
        if (startingTemaplates != null)
        {
            List<int> ints = startingTemaplates[Random.Range(0, startingTemaplates.Length)].chunkIndexCombine;

            foreach(int i in ints)
            {
                GameObject spawningChunk = themeData[0].chunkList[i];
                SpawnChunk(spawningChunk);
            }
        }
    }

    void SpawnTemplet()
    {
        if (leftTemplate <= 0)//남은 템플릿 0이면 테마를 바꾼다.
        {
            if (curTheme.chunkList.Count > 2) chunkQueue.Enqueue(curTheme.chunkList[1]);
            else chunkQueue.Enqueue(curTheme.chunkList[0]);

            curTheme = RandomTheme();
            leftTemplate = RandomThemeChangeThreshold();
            chunkQueue.Enqueue(curTheme.chunkList[0]);
        }

        curTemplate = RandomTemplet();

        foreach (int i in curTemplate.chunkIndexCombine)//랜덤 템플릿을 받아와서 data의 chunklist에서 골라서 인큐
        {
            chunkQueue.Enqueue(themeData[curThemeIndex].chunkList[i]);
        }

        leftTemplate--;
    }



    int RandomThemeChangeThreshold()
    {
        return Random.Range(themeChangeMinThreshold, themeChangeMaxThreshold);
    }

    ChunkTemplate RandomTemplet()
    {
        int newTempIndex;
        int i = 0;
        do
        {
            newTempIndex = RandomTempletIndex();
            i++;
        }
        while (newTempIndex == this.curTemplateIndex && i < 5);

        this.curTemplateIndex = newTempIndex;

        return (themeData[curThemeIndex].templates != null) ? themeData[curThemeIndex].templates[newTempIndex] : null;
    }

    int RandomTempletIndex()
    {
        return (themeData[curThemeIndex].templates != null) ? Random.Range(0, themeData[curThemeIndex].templates.Count) : 0;
    }

    ThemeDataSO RandomTheme()
    {
        int newThemeIndex;
        int i = 0;
        do
        {
            newThemeIndex = RandomThemeIndex();
            i++;
        }
        while (newThemeIndex == this.curThemeIndex && i < 5);

        this.curThemeIndex = newThemeIndex;

        return (newThemeIndex != -1) ? themeData[newThemeIndex] : null;
    }

    int RandomThemeIndex()
    {
        return (themeData != null) ? Random.Range(0, themeData.Length) : -1;
    }

    public void Reset()
    {
        CollectAllChunks();
        Init();
    }

    void CollectAllChunks()
    {
        Chunk[] allChunks = chunkContainer.GetComponentsInChildren<Chunk>();
        List<GameObject> chunkObjects = new List<GameObject>();

        foreach (Chunk chunk in allChunks)
        {
            chunkObjects.Add(chunk.gameObject);
        }

        foreach (GameObject chunkObj in chunkObjects)
        {
            MapManager.Instance.chunkPool.ReturnToPool(chunkObj.GetComponent<Chunk>(), chunkObj);
        }
    }
}
