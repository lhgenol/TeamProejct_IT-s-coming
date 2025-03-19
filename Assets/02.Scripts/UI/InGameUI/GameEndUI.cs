using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndUI : BaseUI
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI rank;
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    /// <summary>
    /// 플레이어가 사망하면 실행
    /// 실행시 점수와 코인 출력
    /// 등수가 등재되면 등수도 출력
    /// </summary>
    private void OnEnable()
    {
        score.text= GameManager.Instance.Score.ToString();
        coin.text= GameManager.Instance.Coin.ToString();
        if(GameManager.Instance.NewRank)
        {
            rank.gameObject.SetActive(true);
            rank.text = GameManager.Instance.RankIndex.ToString() + "등 달성!";
        }
    }
    protected override UIState GetUIState()
    {
        return UIState.GameEnd;
    }
}
