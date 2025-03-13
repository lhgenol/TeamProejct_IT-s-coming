using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEditor.Progress;

public enum UIState
{
    Home,
    GameEnd,
    LeaderBoard,
    Option,
    Customizing,
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField] HomeUI homeUI;
    [SerializeField] GameEndUI gameEndUI;
    [SerializeField] LeaderBoardUI leaderBoardUI;
    [SerializeField] OptionUI optionUI;
    [SerializeField] CustomizingUI customizingUI;
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
        gameEndUI = GetComponentInChildren<GameEndUI>(true);
        gameEndUI.Init(this);
        leaderBoardUI = GetComponentInChildren<LeaderBoardUI>(true);
        leaderBoardUI.Init(this);
        optionUI = GetComponentInChildren<OptionUI>(true);
        optionUI.Init(this);
        customizingUI = GetComponentInChildren<CustomizingUI>(true);
        customizingUI.Init(this);

        ChangeState(UIState.Home);
        ChangeCanvas();
    }

    public void SetHome()
    {
        ChangeState(UIState.Home);
    }
    public void SetGameEnd()
    {
        ChangeState(UIState.GameEnd);
    }
    public void SetRanking()
    {
        ChangeState(UIState.LeaderBoard);
    }

    public void SetOptionUI()
    {
        ChangeState(UIState.Option);
    }

    public void SetCustomizing()
    {
        ChangeState(UIState.Customizing);
    }

    public void ChangeState(UIState state)
    {
        currentState = state;
        homeUI.SetActive(currentState);
        gameEndUI.SetActive(currentState);
        leaderBoardUI.SetActive(currentState);
        optionUI.SetActive(currentState);
        customizingUI.SetActive(currentState);
    }
    public void ChangeCanvas()
    {
        if (currentState == UIState.Home || currentState == UIState.LeaderBoard || currentState == UIState.Option || currentState == UIState.Customizing)
        {
            MainMenuCanvas.SetActive(true);
            InGameCanvas.SetActive(false);
        }
        else if (currentState == UIState.GameEnd)
        {
            MainMenuCanvas.SetActive(true);
            InGameCanvas.SetActive(false);
        }
    }
}
