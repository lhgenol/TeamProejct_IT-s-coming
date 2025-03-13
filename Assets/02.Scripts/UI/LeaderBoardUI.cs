using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardUI : BaseUI
{
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    protected override UIState GetUIState()
    {
        return UIState.LeaderBoard;
    }
}
