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
    [SerializeField] private int themeChangeMinThreshold = 3;
    [SerializeField] private int themeChangeMaxThreshold = 5;

    private ThemeDataSO curTheme;
    private int curThemeIndex = 0;
    ChunkTemplate curTemplet;
    private int curTempletIndex;


    private int leftTemplet;
    private int i = 0;

    private void Start()
    {
        if (spawnPosition == null) spawnPosition = this.transform;
        leftTemplet = RandomThemeChangeThreshold();
        curTheme = themeData[curThemeIndex];
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chunk"))
        {
            if (i >= curTemplet.chunkIndexCombine.Count)
            {
                SpawnTemplet();
                i = 0;
            }

            SpawnChunk(curTemplet, i);
            i++;
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
        while (curTempletIndex != this.curTempletIndex || j > 30);//지금 템플릿과 같다면 다시돌림 최대 30번

        this.curTempletIndex = curTempletIndex;

        return themeData[curThemeIndex].Templates[curTempletIndex];
    }

    int RandomTempletIndex()
    {
        return Random.Range(0, themeData[curThemeIndex].Templates.Count);
    }

    void SpawnTemplet()
    {
        if (leftTemplet > 0)
        {
            curTemplet = RandomTemplet(curTempletIndex);
            leftTemplet--;
        }
        else
        {
            curTheme = RandomTheme(curThemeIndex);
            leftTemplet = RandomThemeChangeThreshold();
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
