using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public ChunkPool chunkPool;
    public ObstaclePool obstaclePool;
    public ItemPool itemPool;// itemPool;

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

        if (itemPool == null)
        {
            itemPool = FindObjectOfType<ItemPool>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
