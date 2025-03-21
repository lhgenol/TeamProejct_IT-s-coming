﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chunk"))
        {
            Chunk chunk = other.GetComponent<Chunk>();
            MapManager.Instance.chunkPool.ReturnToPool(chunk ,other.gameObject);
        }
    }
}
