using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIController : MonoBehaviour
{
	// UI ��ҵ�
	[Header("HUD")]
	[SerializeField] private RectTransform achievementUI;
	[SerializeField] private TextMeshProUGUI achievementNameText;
	[SerializeField] private TextMeshProUGUI achievementDescriptionText;
	[SerializeField] private Image achievementImage;
	[SerializeField] private Sprite[] iconlist;

	[Header("AchivementUI")]
	[SerializeField] private TextMeshProUGUI[] achievementPanelName;
	[SerializeField] private TextMeshProUGUI[] achievementPanelDiscription;
	[SerializeField] private Image[] achievementPanelImage;
	public class AchievementData
	{
		public string name;
		public string description;
		public Sprite image;
	}

	// �������� ������ ����
	private Dictionary<string, AchievementData> achievements = new Dictionary<string, AchievementData>();

	private void Start()
	{
		// �������� ������ ���
		achievements.Add("TenCoin", new AchievementData { name = "����10��!", description = "������ 10����ҽ��ϴ�.", image = iconlist[0] });
		achievements.Add("Ailve", new AchievementData { name = "30�� ����!", description = "������ �����Դϴ�.", image = iconlist[1] });
		achievements.Add("RoundClear", new AchievementData { name = "����Ŭ����!", description = "ó������ ���带 Ŭ�����߽��ϴ�.", image = iconlist[2] });
		achievements.Add("Rank", new AchievementData { name = "ù ���!", description = "ó������ ��ũ�� �ö����ϴ�.", image = iconlist[3] });
		achievements.Add("Costomizing", new AchievementData { name = "ù Ŀ���͸���¡!", description = "ó������ ĳ���͸� �ٿ����ϴ�..", image = iconlist[4] });
	}

	/// <summary>
	/// �гο� �����͸� ������Ʈ�ϴ� �޼���
	/// </summary>
	/// <param name="achievementKey"></param>��ųʸ��� Ű��
	/// <param name="index"></param>
	private void UpdateUIData(string achievementKey, int index)
	{
		AchievementData achievement = achievements[achievementKey];
		achievementPanelName[index].text = achievement.name;
		achievementPanelDiscription[index].text = achievement.description;
		achievementPanelImage[index].sprite = achievement.image;
	}

	/// <summary>
	/// �̺�Ʈ ���
	/// </summary>
	private void OnEnable()
	{
		// �̺�Ʈ ���
		Achievements.OnFirstCoin += () => ShowAchievement("TenCoin");
        Achievements.OnFirstCoin += () => UpdatePannel("TenCoin");
        Achievements.OnFirstThirtySecond += () => ShowAchievement("Ailve");
        Achievements.OnFirstThirtySecond += () => UpdatePannel("Ailve");
        Achievements.OnFristRoundClear += () => ShowAchievement("RoundClear");
        Achievements.OnFristRoundClear += () => UpdatePannel("RoundClear");
        Achievements.OnFirstRank += () => ShowAchievement("Rank");
        Achievements.OnFirstRank += () => UpdatePannel("Rank");
        Achievements.OnFirstCostomizing += () => ShowAchievement("Costomizing");
        Achievements.OnFirstCostomizing += () => UpdatePannel("Costomizing");

    }

	/// <summary>
	/// �̺�Ʈ ����
	/// </summary>
	private void OnDisable()
	{
        // �̺�Ʈ ����
        Achievements.OnFirstCoin -= () => ShowAchievement("TenCoin");
        Achievements.OnFirstCoin -= () => UpdatePannel("TenCoin");
        Achievements.OnFirstThirtySecond -= () => ShowAchievement("Ailve");
        Achievements.OnFirstThirtySecond -= () => UpdatePannel("Ailve");
        Achievements.OnFristRoundClear -= () => ShowAchievement("RoundClear");
        Achievements.OnFristRoundClear -= () => UpdatePannel("RoundClear");
        Achievements.OnFirstRank -= () => ShowAchievement("Rank");
        Achievements.OnFirstRank -= () => UpdatePannel("Rank");
        Achievements.OnFirstCostomizing -= () => ShowAchievement("Costomizing");
        Achievements.OnFirstCostomizing -= () => UpdatePannel("Costomizing");
    }
	/// <summary>
	/// �������� �г��� ������Ʈ�ϴ� �޼���
	/// </summary>
	/// <param name = "achievementKey" ></ param >
	private void UpdatePannel(string achievementKey)
	{
		Debug.Log(achievementKey);
		if (!achievements.ContainsKey(achievementKey))
		{
			Debug.LogWarning($"�������� �����Ͱ� �������� �ʽ��ϴ�: {achievementKey}");
			return;
		}
		switch (achievementKey)
		{
			case "TenCoin":
				UpdateUIData(achievementKey, 0);
				break;
			case "Alive":
				UpdateUIData(achievementKey, 1);
				break;
			case "RoundClear":
				UpdateUIData(achievementKey, 2);
				break;
			case "Rank":
				UpdateUIData(achievementKey, 3);
				break;
			case "Costomizing":
				UpdateUIData(achievementKey, 4);
				break;
		}
	}

	/// <summary>
	/// �������� �˾�UI�� ǥ���ϴ� �޼���
	/// </summary>
	private void ShowAchievement(string achievementKey)
	{
		if (!achievements.ContainsKey(achievementKey))
		{
			Debug.LogWarning($"�������� �����Ͱ� �������� �ʽ��ϴ�: {achievementKey}");
			return;
		}

		AchievementData achievement = achievements[achievementKey];

		// UI ��� ������Ʈ
		achievementNameText.text = achievement.name;
		achievementDescriptionText.text = achievement.description;
		achievementImage.sprite = achievement.image;

		// UI �ִϸ��̼� ����


		float targetX = achievementUI.transform.localPosition.x - 300f;
		float originalX = achievementUI.transform.localPosition.x;

		Sequence sequence = DOTween.Sequence();
		sequence.Append(achievementUI.transform.DOLocalMoveX(targetX, 0.5f).SetEase(Ease.OutCubic)) 
				.AppendInterval(1f) // 1�� ���
				.Append(achievementUI.transform.DOLocalMoveX(originalX, 0.5f).SetEase(Ease.InCubic)); // �ٽ� ���� ��ġ�� �̵�
	}

}