using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    private void OnEnable()
    {
        GameManager.Instance.PauseGame();
    }

    protected override UIState GetUIState()
    {
        return UIState.Pause;
    }
}
