using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkContainer : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float speedMultiplier = 1f;
    public float knockBackMultiplier = 0.2f;
    public float knockBackDuration = 0.3f;
    [SerializeField]private bool isMoving = false;

    private void Start()
    {
        MapManager.Instance.chunkContainer = this;
        MapManager.Instance.chunkSpawner.chunkContainer = this.transform;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveContainer();
        }
    }

    void MoveContainer()
    {
        transform.Translate(-transform.forward * moveSpeed * speedMultiplier * Time.deltaTime);
    }

    public void KnockBack()
    {
        PauseMovement();
        StartCoroutine(CoroutineKnockBack());
    }

    IEnumerator CoroutineKnockBack()
    {
        float time = 0f;

        while (time < knockBackDuration)
        {
            transform.Translate(transform.forward * moveSpeed * knockBackMultiplier);
            time += Time.deltaTime;
            yield return null;
        }

        ResumeMovement();
    }

    public void ChangeSpeedMultiplier(float multiplier,float duration)
    {
        speedMultiplier = multiplier;
        StartCoroutine(CoroutineChangeSpeedMultiplier(duration));
    }

    IEnumerator CoroutineChangeSpeedMultiplier(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        speedMultiplier = 1f;
    }
    public void PauseMovement()
    {
        isMoving = false;
    }

    public void ResumeMovement()
    {
        isMoving = true;
    }

    public void StopMovement()
    {
        isMoving = false;
        moveSpeed = 0f;
    }
}