using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : ObjectPool<Chunk>
{
    private void Awake()
    {
        MapManager.Instance.chunkPool = this;


        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabsList = new List<GameObject>();

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.chunkList != null) // null 체크
                {
                    prefabsList.AddRange(theme.chunkList);
                }
            }

            prefabs = prefabsList.ToArray();
        }
    }
    protected override void Start()
    {
        base.Start();
    }
}
