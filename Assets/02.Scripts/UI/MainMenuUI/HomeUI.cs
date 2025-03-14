using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI : BaseUI
{
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    public void OnEnable()
    {
        if(GameManager.Instance.Started)
        {
            GameManager.Instance.StopGame();
        }
    }
    protected override UIState GetUIState()
    {
        return UIState.Home;
    }
}
