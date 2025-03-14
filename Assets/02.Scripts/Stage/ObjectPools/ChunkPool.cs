using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : ObjectPool<Chunk>
{
    protected override void Start()
    {
        base.Start();
        MapManager.Instance.chunkPool = this;
    }
}
