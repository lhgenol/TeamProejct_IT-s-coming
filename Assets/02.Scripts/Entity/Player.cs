using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 체력, 점수, 점프 횟수 등을 관리하는 클래스
public class Player : Entity
{
    public PlayerController controller;
    public int health;          // 플레이어 체력 (2번 부딪히면 게임 오버)
    private bool isCaught = false; // 플레이어가 잡혔는지 여부
    
    private Chaser chaser;  // Chaser 저장 변수

    public void Awake()
    {
        // 싱글톤 PlayerManager의 Player 속성에 현재 인스턴스를 설정
        PlayerManager.Instance.SetPlayer(this);

        // PlayerController 컴포넌트를 가져옴
        controller = GetComponent<PlayerController>();

        // Animator 컴포넌트 가져오기
        animator = GetComponentInChildren<Animator>();

        // Chaser 찾기
        chaser = FindObjectOfType<Chaser>(true);
        if (chaser == null)
        {
            Debug.Log("Chaser를 찾을 수 없습니다");
        }
    }
    
    public void Init()
    {
        health = 2;
        isCaught = false; // 플레이어가 잡혔는지 여부
    }
    
    // 플레이어의 체력은 기본 2로 설정
    // 플레이어가 한 대 맞으면 체력이 1이 되고, Chaser가 화면에 등장(추적)
    // 일정 시간이 지나면 Chaser가 뒤로 서서히 빠지고 체력은 다시 2
    // 체력 1인 상태에서 플레이어다 한 번 더 맞으면 플레이어는 Die, Chaser는 잡는 모션 실행
    
    // 플레이어 체력 감소 관리
    public void ReduceHealth()
    {
        Debug.Log($"체력 감소! 현재 체력: {health}"); // 디버그 로그 추가
        if (health > 0)
        {
            animator.SetTrigger("Hit"); // Hit 애니메이션 실행
            health--; // 체력 감소

            if (chaser != null)
            {
                if (health == 1)
                {
                    chaser.StartChasing(); // 첫 번째 충돌. 추적 시작
                    StartCoroutine(RecoverHealth(5f)); // 5초 후 체력 회복 & Chaser 사라짐
                }
                else if (health <= 0)
                {
                    chaser.CatchPlayer(); // 두 번째 충돌. 잡힘
                    GetCaught(); // 플레이어 사망 처리
                }
            }
        }
    }
    
    // 플레이어 체력 회복(충돌 후 일정 시간이 지나면)
    private IEnumerator RecoverHealth(float delay)
    {
        yield return new WaitForSeconds(delay); // 일정 시간 대기
        if (health == 1) // 체력이 1인 상태라면
        {
            health = 2; // 체력 회복
            if (chaser != null)
            {
                // Chaser가 서서히 물러나도록 설정
                StartCoroutine(chaser.RetreatAndDeactivate());
            }
        }
    }
    
    // 플레이어 체력 회복(아이템 사용 시)
    public void AddHealth()
    {
        if (health < 2)
        {
            health++;

            if (chaser != null && chaser.state == ChaserState.Catching)
            {
                // 체력이 회복되었을 때, 추적 상태로 돌아가도록 설정
                chaser.state = ChaserState.Chasing;
            
                // Chaser의 Animator 가져와서 상태 변경
                Animator chaserAnimator = chaser.GetAnimator();
                if (chaserAnimator != null)
                {
                    chaserAnimator.SetBool("Catch", false);
                    chaserAnimator.SetBool("IsRun", true);
                }
            }
        }
    }
    
    // 플레이어가 죽었을 때 실행되는 함수
    private void Die()
    {
        Debug.Log("Game Over");
        animator.SetBool("IsRun", false);
        animator.SetBool("IsDie", true); 
        
        // 플레이어 이동 & 점프 막기
        if (controller != null)
        {
            controller.OnDie();
        }
        
        GameManager.Instance.EndGame(); // 게임 종료
    }

    // 플레이어가 잡혔을 때 실행
    private void GetCaught()
    {
        isCaught = true;
        Die();
    }
}
