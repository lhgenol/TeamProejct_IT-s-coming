using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkContainer : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float speedMultiplier = 1f;
    [SerializeField]private bool isMoving = false;

    private void Awake()
    {
        MapManager.Instance.chunkContainer = this;
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

    public void KnockBack(float knockBackSpeedMultiplier, float knockBackDuration)
    {
        PauseMovement();
        StartCoroutine(CoroutineKnockBack(knockBackSpeedMultiplier, knockBackDuration));
    }

    IEnumerator CoroutineKnockBack(float knockBackSpeedMultiplier, float knockBackDuration)
    {
        float time = 0f;

        while (time < knockBackDuration)
        {
            transform.Translate(transform.forward * moveSpeed * knockBackSpeedMultiplier);
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

    /*public void StopMovement()
    {
        isMoving = false;
        moveSpeed = 0f;
    }*/
}