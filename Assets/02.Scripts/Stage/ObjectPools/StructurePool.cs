using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePool : ObjectPool<Structure>
{
    protected override void Start()
    {
        base.Start();
        MapManager.Instance.structurePool = this;
    }

}
