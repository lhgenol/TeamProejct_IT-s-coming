using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 체력, 점수, 점프 횟수 등을 관리하는 클래스
public class Player : Entity
{
    public PlayerController controller;
    public int health;          // 플레이어 체력 (2번 부딪히면 게임 오버)
    public int score;           // 플레이어 점수
    public int maxJumps;        // 최대 점프 가능 횟수
    private int currentJumps;   // 현재 사용한 점프 횟수

    public void Awake()
    {
        // 싱글톤 PlayerManager의 Player 속성에 현재 인스턴스를 설정
        PlayerManager.Instance.Player = this;
        
        // PlayerController 컴포넌트를 가져옴
        controller = GetComponent<PlayerController>();
    }

    protected override void Start()
    {
        base.Start();
        PlayAnimation("Run"); // 기본적으로 달리는 애니메이션 실행
    }

    // 체력을 줄이는 함수 (장애물과 충돌했을 때 호출)
    public void ReduceHealth()
    {
        health--;   // 체력 감소
        if (health == 1)
        {
            //Chaser.Instance.StartChasing(); // 최초 충돌 시 Chaser 등장
        }
        else if (health <= 0)
        {
            Die();  // 체력이 0이 되면 게임 오버
        }
    }

    // 점수를 추가하는 함수
    public void AddScore(int amount)
    {
        score += amount;
    }

    // 점프할 수 있는지 확인하는 함수
    public bool CanJump()
    {
        return currentJumps < maxJumps;
    }
    
    // 점프를 실행하는 함수
    public void Jump()
    {
        if (CanJump())
        {
            currentJumps++; // 점프 횟수 증가
            PlayAnimation("Jump"); // 점프 애니메이션 실행
        }
    }

    // 땅에 닿았을 때 점프 횟수 초기화
    public void ResetJumps()
    {
        currentJumps = 0;
    }

    // 플레이어가 죽었을 때 실행되는 함수
    private void Die()
    {
        Debug.Log("Game Over");
    }
}
