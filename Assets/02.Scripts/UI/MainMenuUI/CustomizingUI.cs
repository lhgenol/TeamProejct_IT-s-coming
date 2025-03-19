using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingUI : BaseUI
{
    public Button[] hats;
    public Button[] outWears;
    public Button[] pants;
    public Customizing manikin;
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

    /// <summary>
    /// 버튼을 할당하여 버튼 클릭시 Costomizing에
    /// 키값과 인덱스번호를 전달
    /// </summary>
    void SetUpButtonListeners()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i; 
            hats[i].onClick.AddListener(() => OnButtonClick(Parts.Hat, index));
        }

        for (int i = 0; i < outWears.Length; i++)
        {
            int index = i; 
            outWears[i].onClick.AddListener(() => OnButtonClick(Parts.OutWear, index));
        }

        for (int i = 0; i < pants.Length; i++)
        {
            int index = i;
            pants[i].onClick.AddListener(() => OnButtonClick(Parts.Pants, index));
        }
    }
    void OnButtonClick(Parts part, int index)
    {
        Achievements.TriggerFirstCostomizing();
        PlayerManager.Instance.customizing.ChangeMesh(part, index);
        manikin.ChangeMesh(part, index);
    }
}
