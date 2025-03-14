﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public int Score { get; private set; }
    [HideInInspector]
    public int Coin{ get; private set; }
    [HideInInspector]
    public int[] Rank { get; private set; }

    [HideInInspector]
    public bool Started { get; private set; }
    [HideInInspector]
    public bool NewRank { get; private set; }
    [HideInInspector]
    public int RankIndex { get; private set; }
    private bool nowPlaying;
 
    private float lastUpdateTime = 0;
    protected override void Awake()
    {
        base.Awake();
        Rank = new int[] { 1000, 500, 300, 200, 50, 50, 50, 50, 50,50 };
    }
    private void Update()
    {
        if (nowPlaying)
        {
            if (Time.time - lastUpdateTime >= 0.1f)
            {
                lastUpdateTime = Time.time;
                Score += 1;
            }
        }
    }
    public void StartGame()
    {
        Started = true;
        nowPlaying = true;
        Score = 0;
        Coin = 0;
    }
    public void GetCoin()
    {
        Score += 10;
        Coin += 1;
    }

    public void StopGame()
    {
        Started = false;
        nowPlaying = false;
    }

    public void EndGame()
    {
        StopGame();
        if (Score > Rank[9])
        {
            NewRank = true;
            Rank[9] = Score;
            Array.Sort(Rank);
            Array.Reverse(Rank);
            RankIndex = 1;
            foreach (var rank in Rank)
            {
                if (rank == Score) break;
                RankIndex++;
            }
        }
        UIManager.Instance.ChangeState(UIState.GameEnd);
    }

    public void PauseGame()
    {
        nowPlaying = false;
        //일시정지
    }

    public void Resume()
    {
        nowPlaying = true;
    }

    public void ManagementLeaderBorad()
    {
        //리더보드 등록
        //게임 끝나면 점수계산해서 실행
    }

    /// ui매니저에서 관리중 끝까지 필요없으면 삭제
    //public void BackToHome()
    //{
    //    //홈메뉴로
    //}

    ///업데이트에서 스코어 계산중 끝까지 필요없으면 삭제
    //public void CalculationScore()
    //{
    //    //점수계산
    //}
}
