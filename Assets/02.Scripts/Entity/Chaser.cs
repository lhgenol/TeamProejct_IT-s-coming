using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChaserState
{
    Inactive,   // Chaser 비활성화
    Chasing,    // 플레이어를 추적하는 상태
    Jumping,    // 추적자가 점프한 상태
    Catching    // 추적한 상태
}

/// <summary>
/// 추적자 NPC를 관리하는 클래스.
/// 플레이어가 장애물에 부딪히면 등장해 플레이어를 따라 이동하고, 특정 조건에서 행동을 수행.
/// </summary>
public class Chaser : Entity
{
    public Transform player;        // 플레이어 위치 정보
    public float speed = 5f;        // 추격 속도
    public float jumpDelay = 0.2f;  // 점프 딜레이
    public float retreatSpeed = 1f; // 뒤로 물러나는 속도
    public float retreatTime = 3f;  // 뒤로 물러나는 시간
    
    private bool isJumping = false; // 점프 중인지 확인
    public ChaserState state = ChaserState.Inactive; // 현재 상태
    
    private Vector3 originalPosition; // 초기 위치 저장
    
    protected override void Start()
    {
        base.Start();
        Init();
    }
    
    /// <summary>
    /// Chaser를 초기화하고 기본 상태로 설정하는 메서드
    /// </summary>
    public void Init()
    {
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
        
        state = ChaserState.Inactive; // 기본 상태로 초기화
        animator.SetBool("Catch", false); // 잡기 애니메이션 해제
        animator.SetBool("IsRun", true); // 다시 달리는 상태로 변경
    }
    
    /// <summary>
    /// 플레이어가 장애물에 부딪혔을 때 추적을 시작하는 메서드
    /// </summary>
    public void StartChasing()
    {
        if (player == null) return; // 플레이어가 없으면 추적 X

        state = ChaserState.Catching;    // 추적 상태를 true로 설정
        gameObject.SetActive(true); // 추적자 활성화
        
        // 플레이어보다 뒤쪽(Z -2) & 살짝 아래쪽(Y -0.5)에서 등장
        transform.position = new Vector3(player.position.x, player.position.y - 0.5f, player.position.z - 1.5f);
        originalPosition = transform.position; // 초기 위치 저장
        
        animator.SetBool("IsRun", true);    // 추적자가 뛰는 애니메이션 실행
    }
    
    private void Update()
    {
        if (state == ChaserState.Catching) // 추적 중일 때만 실행
        {
            FollowPlayer();
        }
    }
    
    public Animator GetAnimator()
    {
        return animator;
    }
    
    /// <summary>
    /// 플레이어의 X축을 따라 이동하는 추적 메서드
    /// </summary>
    public void FollowPlayer()
    {
        if (player == null) return; // 플레이어가 없으면 추격하지 않도록 처리

        // 플레이어의 X축을 따라감 (부드럽게)
        Vector3 targetPosition = transform.position;
        targetPosition.x = player.position.x;   // 추적자가 플레이어와 X축 위치를 동일하게 맞춤
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
    
    /// <summary>
    /// 일정 시간 동안 뒤로 물러난 후 추적자를 비활성화하는 코루틴
    /// </summary>
    public IEnumerator RetreatAndDeactivate()
    {
        float elapsedTime = 0f;

        // 뒤로 물러나는 동안
        while (elapsedTime < retreatTime)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition - new Vector3(0, 0, 4f), retreatSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 물러난 후 Chaser 비활성화
        gameObject.SetActive(false);
        state = ChaserState.Inactive;
    }
    
    /// <summary>
    /// 플레이어가 점프할 때 호출. 추적자도 일정 딜레이 후 점프하는 메서드
    /// </summary>
    public void Jump()
    {
        if (!isJumping && state == ChaserState.Chasing)
        {
            StartCoroutine(DelayedJump());
        }
    }
    
    /// <summary>
    /// 일정 시간 후 추적자가 점프하는 코루틴
    /// </summary>
    private IEnumerator DelayedJump()
    {
        isJumping = true;
        state = ChaserState.Jumping; // 점프 상태로 변경
        yield return new WaitForSeconds(jumpDelay);
        animator.SetBool("IsJump", true);
        state = ChaserState.Chasing; // 다시 추적 상태로 변경
        isJumping = false;
    }

    /// <summary>
    /// 플레이어가 두 번째 충돌을 하면 추적자가 플레이어를 잡는 메서드
    /// </summary>
    public void CatchPlayer()
    {
        state = ChaserState.Catching; // 잡기 상태
        animator.SetBool("Catch", true);
    }
}