using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public ChunkPool chunkPool;
    public ObstaclePool obstaclePool;
    // itemPool;

    void Start()
    {
        if (chunkPool == null)
        {
            chunkPool = FindObjectOfType<ChunkPool>();
        }

        if (obstaclePool == null)
        {
            obstaclePool = FindObjectOfType<ObstaclePool>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
