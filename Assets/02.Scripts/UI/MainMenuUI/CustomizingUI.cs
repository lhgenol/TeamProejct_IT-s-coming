using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingUI : BaseUI
{
    public Button[] hats;
    public Button[] outWears;
    public Button[] pants;
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    protected override UIState GetUIState()
    {
        return UIState.Customizing;
    }
}
