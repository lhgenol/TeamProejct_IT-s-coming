using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurChunkCheck : MonoBehaviour
{
    public ThemeDataSO curTheme;
    public ThemeDataSO newTheme;
    public GameObject curChunk;

    private void Awake()
    {
        MapManager.Instance.CurChunkCheck = this;
    }

    private void Update()
    {
        if (GameManager.Instance.NowPlaying)
        {
            if (curTheme != newTheme)
            {
                curTheme = newTheme;
                SoundManager.Instance.ChangeBgm(curTheme.BGM);//오디오 재생
                Debug.Log("bgm changed");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chunk"))
        {
            curChunk = other.gameObject;
            if (curTheme == null)
            {
                curTheme = curChunk.GetComponent<Chunk>().themeData;
                newTheme = curTheme;
            }
            else newTheme = curChunk.GetComponent<Chunk>().themeData;
        }
    }


}
