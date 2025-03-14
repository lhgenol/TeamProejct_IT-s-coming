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
    void Start()
    {
        SetUpButtonListeners();
    }
    protected override UIState GetUIState()
    {
        return UIState.Customizing;
    }
    void SetUpButtonListeners()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i + 1; 
            hats[i].onClick.AddListener(() => OnButtonClick("Hat", index));
        }

        for (int i = 0; i < outWears.Length; i++)
        {
            int index = i + 1; 
            outWears[i].onClick.AddListener(() => OnButtonClick("OutWears", index));
        }

        for (int i = 0; i < pants.Length; i++)
        {
            int index = i + 1;
            pants[i].onClick.AddListener(() => OnButtonClick("Pant", index));
        }
    }
    void OnButtonClick(string itemType, int index)
    {
        Debug.Log($"Selected Item: {itemType},{index}");
    }
}
