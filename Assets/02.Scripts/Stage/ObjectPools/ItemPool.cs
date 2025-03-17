using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<Item>
{
    protected override void Awake()
    {
        base.Awake();
        MapManager.Instance.itemPool = this;

        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabsList = new List<GameObject>();
            HashSet<string> prefabNames = new HashSet<string>(); //  중복 방지 HashSet

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.itemList != null) 
                {
                    foreach (var item in theme.itemList)
                    {
                        if (item != null && !prefabNames.Contains(item.name)) // 이미 추가된 이름인지 확인
                        {
                            prefabsList.Add(item);
                            prefabNames.Add(item.name); // 
                        }
                    }
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
