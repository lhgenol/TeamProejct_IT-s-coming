using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUIController : MonoBehaviour
{
    public GameObject volumeUI;
    public float duration = 0.5f;
    private bool isAnimating = false;
    private bool isVolumeOpen = false;

    /// <summary>
    /// 볼륨이미지를 누르면 볼륨ui팝업
    /// </summary>
    public void OnClickVolume()
    {
        if (isAnimating)
            return;
        if (isVolumeOpen)
        {
            isVolumeOpen = false;
            CloseVolumUI();
        }
        else
        {
            isVolumeOpen=true;
            OpenVolumeUI();
        }
    }
    public void OpenVolumeUI()
    {
        MovingUI(volumeUI,-300, 0);
    }
    public void CloseVolumUI()
    {
        MovingUI(volumeUI,300, 0);
    }

    /// <summary>
    /// 볼륨UI가 움직인다.
    /// DOTWEEN이용
    /// </summary>
    /// <param name="gameobject"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void MovingUI(GameObject gameobject,float x ,float  y)
    {

        isAnimating = true;
        RectTransform rect = gameobject.GetComponent<RectTransform>();
        Vector2 targetPos = rect.anchoredPosition + new Vector2(x, y);
        rect.DOAnchorPos(targetPos, duration)
            .SetEase(Ease.OutCubic).OnComplete(() =>
            {
                isAnimating = false;
            });
    }
}
