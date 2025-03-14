using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<Item>
{
    private void Awake()
    {
        MapManager.Instance.itemPool = this;
    }

    protected override void Start()
    {
        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabs = new List<GameObject>();

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.itemList != null) // null 체크
                {
                    prefabs.AddRange(theme.itemList);
                }
            }

            GameObject[] prefabArray = prefabs.ToArray();
        }

        base.Start();
    }
}
