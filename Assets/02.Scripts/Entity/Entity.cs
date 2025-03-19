using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player, PlayerController, Chaser를 하나로 묶어주는 기본 클래스
/// </summary>
public class Entity : MonoBehaviour
{
    protected Animator animator; // 애니메이션 컨트롤러

    /// <summary>
    /// 자식 클래스에서 애니메이터를 오버라이드할 수 있게 설정
    /// </summary>
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>(); // 애니메이터 가져오기
    }
}
