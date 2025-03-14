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
    private void OnEnable()
    {
        score.text= GameManager.Instance.Score.ToString();
        coin.text= GameManager.Instance.Coin.ToString();
        if(GameManager.Instance.NewRank)
        {
            rank.gameObject.SetActive(true);
            rank.text = GameManager.Instance.RankIndex.ToString() + "µî ´Þ¼º!";
        }
    }
    protected override UIState GetUIState()
    {
        return UIState.GameEnd;
    }
}
