using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : ObjectPool<Obstacle>
{
    protected override void Start()
    {
        base.Start();
        MapManager.Instance.obstaclePool = this;
    }
}
