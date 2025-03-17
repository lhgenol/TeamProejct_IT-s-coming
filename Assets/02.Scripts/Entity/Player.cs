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

    public void Awake()
    {
        // 싱글톤 PlayerManager의 Player 속성에 현재 인스턴스를 설정
        PlayerManager.Instance.SetPlayer(this);
        
        // PlayerController 컴포넌트를 가져옴
        controller = GetComponent<PlayerController>();
        
        // Animator 컴포넌트 가져오기
        animator = GetComponentInChildren<Animator>();
    }
    
    private void OnEnable()
    {
        animator.SetBool("IsRun", true); 
        animator.SetBool("IsDie", false); // 죽은 상태 초기화
    }
    
    private void Update()
    {
        
    }

    // Hit 애니메이션이 끝난 후 다시 달리기 애니메이션 실행
    private IEnumerator ResetToRunAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Hit 애니메이션 지속 시간만큼 대기
        //PlayAnimation("Run");
        animator.SetBool("IsRun", true); 
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
        
        animator.SetBool("IsDie", true); 
        
        // 플레이어 이동 & 점프 막기
        if (controller != null)
        {
            controller.OnDie();
        }
        
        GameManager.Instance.EndGame(); // 게임 종료
    }
    
    // 장애물과 충돌했을 때 체력을 깎고 추적자를 등장시키는 메서드
    public void ReduceHealth()
    {
        if (health > 0)
        {
            animator.SetTrigger("Hit"); // Hit 애니메이션 실행
        }
    
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
        Die();
    }
}
