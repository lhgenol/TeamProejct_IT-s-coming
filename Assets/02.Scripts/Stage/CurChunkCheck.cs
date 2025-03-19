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



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chunk"))
        {
            curChunk = other.gameObject;

            if (curTheme == null)
            {
                curTheme = curChunk.GetComponent<Chunk>().themeData;
                newTheme = curTheme;
                SoundManager.Instance.SetBGM(curTheme.BGM);
            }
            else
            {
                newTheme = curChunk.GetComponent<Chunk>().themeData;
                if (curTheme != newTheme)
                {
                    Achievements.TriggerFirstRoundClear();
                    curTheme = newTheme;
                    SoundManager.Instance.SetBGM(curTheme.BGM);
                    SoundManager.Instance.PlayBGM();
                    GameManager.Instance.backgroundController.ChangeBackground(curTheme.background);
                    Debug.Log("theme changed");
                }
            }
        }

    }
}

