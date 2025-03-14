using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : ObjectPool<Chunk>
{
    private void Awake()
    {
        MapManager.Instance.chunkPool = this;
    }

}
