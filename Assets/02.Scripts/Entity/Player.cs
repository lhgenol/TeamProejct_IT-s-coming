using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 체력, 점수, 점프 횟수 등을 관리하는 클래스
/// </summary>
public class Player : Entity
{
    public PlayerController controller;
    public int health;              // 플레이어 체력 (2번 부딪히면 게임 오버)
    private bool isCaught = false;  // 플레이어가 잡혔는지 여부
    
    private Chaser chaser;          // 추적자(Chaser) 객체

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
    }
    
    /// <summary>
    /// 플레이어 상태를 초기화하는 함수
    /// </summary>
    public void Init()
    {
        health = 2;
        isCaught = false; // 플레이어가 잡혔는지 여부
    }
    
    /// <summary>
    /// 플레이어 체력을 감소시키는 함수
    /// 체력이 1이 되면 Chaser가 추격 시작.
    /// 체력이 0이 되면 Chaser에게 잡히고 게임 종료.
    /// </summary>
    public void ReduceHealth()
    {
        if (health > 0)
        {
            animator.SetTrigger("Hit");
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
    
    /// <summary>
    /// 충돌 후 일정 시간이 지나면 체력을 회복하는 함수
    /// 체력이 1이면 5초 후 2로 회복, Chaser는 물러남.
    /// </summary>
    /// <param name="delay">회복되기까지의 대기 시간(초)</param>
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
    
    /// <summary>
    /// 아이템을 통해 체력을 회복하는 함수
    /// 체력이 2 미만이면 1 증가, Chaser의 상태를 갱신.
    /// </summary>
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
    
    /// <summary>
    /// 플레이어가 사망했을 때 실행되는 함수
    /// 애니메이션을 변경, 이동을 막은 후 게임을 종료.
    /// </summary>
    private void Die()
    {
        animator.SetBool("IsRun", false);
        animator.SetBool("IsDie", true); 
        
        // 플레이어 이동 & 점프 막기
        if (controller != null)
        {
            controller.OnDie();
        }
        
        GameManager.Instance.EndGame(); // 게임 종료
    }

    /// <summary>
    /// 플레이어가 Chaser에게 잡혔을 때 실행되는 함수
    /// </summary>
    private void GetCaught()
    {
        isCaught = true;
        Die();
    }
}
