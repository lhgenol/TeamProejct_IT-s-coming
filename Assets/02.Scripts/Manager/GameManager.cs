using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public int Score { get; private set; }
    [HideInInspector]
    public int Coin{ get; set; }
    [HideInInspector]
    public int[] Rank { get; private set; }

    [HideInInspector]
    public bool Started { get; private set; }
    [HideInInspector]
    public bool NewRank { get; private set; }
    [HideInInspector]
    public int RankIndex { get; private set; }
    public BackgroundController backgroundController;
    public bool NowPlaying { get; private set; }
 
    private float lastUpdateTime = 0;
    
    public Chaser chaser;
    
    protected override void Awake()
    {
        base.Awake();
        Rank = new int[] { 1000, 500, 300, 200, 50, 50, 50, 50, 50,50 };
    }
    private void Start()
    {
        Screen.SetResolution(380, 640, false);
    }
    private void Update()
    {
        if (NowPlaying)
        {
            if (Time.time - lastUpdateTime >= 0.1f)
            {
                lastUpdateTime = Time.time;
                Score += 1;
            }
        }
    }

    /// <summary>
    /// 게임시작을 누를때 초기화해주는 메서드
    /// 매니저들에게 초기화 메서드를 받아와 실행
    /// </summary>
    public void StartGame()
    {
        Started = true;
        NowPlaying = true;
        Score = 0;
        Coin = 0;
        PlayerManager.Instance.Player.health = 2;
        PlayerManager.Instance.controller.Init();
        SoundManager.Instance.PlayBGM();
        MapManager.Instance.ResetChunks();
        MapManager.Instance.chunkContainer.ResumeMovement();
        chaser.Init();
    }
    public void GetCoin()
    {
        Score += 10;
        Coin += 1;
        if(Coin>10)
        {
            Achievements.TriggerFirstTenCoin();
        }
    }
    /// <summary>
    /// 게임을 멈추어주는 메서드
    /// </summary>
    public void StopGame()
    {
        Started = false;
        NowPlaying = false;
        CameraController.Init();
        MapManager.Instance.chunkContainer.PauseMovement();
    }
    /// <summary>
    /// 게임 종료시 호출되는 메서드
    /// 리더보드도 같이 호출해서 랭킹 갱신
    /// </summary>
    public void EndGame()
    {
        StopGame();
        ManagementLeaderBorad();
        UIManager.Instance.ChangeState(UIState.GameEnd);
    }

    /// <summary>
    /// 게임을 일시정지 해주는 메서드
    /// </summary>
    public void PauseGame()
    {
        NowPlaying = false;
        MapManager.Instance.chunkContainer.PauseMovement();
    }
    /// <summary>
    /// 일시정지한 게임을 재시작해주는 메서드
    /// </summary>
    public void Resume()
    {
        NowPlaying = true;
        MapManager.Instance.chunkContainer.ResumeMovement();
    }
    /// <summary>
    /// 현재 랭킹 10등과 비교하여 랭크에 등재해주는 메서드
    /// 랭킹에 넣은이후 올림차순을 해준다.
    /// </summary>
    public void ManagementLeaderBorad()
    {
        if (Score > Rank[9])
        {
            Achievements.TriggerFirstRank();
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
    }
}
