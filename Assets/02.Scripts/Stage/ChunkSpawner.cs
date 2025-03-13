using System.Collections;
using System.Collections.Generic;
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

    [Header("StartingTemplates")]
    public ChunkTemplate[] startingTemaplates;

    private ThemeDataSO curTheme;
    private int curThemeIndex = 0;
    ChunkTemplate curTemplate;
    private int curTemplateIndex;


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
                startingPosition.position += Vector3.back * 20f;
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
