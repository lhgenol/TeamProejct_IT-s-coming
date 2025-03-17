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
        base.Start();   // 부모 클래스(Entity)의 Start 함수 호출
        gameObject.SetActive(false); // 시작할 때 비활성화

        // 플레이어 찾기 (PlayerManager에서 플레이어 객체가 존재한다면)
        if (PlayerManager.Instance.Player != null)
        {
            // 플레이어 위치 추적을 위해 transform 할당
            player = PlayerManager.Instance.Player.transform;
        }
        else
        {
            Debug.LogError("PlayerManager에서 플레이어를 찾을 수 없습니다.");
        }
    }
    
    // 추적 시작 함수 (장애물과 충돌 시 실행됨)
    public void StartChasing()
    {
        if (player == null) return; // 플레이어가 없으면 추적 X

        isChasing = true;           // 추적 상태를 true로 설정
        gameObject.SetActive(true); // 추적자 활성화
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z); // X축 위치를 플레이어와 맞추기
        animator.SetBool("IsRun", true);    // 추적자가 뛰는 애니메이션 실행
    }
    
    private void Update()
    {
        if (isChasing)  // 추적 중이라면
        {
            FollowPlayer(); // 플레이어를 추적하는 메서드 호출
        }
    }
    
    // 추적 메서드
    public void FollowPlayer()
    {
        if (player == null) return; // 플레이어가 없으면 추격하지 않도록 처리

        // 플레이어 위치를 따라 좌우(X축) 이동만 수행
        Vector3 targetPosition = transform.position; // 현재 추적자의 위치
        targetPosition.x = player.position.x; // 추적자가 플레이어와 X축 위치를 동일하게 맞춤

        // 추격자 Y축 이동 제한 (필요 시 y 보정 가능)
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
    
    // 플레이어가 2번째 충돌하면 잡기 애니메이션 실행
    public void CatchPlayer()
    {
        isChasing = false;      // 추적 상태를 false로 설정해 추적 중지
        animator.SetBool("Catch", true);    // 추작자가 플레이어를 잡는 애니메이션 실행
        Debug.Log("게임 오버! 추적자에게 잡혔습니다.");
    }
}
