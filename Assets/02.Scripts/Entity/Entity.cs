using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player, PlayerController, Chaser를 하나로 묶어주는 클래스
public class Entity : MonoBehaviour
{
    protected Animator animator; // 애니메이션 컨트롤러

    // (자식 클래스에서 오버라이드 가능)
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>(); // 애니메이터 가져오기
    }
}
