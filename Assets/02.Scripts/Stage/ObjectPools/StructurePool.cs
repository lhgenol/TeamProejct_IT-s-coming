using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePool : ObjectPool<Structure>
{
    private void Awake()
    {
        MapManager.Instance.structurePool = this;
    }
}
