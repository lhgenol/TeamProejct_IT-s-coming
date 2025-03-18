using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [Header("ThemeData")]
    public ThemeDataSO[] themeData;

    [Header("ObjectPool")]
    public ChunkPool chunkPool;
    public ObstaclePool obstaclePool;
    public ItemPool itemPool;
    public StructurePool structurePool;
    //public StructurePool structurePool;

    public ChunkContainer chunkContainer;
    public ChunkSpawner chunkSpawner;
    public CurChunkCheck CurChunkCheck;

    public void ResumeMove()
    {
        if (chunkContainer != null) chunkContainer.ResumeMovement();
        else Debug.Log($"{chunkContainer.name} is null");
    }
    public void PauseMove()
    {
        if (chunkContainer != null) chunkContainer.PauseMovement();
        else Debug.Log($"{chunkContainer.name} is null");
    }

    public void KnockBack(float knockBackSpeedMultiplier, float knockBackDuration)
    {
        Debug.Log("넉백");
        if (chunkContainer != null) chunkContainer.KnockBack(knockBackSpeedMultiplier, knockBackDuration);
        else Debug.Log($"{chunkContainer.name} is null");
    }

    public void SpeedUp(float multiplier, float duration)
    {
        if (chunkContainer != null) chunkContainer.ChangeSpeedMultiplier(multiplier, duration);
        else Debug.Log($"{chunkContainer.name} is null");
    }

    public void ResetChunks()
    {
        if (chunkSpawner != null) chunkSpawner.Reset();
        else Debug.Log($"{chunkSpawner.name} is null");
    }
}
