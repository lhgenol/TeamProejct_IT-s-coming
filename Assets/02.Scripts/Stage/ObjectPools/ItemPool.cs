using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<Item>
{
    protected override void Start()
    {
        base.Start();
        MapManager.Instance.itemPool = this;
    }
}
