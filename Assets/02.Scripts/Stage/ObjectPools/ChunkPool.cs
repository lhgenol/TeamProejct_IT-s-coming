using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : ObjectPool<Chunk>
{
    private void Awake()
    {
        MapManager.Instance.chunkPool = this;
    }
    protected override void Start()
    {
        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabs = new List<GameObject>();

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.chunkList != null) // null 체크
                {
                    prefabs.AddRange(theme.chunkList);
                }
            }

            GameObject[] prefabArray = prefabs.ToArray();
        }

        base.Start();
    }
}
