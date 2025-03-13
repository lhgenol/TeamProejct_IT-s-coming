using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkSpawner : MonoBehaviour
{
    [Header("Themes")]
    public ThemeDataSO[] themeData;

    [Header("Chunk Template Settings")]
    public List<ChunkTemplate> cityChunkTemplates;
    public List<ChunkTemplate> westernChunkTemplates;
    public List<ChunkTemplate> toyChunkTemplates;

    [Header("SpawnInfo")]
    public Transform spawnPosition;
    [SerializeField] private int curThemeIndex = 0;
    [SerializeField] private int themeChangeMinThreshold = 3;
    [SerializeField] private int themeChangeMaxThreshold = 5;
    private int curLastTemplet;

    private void Start()
    {
        if (spawnPosition == null) spawnPosition = this.transform;
        curLastTemplet = RandomThemeChangeThreshold();
    }

    int RandomThemeChangeThreshold()
    {
        return  Random.Range(themeChangeMinThreshold, themeChangeMaxThreshold);
    }

    List<ChunkTemplate> RandomTemplet(int themeIndex)
    {
        return null;
    }


    void SpawnTemplet()
    {
        if (curLastTemplet > 0)
        {

        }
    }


    void SpawnChunk()
    {

    }

}
