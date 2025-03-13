using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkContainer : MonoBehaviour
{
    public float moveSpeed = 5f; // 
    public float speedMultiplier = 1f;
    public float knockBackMultiplier = 0.2f;
    public float knockBackDuration = 0.3f;
    private bool isMoving = false;

    private void Start()
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
        transform.Translate(Vector3.back * moveSpeed * speedMultiplier * Time.deltaTime);
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
            transform.Translate(Vector3.forward * moveSpeed * knockBackMultiplier);
            time += Time.deltaTime;
            yield return null;
        }

        ResumeMovement();
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
        moveSpeed = 0f; // 속도를 0으로 설정하여 완전히 멈춤
    }
}