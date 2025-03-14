using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<Item>
{
    private void Awake()
    {

        MapManager.Instance.itemPool = this;
    }
}
