using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : ObjectPool<Obstacle>
{
    private void Awake()
    {
        MapManager.Instance.obstaclePool = this;

        if (MapManager.Instance.themeData != null)
        {
            List<GameObject> prefabsList = new List<GameObject>();

            foreach (var theme in MapManager.Instance.themeData)
            {
                if (theme.obstacleList != null) // null 체크
                {
                    prefabsList.AddRange(theme.obstacleList);
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
