using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Score { get; private set; }
    public float GameTime { get; private set; }
    public int Coin{ get; private set; }

    public int[] Rank {  get; private set; }
    protected override void Awake()
    {
        base.Awake();
        Rank = new int[] { 1000, 500, 300, 200, 50, 50, 50, 50, 50,50 };
    }

    public void StartGame()
    {
        //게임시작
    }
    public void BackToHome()
    {
        //홈메뉴로
    }
    public void CalculationScore()
    {
        //점수계산
    }
    public void PauseGame()
    {
        //일시정지
    }

    public void ManagementLeaderBorad()
    {
        //리더보드 등록
        //게임 끝나면 점수계산해서 실행
    }
}
