using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePool : ObjectPool<Structure>
{
    private void Awake()
    {
        MapManager.Instance.structurePool = this;


        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabsList = new List<GameObject>();

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.structureList != null) // null 체크
                {
                    prefabsList.AddRange(theme.structureList);
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
