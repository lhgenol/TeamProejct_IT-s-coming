using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 체력, 점수, 점프 횟수 등을 관리하는 클래스
public class Player : Entity
{
    public PlayerController controller;
    public int health;          // 플레이어 체력 (2번 부딪히면 게임 오버)
    public int score;           // 플레이어 점수
    private bool isCaught = false; // 플레이어가 잡혔는지 여부
    
    // [SerializeField] private float rayDistance = 3f;  // 레이 길이
    // [SerializeField] private LayerMask ObstacleLayer;   // 장애물 레이어 설정

    public void Awake()
    {
        // 싱글톤 PlayerManager의 Player 속성에 현재 인스턴스를 설정
        PlayerManager.Instance.SetPlayer(this);
        
        // PlayerController 컴포넌트를 가져옴
        controller = GetComponent<PlayerController>();
    }

    protected override void Start()
    {
        base.Start();
        //PlayAnimation(""); // 기본적으로 달리는 애니메이션 실행
    }
    private void OnEnable()
    {
        PlayAnimation("Run");
    }
    private void Update()
    {
        
    }

    // Hit 애니메이션이 끝난 후 다시 달리기 애니메이션 실행
    private IEnumerator ResetToRunAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Hit 애니메이션 지속 시간만큼 대기
        PlayAnimation("Run");
    }

    // 점수를 추가하는 함수
    public void AddScore(int amount)
    {
        score += amount;
    }
    
    // 플레이어가 죽었을 때 실행되는 함수
    private void Die()
    {
        Debug.Log("Game Over");
    }
    
    // 장애물과 충돌했을 때 체력 감소 및 위치가 올라가는 메서드
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Obstacle1") || other.CompareTag("Obstacle2_Hit"))
    //     {
    //         ReduceHealth(); // 체력 감소 및 Hit 애니메이션 실행
    //     }
    // }
    //
    // // 장애물2 위에 올라갈 조건 확인
    // private void CheckForObstacleTop()
    // {
    //     RaycastHit hit;
    //     Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red);
    //
    //     // 플레이어 아래 방향으로 레이 발사
    //     if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, ObstacleLayer))
    //     {
    //         Debug.Log("레이 발사");
    //         // 장애물2의 윗면(Top)에 닿았는지 확인
    //         if (hit.collider.CompareTag("Obstacle2_Top") && IsJumping())
    //         {
    //             ClimbObstacle(hit.point.y);
    //         }
    //     }
    // }
    //
    // // 장애물2 위로 올라가는 로직
    // private void ClimbObstacle(float obstacleTopY)
    // {
    //     float newY = 5.0f;   // 장애물 높이에 맞춰 플레이어 위치 조정
    //     transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    // }
    //
    // // 플레이어가 점프 중인지 체크 (예제 코드)
    // private bool IsJumping()
    // {
    //     return !Physics.Raycast(transform.position, Vector3.down, 0.1f); // 바닥에 닿지 않았으면 점프 중
    // }
    
    // 장애물과 충돌했을 때 체력을 깎고 추적자를 등장시키는 메서드
    public void ReduceHealth()
    {
        animator.SetTrigger("Hit"); // Hit 애니메이션 실행
        health--; // 체력 감소
        
        if (health == 1)
        {
            // 첫 번째 충돌 시 추적자 등장
            Chaser chaser = FindObjectOfType<Chaser>(); // Chaser 찾기
            if (chaser != null)
            {
                chaser.StartChasing();
            }
        }
        else if (health <= 0)
        {
            // 두 번째 충돌 시 플레이어 잡기 + 죽음 연출
            Chaser chaser = FindObjectOfType<Chaser>(); // Chaser 찾기
            if (chaser != null)
            {
                chaser.CatchPlayer();
            }
            GetCaught(); // 플레이어 사망 연출
        }
    }
    
    // 플레이어가 잡혔을 때 실행
    private void GetCaught()
    {
        isCaught = true;
        GameManager.Instance.EndGame();
        PlayAnimation("Die"); // 사망 애니메이션 실행
        Debug.Log("죽었다");
    }
}
