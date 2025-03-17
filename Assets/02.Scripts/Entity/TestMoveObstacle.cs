using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveObstacle : MonoBehaviour
{
    public float moveDistance = 10f; // 이동할 거리
    public float moveSpeed = 2f;    // 이동 속도

    private Vector3 startPosition;  // 초기 위치

    void Start()
    {
        // 초기 위치 저장
        startPosition = transform.position;
    }

    void Update()
    {
        // 앞뒤로 움직이기
        float movement = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z + movement);
    }
}
