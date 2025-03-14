using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Score { get; private set; }
    public int Coin{ get; private set; }

    public int[] Rank {  get; private set; }

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
                lastUpdateTime = Time.time;  // 마지막 업데이트 시간 갱신
                Score += 1;  // 점수 1 증가
            }
        }
    }
    public void StartGame()
    {
        nowPlaying = true;
        Score = 0;
        Coin = 0;
    }
    public void GetCoin()
    {
        Score += 10;
        Coin += 1;
    }
   

    public void EndGame()
    {
        nowPlaying=false;
        UIManager.Instance.ChangeState(UIState.GameEnd);
    }

    public void PauseGame()
    {
        nowPlaying = false;
        //일시정지
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
