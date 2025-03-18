using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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
            if (curTheme.chunkList.Count > 2) chunkQueue.Enqueue(curTheme.chunkList[1]);//바꾸기 전에 테마 종료 청크 인큐
            else chunkQueue.Enqueue(curTheme.chunkList[0]);

            curTheme = RandomTheme();//본인제외 랜덤 테마
            leftTemplate = RandomThemeChangeThreshold();
            chunkQueue.Enqueue(curTheme.chunkList[0]);//바꾼후 테마 시작 청크 인큐
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
        while (newTempIndex == this.curTemplateIndex && i < 5);//지금 템플릿과 같다면 다시돌림 최대 30번

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
        //  chunkContainer 내부의 모든 Chunk 컴포넌트를 가져오기
        Chunk[] allChunks = chunkContainer.GetComponentsInChildren<Chunk>();

        //  Chunk가 붙어 있는 GameObject 리스트 생성
        List<GameObject> chunkObjects = new List<GameObject>();

        foreach (Chunk chunk in allChunks)
        {
            chunkObjects.Add(chunk.gameObject);
        }

        //  반환 로직 실행
        foreach (GameObject chunkObj in chunkObjects)
        {
            MapManager.Instance.chunkPool.ReturnToPool(chunkObj.GetComponent<Chunk>(), chunkObj);
        }
    }
}

/*
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

    private ThemeDataSO curTheme;
    private int curThemeIndex = 0;
    ChunkTemplate curTemplate;
    private int curTemplateIndex;
    Queue<GameObject> chunkQueue = new Queue<GameObject>();

    private int leftTemplate;
    private int i = 0;

    private void Start()
    {
        MapManager.Instance.chunkSpawner = this;
        Init();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chunk"))
        {
            if (i >= curTemplate.chunkIndexCombine.Count)
            {
                SpawnTemplet();
                i = 0;
            }

            SpawnChunk(curTemplate, i);
            i++;
        }
    }

    public void Reset()
    {
        CollectAllChunks();
        Init();
    }

    void CollectAllChunks()
    {
        Chunk[] allChunksOnContainer = chunkContainer.gameObject.GetComponentsInChildren<Chunk>();

        foreach (Chunk chunk in allChunksOnContainer)
        {
            MapManager.Instance.chunkPool.ReturnToPool(chunk, chunk.gameObject);
        }
    }

    void Init()
    {
        if (spawnPosition == null) spawnPosition = this.transform;

        leftTemplate = RandomThemeChangeThreshold();
        curThemeIndex = 0;
        i = 0;
        curTheme = themeData[curThemeIndex];
        chunkContainer.position = Vector3.zero;

        if (startingPosition == null)
        {
            GameObject tempObj = new GameObject("StartingPosition");
            tempObj.transform.position = new Vector3(0, 0, -20);
            startingPosition = tempObj.transform;
        }

        PlaceStartingTemplate();
    }


    void PlaceStartingTemplate()
    {
        if (startingTemaplates == null)
        {
            List<int> ints = startingTemaplates[Random.Range(0, startingTemaplates.Length)].chunkIndexCombine;
            Vector3 origin = startingPosition.position;

            for (int i = 0; i < ints.Count; i++)
            {
                MapManager.Instance.chunkPool.GetFromPool(themeData[0].chunkList[i], startingPosition, chunkContainer);
                startingPosition.position += Vector3.back * chunkLenghth;
            }

            startingPosition.position = origin;
        }
    }

    int RandomThemeChangeThreshold()
    {
        return Random.Range(themeChangeMinThreshold, themeChangeMaxThreshold);
    }

    ChunkTemplate RandomTemplet(int curTempletIndex)
    {
        int j = 0;
        do
        {
            curTempletIndex = RandomTempletIndex();
            j++;
        }
        while (curTempletIndex != this.curTemplateIndex || j > 30);//지금 템플릿과 같다면 다시돌림 최대 30번

        this.curTemplateIndex = curTempletIndex;

        return themeData[curThemeIndex].Templates[curTempletIndex];
    }

    int RandomTempletIndex()
    {
        return Random.Range(0, themeData[curThemeIndex].Templates.Count);
    }

    void SpawnTemplet()
    {
        if (leftTemplate > 0)
        {
            curTemplate = RandomTemplet(curTemplateIndex);
            leftTemplate--;
        }
        else
        {
            curTheme = RandomTheme(curThemeIndex);
            leftTemplate = RandomThemeChangeThreshold();
        }
    }

    ThemeDataSO RandomTheme(int curThemeIndex)
    {
        int j = 0;
        do
        {
            curThemeIndex = RandomThemeIndex();
            j++;
        }
        while (curThemeIndex != this.curThemeIndex || j > 30);

        this.curThemeIndex = curThemeIndex;

        return themeData[curThemeIndex];
    }

    int RandomThemeIndex()
    {
        return Random.Range(0, themeData.Length);
    }

    void SpawnChunk(ChunkTemplate curTemplet, int index)
    {
        MapManager.Instance.chunkPool.GetFromPool(themeData[curThemeIndex].chunkList[curTemplet.chunkIndexCombine[index]], spawnPosition, chunkContainer);
    }
}
*/