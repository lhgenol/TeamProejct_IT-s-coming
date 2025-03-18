using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : BaseUI
{
    [Header("score")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI coin;

    [Header("Item")]
    public ItemSlot[] itemSlot;
    [SerializeField] private Sprite[] iconlist;

    [System.Serializable]
    public struct ItemSlot
    {
        public bool used;
        public GameObject item;
        public Image icon;
        public Image gauge;
        public UsedItemType type;
        public int slotNum;

        public ItemSlot(bool used, GameObject item, Image icon, Image gauge, UsedItemType type, int slotNum)
        {
            this.used = used;
            this.item = item;
            this.icon = icon;
            this.gauge = gauge;
            this.type = type;
            this.slotNum = slotNum;
        }
    }

    private int lastCoin = 0;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }

    private void OnEnable()
    {
        if (!GameManager.Instance.Started)
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

    public void GetItem(UsedItemType itemType)
    {
        int slotNum = GetUsedSlot(itemType);

        if (slotNum != -1)
        {
            ShowGauge(itemType, slotNum);
        }
        else
        {
            slotNum = CanUse();
            if (slotNum != -1)
            {
                itemSlot[slotNum].item.gameObject.SetActive(true);
                itemSlot[slotNum].type = itemType;
                ShowGauge(itemType, slotNum);
            }
        }
    }

    private int GetUsedSlot(UsedItemType itemType)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].used && itemSlot[i].type == itemType)
            {
                return itemSlot[i].slotNum;
            }
        }
        return -1;
    }

    public int CanUse()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].used)
            {
                return i;
            }
        }
        return -1;
    }

    public void ShowGauge(UsedItemType type, int slotNum)
    {
        itemSlot[slotNum].used = true;
        itemSlot[slotNum].gauge.fillAmount = 1f;
        itemSlot[slotNum].icon.sprite = iconlist[(int)type];

        float duration = 5f;
        itemSlot[slotNum].gauge.color = Color.green;

        itemSlot[slotNum].gauge.DOFillAmount(0f, duration)
                    .OnKill(() =>
                    {
                        itemSlot[slotNum].item.gameObject.SetActive(false);
                        itemSlot[slotNum].used = false;  
                    });

        itemSlot[slotNum].gauge.DOColor(Color.red, duration).SetEase(Ease.Linear)
            .OnKill(() => itemSlot[slotNum].gauge.color = Color.red);
    }

    protected override UIState GetUIState()
    {
        return UIState.InGame;
    }
}
