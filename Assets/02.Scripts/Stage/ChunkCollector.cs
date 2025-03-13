using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chunk"))
        {
            MapManager.Instance.chunkPool.ReturnToPool(other.GetComponent<Chunk>() ,other.gameObject);
        }
    }
}
