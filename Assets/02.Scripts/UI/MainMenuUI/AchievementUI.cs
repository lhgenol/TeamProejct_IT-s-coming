using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUI : BaseUI
{
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    protected override UIState GetUIState()
    {
        return UIState.Achivement;
    }
}
