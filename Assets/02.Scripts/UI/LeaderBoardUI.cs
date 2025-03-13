using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardUI : BaseUI
{
    public TextMeshProUGUI[] rankscore;

    protected void OnEnable()
    {
        ScoreSet();
    }
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    protected override UIState GetUIState()
    {
        return UIState.LeaderBoard;
    }

    public void ScoreSet()
    {
        int index = 0;
        foreach (var item in rankscore)
        {
            item.text = GameManager.Instance.Rank[index].ToString();
            index++;
        }
    }
}
