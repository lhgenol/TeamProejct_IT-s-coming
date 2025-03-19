using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType
{
    City,
    Western,
    Toy
}
public class BackgroundController : MonoBehaviour
{
    public Material curThemeMaterial;
    private Renderer planeRenderer;

    private void Awake()
    {
        GameManager.Instance.backgroundController = this;
    }

    private void Start()
    {
        curThemeMaterial = MapManager.Instance.themeData[0].background;
        planeRenderer = GetComponent<Renderer>();
        ChangeBackground(curThemeMaterial);
    }
    /// <summary>
    /// 백그라운드 이미지 바꾸는메서드
    /// </summary>
    /// <param name="background"></param>
    public void ChangeBackground(Material background)
    {
        curThemeMaterial = background;
        planeRenderer.material = curThemeMaterial;
    }
}
