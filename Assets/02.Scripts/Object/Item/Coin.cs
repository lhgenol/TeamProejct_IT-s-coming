using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    protected override void ApplyEffect(GameObject player) 
    {
        SoundManager.Instance.PlaySFX(2);
        GameManager.Instance.GetCoin(); // 점수 증가
    }
}
