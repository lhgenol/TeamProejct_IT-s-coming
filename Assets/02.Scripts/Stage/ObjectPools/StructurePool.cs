using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePool : ObjectPool<Structure>
{
    private void Awake()
    {
        MapManager.Instance.structurePool = this;
    }

    protected override void Start()
    {
        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabs = new List<GameObject>();

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.structureList != null) // null 체크
                {
                    prefabs.AddRange(theme.structureList);
                }
            }

            GameObject[] prefabArray = prefabs.ToArray();
        }

        base.Start();
    }
}
