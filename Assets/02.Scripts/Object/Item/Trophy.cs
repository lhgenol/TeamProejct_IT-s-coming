using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : Item
{
    public float trophyDuration = 5f; // 5초 동안 효과 지속
    private bool isTrophyActive = false;

    protected override void ApplyEffect(GameObject player)
    {
        ItemManager.Instance.StartExternalCoroutine(TrophyEffect());
    }

    private IEnumerator TrophyEffect()
    {
        ItemManager.Instance.coinMultiplier = 2; // 코인 획득량 2배 증가
        yield return new WaitForSeconds(trophyDuration);
        ItemManager.Instance.coinMultiplier = 1; // 원래 값으로 복구
    }
}
