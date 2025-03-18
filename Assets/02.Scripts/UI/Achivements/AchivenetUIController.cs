using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIController : MonoBehaviour
{
	// UI 요소들
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

	// 도전과제 데이터 매핑
	private Dictionary<string, AchievementData> achievements = new Dictionary<string, AchievementData>();

	private void Start()
	{
		// 도전과제 데이터 등록
		achievements.Add("TenCoin", new AchievementData { name = "코인10개!", description = "코인을 10개모았습니다.", image = iconlist[0] });
		achievements.Add("Ailve", new AchievementData { name = "30초 생존!", description = "생존의 달인입니다.", image = iconlist[1] });
		achievements.Add("RoundClear", new AchievementData { name = "라운드클리어!", description = "처음으로 라운드를 클리어했습니다.", image = iconlist[2] });
		achievements.Add("Rank", new AchievementData { name = "첫 등수!", description = "처음으로 랭크에 올랐습니다.", image = iconlist[3] });
		achievements.Add("Costomizing", new AchievementData { name = "첫 커스터마이징!", description = "처음으로 캐릭터를 꾸였습니다..", image = iconlist[4] });
	}

	/// <summary>
	/// 패널에 데이터를 업데이트하는 메서드
	/// </summary>
	/// <param name="achievementKey"></param>딕셔너리의 키값
	/// <param name="index"></param>
	private void UpdateUIData(string achievementKey, int index)
	{
		AchievementData achievement = achievements[achievementKey];
		achievementPanelName[index].text = achievement.name;
		achievementPanelDiscription[index].text = achievement.description;
		achievementPanelImage[index].sprite = achievement.image;
	}

	/// <summary>
	/// 이벤트 등록
	/// </summary>
	private void OnEnable()
	{
		// 이벤트 등록
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
	/// 이벤트 해제
	/// </summary>
	private void OnDisable()
	{
        // 이벤트 해제
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
	/// 도전과제 패널을 업데이트하는 메서드
	/// </summary>
	/// <param name = "achievementKey" ></ param >
	private void UpdatePannel(string achievementKey)
	{
		Debug.Log(achievementKey);
		if (!achievements.ContainsKey(achievementKey))
		{
			Debug.LogWarning($"도전과제 데이터가 존재하지 않습니다: {achievementKey}");
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
	/// 도전과제 팝업UI를 표시하는 메서드
	/// </summary>
	private void ShowAchievement(string achievementKey)
	{
		if (!achievements.ContainsKey(achievementKey))
		{
			Debug.LogWarning($"도전과제 데이터가 존재하지 않습니다: {achievementKey}");
			return;
		}

		AchievementData achievement = achievements[achievementKey];

		// UI 요소 업데이트
		achievementNameText.text = achievement.name;
		achievementDescriptionText.text = achievement.description;
		achievementImage.sprite = achievement.image;

		// UI 애니메이션 실행


		float targetX = achievementUI.transform.localPosition.x - 300f;
		float originalX = achievementUI.transform.localPosition.x;

		Sequence sequence = DOTween.Sequence();
		sequence.Append(achievementUI.transform.DOLocalMoveX(targetX, 0.5f).SetEase(Ease.OutCubic)) 
				.AppendInterval(1f) // 1초 대기
				.Append(achievementUI.transform.DOLocalMoveX(originalX, 0.5f).SetEase(Ease.InCubic)); // 다시 원래 위치로 이동
	}

}