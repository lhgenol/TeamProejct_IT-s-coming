using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public ChunkPool chunkPool;
    public ObstaclePool obstaclePool;
    public ItemPool itemPool;
    //public StructurePool structurePool;

    public ChunkContainer chunkContainer;
    public ChunkSpawner chunkSpawner;
}
