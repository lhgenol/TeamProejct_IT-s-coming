using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : ObjectPool<Obstacle>
{
    private void Awake()
    {
        MapManager.Instance.obstaclePool = this;
    }
}
