using System;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    // 각 도전과제의 상태 (static으로 선언하여 전역 상태로 관리)
    public static bool isFirstTenCoin { get; private set; } = false;
    public static bool isFirstThirtySecond { get; private set; } = false;
    public static bool isFirstRoundClear { get; private set; } = false;
    public static bool isFirstRank { get; private set; } = false;
    public static bool isFirstCostomizing { get; private set; } = false;
    // 단발성 이벤트 (각 도전과제 발생 시 한 번만 호출)
    public static event Action OnFirstCoin;
    public static event Action OnFirstThirtySecond;
    public static event Action OnFristRoundClear;//추가완료
    public static event Action OnFirstRank;
    public static event Action OnFirstCostomizing;



    /// <summary>
    /// 도전과제 트리거 메서드들
    /// </summary>
    public static void TriggerFirstTenCoin()
    {
        if (!isFirstTenCoin)
        {
            isFirstTenCoin = true;
            OnFirstCoin?.Invoke();
        }
    }

    public static void TriggerThirtySecond()
    {
        if (!isFirstThirtySecond)
        {
            isFirstThirtySecond = true;
            OnFirstThirtySecond?.Invoke();
        }
    }


    public static void TriggerFirstRoundClear()
    {
        if (!isFirstRoundClear)
        {
            isFirstRoundClear = true;
            OnFristRoundClear?.Invoke();
        }
    }

    public static void TriggerFirstRank()
    {
        if (!isFirstRank)
        {
            isFirstRank = true;
            OnFirstRank?.Invoke();
        }
    }

    public static void TriggerFirstCostomizing()
    {
        if (!isFirstCostomizing)
        {
            isFirstCostomizing = true;
            OnFirstCostomizing?.Invoke();
        }
    }    
}

