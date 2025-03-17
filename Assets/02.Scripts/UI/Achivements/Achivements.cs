using System;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    // �� ���������� ���� (static���� �����Ͽ� ���� ���·� ����)
    public static bool isFirstTenCoin { get; private set; } = false;
    public static bool isFirstThirtySecond { get; private set; } = false;
    public static bool isFirstRoundClear { get; private set; } = false;
    public static bool isFirstRank { get; private set; } = false;
    public static bool isFirstCostomizing { get; private set; } = false;
    // �ܹ߼� �̺�Ʈ (�� �������� �߻� �� �� ���� ȣ��)
    public static event Action OnFirstCoin;
    public static event Action OnFirstThirtySecond;
    public static event Action OnFristRoundClear;//�߰��Ϸ�
    public static event Action OnFirstRank;
    public static event Action OnFirstCostomizing;



    /// <summary>
    /// �������� Ʈ���� �޼����
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

