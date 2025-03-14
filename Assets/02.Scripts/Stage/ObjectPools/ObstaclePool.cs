using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : ObjectPool<Obstacle>
{
    private void Awake()
    {
        MapManager.Instance.obstaclePool = this;
    }

    protected override void Start()
    {
        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabs = new List<GameObject>();

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.obstacleList != null) // null 체크
                {
                    prefabs.AddRange(theme.obstacleList);
                }
            }

            GameObject[] prefabArray = prefabs.ToArray();
        }

        base.Start();
    }
}
