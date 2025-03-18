using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

// 플레이어의 좌우 이동, 점프, 슬라이드, 아이템 효과 등을 관리하는 클래스
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;             // 플레이어의 이동 속도
    public float jumpForce;             // 점프 높이
    private float jumpTime;             // 점프한 시간을 기록
    public float laneDistance;          // 트랙 간 거리 (좌/중/우 이동 시 거리 차이)
    public float slideDuration;         // 슬라이드 지속 시간
    public LayerMask groundLayerMask;   // 바닥 체크를 위한 레이어 마스크
    
    private bool isSliding = false;     // 현재 슬라이드 상태 여부
    private int currentLane = 1;        // 현재 플레이어 위치 (0=왼쪽, 1=중앙, 2=오른쪽)
    private Vector3 targetPosition;     // 목표 위치 저장 (좌/중/우 이동 시 활용)
    
    public bool isInvincible = false;   // 무적 상태 여부
    private float defaultMoveSpeed;     // 기본 이동 속도를 저장하여 원래 상태로 복구할 때 사용
    
    private Animator _animator;         // 애니메이터 변수 추가
    private Rigidbody _rigidbody;       // Rigidbody 변수 추가
    private BoxCollider _collider;

    [SerializeField]
    private Vector3 nomalColliderSize;
    [SerializeField]
    private Vector3 slideColliderSize;
    [SerializeField]
    private Vector3 dieColliderSize;

    public float moveDuration = 1f; 
    private float timeElapsed = 0f;
    private int previousLane;   // 충돌 후 원래 위치로 돌아가기 위한 변수

    
    private bool isHighJumpActive = false; // 높은 점프 모드 활성화 여부
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();   // 애니메이터 컴포넌트 가져오기
        _rigidbody = GetComponentInChildren<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
       Init();
    }
    
    void FixedUpdate()
    {
        Physics.gravity = new Vector3(0, -50f, 0);
    }

    private void Update()
    {
        // 점프 중이고 바닥에 닿았으면
        if (_animator.GetBool("IsJump") && isGrounded())
        {
            if (Time.time - jumpTime > 0.5f)
            {
                // 점프 애니메이션 해제
                _animator.SetBool("IsJump", false);
            }
        }
    }

    // 왼쪽 이동 입력 처리 (A 키)
    public void OnMoveLeft(InputAction.CallbackContext context) // context는 현재 상태를 받아올 수가 있음
    {
        // 키가 눌린 순간 실행
        if (context.phase == InputActionPhase.Started)
        {
            if (currentLane > 0)    // 왼쪽 이동 가능 여부 체크
            {
                previousLane = currentLane; // 이동 전 위치 저장
                currentLane--;
                StartCoroutine(MoveToLane());
            }
        }
    }
    
    // 오른쪽 이동 입력 처리 (D 키)
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        // 키가 눌린 순간 실행
        if (context.phase == InputActionPhase.Started)
        {
            if (currentLane < 2)    // 오른쪽 이동 가능 여부 체크
            {
                previousLane = currentLane; // 이동 전 위치 저장
                currentLane++;
                StartCoroutine(MoveToLane());
            }
        }
    }
    
    // 슬라이드 입력 처리
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isSliding && isGrounded())
        {
            isSliding = true; // 슬라이드 시작
            _animator.SetBool("IsSlide", true); // 슬라이드 애니메이션 실행
            _collider.size = slideColliderSize;
        
            Invoke(nameof(EndSlide), slideDuration); // 일정 시간 후 슬라이드 종료
        }
    }
    
    // 슬라이드 종료
    private void EndSlide()
    {
        _animator.SetBool("IsSlide", false); // 다시 Run 애니메이션 실행
        _collider.size = nomalColliderSize;
        isSliding = false; // 슬라이드 상태 해제
    }

    public void OnDie()
    {
        _collider.size = dieColliderSize;
        _collider.center = new Vector3(0, 0.9f, 0);
        enabled = false;
        Debug.Log("OnDie");
    }

    // 위치를 갱신하는 함수 (현재 레인을 기반으로 X 좌표 설정)
    private void UpdatePosition()
    {
        // X 값만 변경, 현재 높이 유지
        targetPosition = new Vector3((currentLane - 1) * laneDistance, transform.position.y, transform.position.z);

        // 바로 위치 이동
        transform.position = targetPosition;
    }
    
    private IEnumerator MoveToLane()
    {
        Vector3 startPosition = transform.position; // 시작 위치 저장
        Vector3 endPosition = new Vector3((currentLane - 1) * laneDistance, startPosition.y, startPosition.z);
        float elapsedTime = 0f;
        float duration = 0.2f; // 이동 시간

        CameraController.CameraMove(endPosition);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition; // 최종 위치 보정
        previousLane = currentLane;
    }
    
    // 점프 입력 처리 (스페이스바)
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            // 위 방향으로 순간적인 힘을 가함. 순간적으로 힘을 줄 수 있게 Impulse로 설정
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            _animator.SetBool("IsJump", true); // 점프 애니메이션 실행
            jumpTime = Time.time;   // 점프 시간 기록
        }
    }
    
    // 아이템 수집 시 높은 점프
    public void HighJump(float jumpPower)
    {
        // 위 방향으로 순간적인 힘을 가함. 순간적으로 힘을 줄 수 있게 Impulse로 설정
        _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        _animator.SetBool("IsJump", true); // 점프 애니메이션 실행
    }

    public void Init()
    {
        enabled = true;
        transform.position = Vector3.zero;
        currentLane = 1;
        targetPosition = transform.position;    // 시작 시 플레이어 위치를 기준으로 설정 
        _collider.size = nomalColliderSize;
        _collider.center= Vector3.zero + new Vector3(0f, 0.5f, 0f);
        _animator.SetBool("IsDie", false);
        _animator.SetBool("IsRun", true);  // 게임 시작 시 바로 Run 애니메이션 실행
        previousLane = currentLane;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!isInvincible)
        {// 장애물과 충돌했을 때
            if (other.gameObject.CompareTag("Obstacle1") || other.gameObject.CompareTag("Obstacle2_Hit"))
            {
                Debug.Log("넉백");
                PlayerManager.Instance.Player.ReduceHealth(); // 체력 감소 및 Hit 애니메이션 실행
                MapManager.Instance.KnockBack(0.01f, 0.1f);

                isInvincible = true;
                Invoke("Invincivbleoff", 1f);
                
                // 충돌 후 원래 레인으로 복귀
                currentLane = previousLane;
                StartCoroutine(MoveToLane());
            }
        }
        else
        {
            Debug.Log("무적상태에서 부딪");
            Obstacle obstacle=other.GetComponent<Obstacle>();
            MapManager.Instance.obstaclePool.ReturnToPool(obstacle, other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isInvincible)
        {
            if (collision.gameObject.CompareTag("Obstacle1") || collision.gameObject.CompareTag("Obstacle2_Hit"))
            {
                Debug.Log("넉백");
                PlayerManager.Instance.Player.ReduceHealth(); // 체력 감소 및 Hit 애니메이션 실행
                MapManager.Instance.KnockBack(0.01f, 0.1f);
                isInvincible = true;
                Invoke("Invincivbleoff", 1f);
                
                // 충돌 후 원래 레인으로 복귀
                currentLane = previousLane;
                StartCoroutine(MoveToLane());
                
                Debug.Log("나는 무적이다");
            }
        }
        else
        {
            Obstacle obj = collision.gameObject.GetComponent<Obstacle>();
            if (obj != null) MapManager.Instance.obstaclePool.ReturnToPool(obj, collision.gameObject);

        }
    }
    
    public void Invincivbleoff()
    {
        Debug.Log("무적풀림");
        isInvincible = false;
    }

    // 아이템 효과를 적용하는 메서드
    public void ApplyItemEffect(ItemType itemType, float duration, int value = 0)
    {
        switch (itemType)
        {
            case ItemType.JumpBoost:    // 일정 시간 동안 점프력 증가
                jumpForce *= 1.5f;     // 점프 높이 1.5배 증가
                StartCoroutine(RemoveEffectAfterTime(ItemType.JumpBoost, duration));
                break;

            case ItemType.Star: // 일정 시간 동안 무적 상태
                isInvincible = true;
                StartCoroutine(RemoveEffectAfterTime(ItemType.Star, duration));
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
                jumpForce /= 1.5f; // 원래 점프 높이로 복구
                break;
            case ItemType.Star:
                isInvincible = false; // 무적 상태 해제
                break;
        }
    }
}

