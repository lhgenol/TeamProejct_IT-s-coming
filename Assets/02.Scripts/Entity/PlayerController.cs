using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어의 좌우 이동, 점프, 슬라이드 등을 관리하는 클래스
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;             // 이동 속도
    public float jumpPower;             // 점프 힘
    private Vector2 curMovementInput;   // 현재 이동 입력 값
    public float laneDistance;          // 트랙 간 거리 (왼쪽, 중앙, 오른쪽)
    public LayerMask groundLayerMask;   // 바닥 체크를 위한 레이어 마스크
    public float slideDuration;         // 슬라이드 지속 시간
    private bool isSliding = false;     // 현재 슬라이드 중인지 체크

    private float jumpTime;             // 점프한 시간을 기록
    private int currentLane = 1;        // 현재 플레이어 위치 (1 = 중앙)
    private Vector3 targetPosition;     // 목표 위치 저장
    
    private Rigidbody _rigidbody;       // 플레이어의 Rigidbody 가져오기
    private Animator _animator;         // 애니메이터 변수 추가
    
    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        _animator = GetComponentInChildren<Animator>();   // 애니메이터 컴포넌트 가져오기
    }

    private void Start()
    {
        targetPosition = transform.position; // 시작 위치를 현재 위치로 설정
        
        // 게임 시작하자마자 달리기 애니메이션 실행
        _animator.SetBool("IsRun", true);
    }

    private void Update()
    {
        // 점프 중이고 바닥에 닿았으면
        if (_animator.GetBool("IsJump") && isGrounded())
        {
            if (Time.time - jumpTime > 1f)
            {
                // 점프 애니메이션 해제
                _animator.SetBool("IsJump", false);
            }
        }
        
        // 목표 위치로 부드럽게 이동 (x, z 좌표만 보간)
        Vector3 pos = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        pos.y = transform.position.y;   // y 값은 유지하여 점프 시 덮어씌워지는 문제 방지
        
        transform.position = pos;   // 새로운 위치 적용
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
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            // 위 방향으로 순간적인 힘을 가함. 순간적으로 힘을 줄 수 있게 Impulse로 설정
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            
            _animator.SetBool("IsJump", true); // 점프 애니메이션 실행

            jumpTime = Time.time;   // 점프 시간 기록
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
}
