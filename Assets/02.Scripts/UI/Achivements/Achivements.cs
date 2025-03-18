using System;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public static bool isFirstTenCoin { get; private set; } = false;
    public static bool isFirstRoundClear { get; private set; } = false;
    public static bool isFirstRank { get; private set; } = false;
    public static bool isFirstCostomizing { get; private set; } = false;
    public static event Action OnFirstCoin;
    public static event Action OnFristRoundClear;
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

