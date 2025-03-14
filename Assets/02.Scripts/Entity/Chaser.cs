using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추적자 NPC를 관리하는 클래스. 플레이어를 따라 이동하고, 플레이어가 장애물에 부딪히면 등장하고 추격하는 역할
public class Chaser : Entity
{
    public Transform player;    // 플레이어 위치 정보
    public float speed;         // 추격 속도
    private bool isChasing = false; // 추격 상태 확인

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false); // 시작할 때 비활성화

        // 플레이어 찾기
        if (PlayerManager.Instance.Player != null)
        {
            player = PlayerManager.Instance.Player.transform;
        }
        else
        {
            Debug.LogError("PlayerManager에서 플레이어를 찾을 수 없습니다.");
        }
    }
    
    // 추격 시작 함수 (장애물과 충돌 시 실행됨)
    public void StartChasing()
    {
        if (player == null) return; // 플레이어가 없으면 실행 안 함

        isChasing = true;
        gameObject.SetActive(true); // 추적자 활성화
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z); // X축만 플레이어 근처로 이동
        PlayAnimation("Run"); // 추적자가 뛰는 애니메이션 실행
    }
    
    private void Update()
    {
        if (isChasing)
        {
            FollowPlayer();
        }
    }
    
    public void FollowPlayer()
    {
        if (player == null) return;

        // 플레이어 위치를 따라 좌우(X축) 이동만 수행
        Vector3 targetPosition = transform.position;
        targetPosition.x = player.position.x;

        // 추격자 Y축 이동 제한 (필요 시 y 보정 가능)
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
    
    // 플레이어가 2번째 충돌하면 잡기 애니메이션 실행
    public void CatchPlayer()
    {
        isChasing = false;      // 추적 중지
        PlayAnimation("Catch"); // 추적자 잡기 애니메이션 실행
        Debug.Log("게임 오버! 추적자에게 잡혔습니다.");
    }
}
