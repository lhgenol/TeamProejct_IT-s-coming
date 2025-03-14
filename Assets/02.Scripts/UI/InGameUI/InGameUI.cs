using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class InGameUI : BaseUI
{
    [Header("score")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI coin;

    [Header("Item")]
    public GameObject[] items;
    public Image[] gauge;

    private int lastCoin=0;
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    private void OnEnable()
    {
        if(!GameManager.Instance.Started)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            GameManager.Instance.Resume();
        }
    }
    public void Update()
    {
        score.text = GameManager.Instance.Score.ToString();
        if (GameManager.Instance.Coin != lastCoin)
        {
            coin.text = GameManager.Instance.Coin.ToString();
            lastCoin = GameManager.Instance.Coin;
        }
    }

    public void GetItem(int sequence)
    {
        items[sequence].gameObject.SetActive(true);
        ShowGauge(sequence);
    }
    public void ShowGauge(int sequence)
    {
        gauge[sequence].fillAmount = 1f;
        float duration = 5f;
        gauge[sequence].color = Color.green;
        gauge[sequence].DOFillAmount(0f, duration)  
            .OnKill(() => items[sequence].gameObject.SetActive(false));

        gauge[sequence].DOColor(Color.red, duration).SetEase(Ease.Linear)
            .OnKill(() => gauge[sequence].color = Color.red);

    }
    protected override UIState GetUIState()
    {
        return UIState.InGame;
    }
}