using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum UIState
{
    Home,
    InGame,
    GameEnd,
    Pause,
    LeaderBoard,
    Achivement,
    Customizing,

}

public class UIManager : Singleton<UIManager>
{
    [Header("UI")]
    [SerializeField] HomeUI homeUI;
    [SerializeField] InGameUI inGameUI;
    [SerializeField] GameEndUI gameEndUI;
    [SerializeField] LeaderBoardUI leaderBoardUI;
    [SerializeField] AchievementUI AchievementUI;
    [SerializeField] CustomizingUI customizingUI;
    [SerializeField] PauseUI pauseUI;

    [Header("Canvas")]
    public GameObject MainMenuCanvas;
    public GameObject InGameCanvas;

    private UIState currentState;
    protected override void Awake()
    {
        base.Awake();
        MainMenuCanvas = transform.Find("MainMenuCanvas").gameObject;
        InGameCanvas = transform.Find("InGameCanvas").gameObject;
        homeUI = GetComponentInChildren<HomeUI>(true);
        homeUI.Init(this);
        inGameUI = GetComponentInChildren<InGameUI>(true);
        inGameUI.Init(this);
        gameEndUI = GetComponentInChildren<GameEndUI>(true);
        gameEndUI.Init(this);
        pauseUI = GetComponentInChildren<PauseUI>(true);
        pauseUI.Init(this);
        leaderBoardUI = GetComponentInChildren<LeaderBoardUI>(true);
        leaderBoardUI.Init(this);
        AchievementUI = GetComponentInChildren<AchievementUI>(true);
        AchievementUI.Init(this);
        customizingUI = GetComponentInChildren<CustomizingUI>(true);
        customizingUI.Init(this);

        ChangeState(UIState.Home);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState != UIState.Home && currentState != UIState.GameEnd && currentState != UIState.InGame)
            {
                ChangeState(UIState.Home);
            }
        }
    }
    public void GetItem(UsedItemType itemType)
    {
        inGameUI.GetItem(itemType);
    }
    public void ChangeState(UIState state)
    {
        currentState = state;
        homeUI.SetActive(currentState);
        inGameUI.SetActive(currentState);
        gameEndUI.SetActive(currentState);
        leaderBoardUI.SetActive(currentState);
        AchievementUI.SetActive(currentState);
        customizingUI.SetActive(currentState);
        pauseUI.SetActive(currentState);
    }
}
