using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추적자 NPC를 관리하는 클래스. 플레이어를 따라 이동하고, 플레이어가 장애물에 부딪히면 등장하고 추격하는 역할
public class Chaser : Entity
{
    public static Chaser Instance { get; private set; } // 싱글톤
    public Transform player;    // 플레이어 위치 정보
    public float speed;         // 추격 속도
    private bool isChasing = false; // 추격 상태 확인

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>(); // 애니메이터 가져오기
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(false); // 시작할 때 비활성화
    }
    
    // 추격 시작 함수 (장애물과 충돌 시 실행됨)
    public void StartChasing()
    {
        isChasing = true;
        gameObject.SetActive(true); // 추적자 활성화
        PlayAnimation("Run"); // 추적자가 뛰는 애니메이션 실행
    }

    // private void Update()
    // {
    //     if (isChasing)
    //     {
    //         // Chaser가 플레이어를 따라가도록 설정
    //         transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.deltaTime);
    //     }
    // }
    
    private void Update()
    {
        if (isChasing)
        {
            FollowPlayer();
        }
    }
    
    public void FollowPlayer()
    {
        // 플레이어 위치를 따라 좌우 이동
        Vector3 targetPosition = transform.position;
        targetPosition.x = player.position.x;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
    
    // 플레이어가 2번째 충돌하면 잡기 애니메이션 실행
    public void CatchPlayer()
    {
        PlayAnimation("Catch");
        Debug.Log("Chaser caught the player! Game Over.");
    }
    
    public void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.SetTrigger(animationName);
        }
    }
}
