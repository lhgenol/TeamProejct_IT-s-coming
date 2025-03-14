using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어의 좌우 이동, 점프, 슬라이드 등을 관리하는 클래스
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;             // 이동 속도
    public float jumpHeight;            // 점프 높이 (jumpForce 대신)
    public float jumpDuration;          // 점프 지속 시간
    private Vector2 curMovementInput;   // 현재 이동 입력 값
    public float laneDistance;          // 트랙 간 거리 (왼쪽, 중앙, 오른쪽)
    public float slideDuration;         // 슬라이드 지속 시간
    public LayerMask groundLayerMask;   // 바닥 체크를 위한 레이어 마스크
    private bool isSliding = false;     // 현재 슬라이드 중인지 체크
    private bool isJumping = false;     // 점프 중인지 체크

    private float jumpTime;             // 점프한 시간을 기록
    private int currentLane = 1;        // 현재 플레이어 위치 (1 = 중앙)
    private Vector3 targetPosition;     // 목표 위치 저장
    
    private bool isInvincible = false;  // 무적 상태 여부
    private bool isDoubleScore = false; // 2배 점수 상태 여부
    private float defaultMoveSpeed;     // 원래 이동 속도 저장
    
    private Animator _animator;         // 애니메이터 변수 추가
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();   // 애니메이터 컴포넌트 가져오기
    }

    private void Start()
    {
        targetPosition = transform.position; // 시작 위치를 현재 위치로 설정
        defaultMoveSpeed = moveSpeed; // 기본 속도 저장
        
        // 게임 시작하자마자 달리기 애니메이션 실행
        _animator.SetBool("IsRun", true);
    }

    private void Update()
    {
        if (!isJumping) // 점프 중에는 좌우 이동 금지
        {
            Vector3 pos = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            pos.y = transform.position.y; // y값은 그대로 유지
            transform.position = pos;
        }
    }
    
   // 왼쪽 이동 입력 처리 (A 키)
    public void OnMoveLeft(InputAction.CallbackContext context) // context는 현재 상태를 받아올 수가 있음
    {
        // 키를 누른 순간만 실행
        if (context.phase == InputActionPhase.Started)
        {
            MoveLeft();
        }
    }
    
    // 오른쪽 이동 입력 처리 (D 키)
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        // 키를 누른 순간만 실행
        if (context.phase == InputActionPhase.Started)
        {
            MoveRight();
        }
    }
    
    // 왼쪽으로 이동하는 함수
    private void MoveLeft()
    {
        if (currentLane > 0)    // 왼쪽으로 이동 가능할 경우
        {
            currentLane--;
            UpdatePosition();   // 목표 위치 갱신
        }
    }

    // 오른쪽으로 이동하는 함수
    private void MoveRight()
    {
        if (currentLane < 2)    // 오른쪽으로 이동 가능할 경우
        {
            currentLane++;
            UpdatePosition();   // 목표 위치 갱신
        }
    }

    // 목표 위치 갱신하는 함수 (현재 레인의 x 좌표를 기반으로 설정)
    private void UpdatePosition()
    {
        targetPosition = new Vector3((currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
    }
    
    // 점프 입력 처리
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded() && !isJumping)
        {
            isJumping = true;
            _animator.SetBool("IsJump", true); // 점프 애니메이션 실행

            float startY = transform.position.y;
            float targetY = startY + jumpHeight;

            // DOTween으로 부드럽게 점프 (올라갔다가 내려오기)
            transform.DOMoveY(targetY, jumpDuration / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    // 점프 최고점에 도달한 순간 바로 Run 모션으로 변경
                    _animator.SetBool("IsJump", false); 
                    _animator.SetBool("IsRun", true); // Run 모션 강제 변경

                    transform.DOMoveY(startY, jumpDuration / 2)
                        .SetEase(Ease.InQuad)
                        .OnComplete(() => 
                        {
                            isJumping = false;
                        });
                });
        }
    }
    
    // 일정 시간 후 점프 애니메이션을 해제하는 코루틴 추가
    private IEnumerator ResetJumpAnimation()
    {
        yield return new WaitForSeconds(0.01f); // 착지 후 약간의 딜레이 후 실행
        if (isGrounded()) // 확실히 바닥에 있을 때만 해제
        {
            _animator.SetBool("IsJump", false);
        }
    }
    
    // 슬라이드 입력 처리
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isSliding && isGrounded())
        {
            isSliding = true; // 슬라이드 시작
            _animator.SetBool("IsSlide", true); // 슬라이드 애니메이션 실행
        
            Invoke(nameof(EndSlide), slideDuration); // 일정 시간 후 슬라이드 종료
        }
    }
    
    // 슬라이드 종료
    private void EndSlide()
    {
        _animator.SetBool("IsSlide", false); // 다시 Run 애니메이션 실행
        isSliding = false; // 슬라이드 상태 해제
    }
    
    // 플레이어가 바닥에 있는지 확인하는 함수
    bool isGrounded()
    {
        Ray[] rays = new Ray[4]     // 네 개의 Ray를 사용하여 플레이어가 바닥에 닿았는지 확인
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
    
        // 각 Raycast가 바닥에 닿았는지 검사
        for (int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;    // 하나라도 닿으면 바닥에 있는 것으로 판단하고 true 반환
            }
        }
        return false;   // 모든 레이가 바닥에 닿지 않으면 false 반환
    }
    
    // 아이템을 먹었을 때 효과를 적용하는 메서드
    public void ApplyItemEffect(ItemType itemType, float duration, int value = 0)
    {
        switch (itemType)
        {
            case ItemType.Coin:         // 코인을 먹으면 점수 증가
                PlayerManager.Instance.Player.AddScore(value);
                break;

            case ItemType.JumpBoost:    // 일정 시간 동안 점프력 증가
                jumpHeight *= 1.5f; // 점프 높이 1.5배 증가
                StartCoroutine(RemoveEffectAfterTime(ItemType.JumpBoost, duration));
                break;

            case ItemType.SpeedBoost:   // 일정 시간 동안 이동 속도 증가 (플레이어는 제자리, 맵이 빨라짐)
                moveSpeed *= 1.5f; // 이동 속도 1.5배 증가
                StartCoroutine(RemoveEffectAfterTime(ItemType.SpeedBoost, duration));
                break;

            case ItemType.Invincibility: // 일정 시간 동안 무적 상태
                isInvincible = true;
                StartCoroutine(RemoveEffectAfterTime(ItemType.Invincibility, duration));
                break;

            case ItemType.DoubleScore:  // 일정 시간 동안 점수 2배 증가
                isDoubleScore = true;
                StartCoroutine(RemoveEffectAfterTime(ItemType.DoubleScore, duration));
                break;
        }
    }
    
    // 일정 시간 후 아이템 효과를 제거하는 코루틴. WaitForSeconds(duration)으로 대기 후 원래 값으로 복구
    private IEnumerator RemoveEffectAfterTime(ItemType itemType, float duration)
    {
        yield return new WaitForSeconds(duration); // 설정된 시간 동안 대기

        switch (itemType)
        {
            case ItemType.JumpBoost:
                jumpHeight /= 1.5f; // 원래 점프 높이로 복구
                break;

            case ItemType.SpeedBoost:
                moveSpeed = defaultMoveSpeed; // 원래 이동 속도로 복구
                break;

            case ItemType.Invincibility:
                isInvincible = false; // 무적 상태 해제
                break;

            case ItemType.DoubleScore:
                isDoubleScore = false; // 2배 점수 해제
                break;
        }
    }
}

// 아이템 타입을 정의하는 열거형
public enum ItemType
{
    Coin,          // 점수 증가 아이템
    JumpBoost,     // 점프력 증가 아이템
    SpeedBoost,    // 이동 속도 증가 아이템
    Invincibility, // 무적 아이템
    DoubleScore    // 점수 2배 증가 아이템
}
