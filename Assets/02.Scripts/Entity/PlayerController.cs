using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어의 좌우 이동, 점프, 슬라이드, 아이템 효과 등을 관리하는 클래스
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;             // 플레이어의 이동 속도
    public float jumpHeight;            // 점프 높이 (jumpForce 대신 사용)
    public float jumpDuration;          // 점프 지속 시간 (올라가고 내려오는 데 걸리는 시간)
    private Vector2 curMovementInput;   // 현재 입력된 이동 값 저장
    public float laneDistance;          // 트랙 간 거리 (좌/중/우 이동 시 거리 차이)
    public float slideDuration;         // 슬라이드 지속 시간
    public LayerMask groundLayerMask;   // 바닥 체크를 위한 레이어 마스크
    
    private bool isSliding = false;     // 현재 슬라이드 상태 여부
    private bool isJumping = false;     // 현재 점프 상태인지 여부
    private int currentLane = 1;        // 현재 플레이어 위치 (0=왼쪽, 1=중앙, 2=오른쪽)
    private Vector3 targetPosition;     // 목표 위치 저장 (좌/중/우 이동 시 활용)
    
    private bool isInvincible = false;  // 무적 상태 여부
    private bool isDoubleScore = false; // 2배 점수 상태 여부
    private float defaultMoveSpeed;     // 기본 이동 속도를 저장하여 원래 상태로 복구할 때 사용
    
    private Animator _animator;         // 애니메이터 변수 추가
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();   // 애니메이터 컴포넌트 가져오기
    }

    private void Start()
    {
        targetPosition = transform.position;    // 시작 시 플레이어 위치를 기준으로 설정
        defaultMoveSpeed = moveSpeed;           // 기본 이동 속도 저장
        
        _animator.SetBool("IsRun", true); // 게임 시작 시 바로 Run 애니메이션 실행
    }

    private void Update()
    {
        if (!isJumping) // 점프 중이 아닐 때만 좌/우 이동 가능
        {
            Vector3 pos = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            pos.y = transform.position.y; // y값 고정 (점프 시 위치 유지)
            transform.position = pos;
        }
    }
    
   // 왼쪽 이동 입력 처리 (A 키)
    public void OnMoveLeft(InputAction.CallbackContext context) // context는 현재 상태를 받아올 수가 있음
    {
        // 키가 눌린 순간 실행
        if (context.phase == InputActionPhase.Started)
        {
            MoveLeft();
        }
    }
    
    // 오른쪽 이동 입력 처리 (D 키)
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        // 키가 눌린 순간 실행
        if (context.phase == InputActionPhase.Started)
        {
            MoveRight();
        }
    }
    
    // 왼쪽으로 이동하는 함수
    private void MoveLeft()
    {
        if (currentLane > 0)    // 왼쪽 이동 가능 여부 체크
        {
            currentLane--;
            UpdatePosition();   // 목표 위치 갱신
        }
    }

    // 오른쪽으로 이동하는 함수
    private void MoveRight()
    {
        if (currentLane < 2)    // 오른쪽 이동 가능 여부 체크
        {
            currentLane++;
            UpdatePosition();   // 목표 위치 갱신
        }
    }

    // 목표 위치 갱신하는 함수 (현재 레인을 기반으로 X 좌표 설정)
    private void UpdatePosition()
    {
        targetPosition = new Vector3((currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
    }
    
    // 점프 입력 처리 (스페이스바)
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded() && !isJumping)
        {
            isJumping = true;
            _animator.SetBool("IsJump", true); // 점프 애니메이션 실행

            float startY = transform.position.y;
            float targetY = startY + jumpHeight;

            // DOTween을 이용한 부드러운 점프 (올라갔다가 내려오기)
            transform.DOMoveY(targetY, jumpDuration / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    _animator.SetBool("IsJump", false); // 최고점 도달 시 점프 애니메이션 해제
                    _animator.SetBool("IsRun", true);   // 다시 달리기 애니메이션 실행

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
    
    // // 플레이어가 바닥에 있는지 확인하는 함수
    // private bool isGrounded()
    // {
    //     return Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayerMask);
    // }
    
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
    
    // 아이템 효과를 적용하는 메서드
    public void ApplyItemEffect(ItemType itemType, float duration, int value = 0)
    {
        switch (itemType)
        {
            case ItemType.Coin:         // 코인을 먹으면 점수 증가
                PlayerManager.Instance.Player.AddScore(value);
                break;

            case ItemType.JumpBoost:    // 일정 시간 동안 점프력 증가
                jumpHeight *= 1.5f;     // 점프 높이 1.5배 증가
                StartCoroutine(RemoveEffectAfterTime(ItemType.JumpBoost, duration));
                break;

            case ItemType.SpeedBoost:   // 일정 시간 동안 이동 속도 증가 (플레이어는 제자리, 맵이 빨라짐)
                moveSpeed *= 1.5f;      // 이동 속도 1.5배 증가
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
