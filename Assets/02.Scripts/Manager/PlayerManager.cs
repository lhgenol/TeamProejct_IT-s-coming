using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // 싱글톤 패턴을 사용해 게임 내에서 하나의 PlayerManager 인스턴스만 존재하도록 함
    private static PlayerManager _instance;
    
    // PlayerManager의 유일한 인스턴스를 가져오는 프로퍼티. 다른 스크립트에서 PlayerManager.Instance를 통해 접근 가능
    public static PlayerManager Instance   
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return _instance;
        }
    }

    public Player _player;  // 현재 플레이어 객체
    public Player Player    // 플레이어 객체에 접근할 수 있는 프로퍼티
    {
        get {return _player;}
        set {_player = value;}
    }

    private void Awake()
    {
        // 현재 인스턴스가 존재하지 않으면 this를 인스턴스로 설정하고 씬 전환 시 파괴되지 않도록 함
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 존재하는 경우 중복 생성을 방지하기 위해 현재 객체를 파괴함
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
