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
    //public BackgroundType currentBackgroundType;

    // 각 배경 상태에 맞는 Material을 연결
    public Material curThemeMaterial;
    /*public Material cityMaterial;
    public Material westernMaterial;
    public Material toyMaterial;*/

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

    public void ChangeBackground(Material background)
    {
        curThemeMaterial = background;
        planeRenderer.material = curThemeMaterial;
    }

    /*public void SwitchBackground(BackgroundType backgroundType)
    {
        switch (backgroundType)
        {
            case BackgroundType.City:
                planeRenderer.material = cityMaterial;
                break;

            case BackgroundType.Western:
                planeRenderer.material = westernMaterial;
                break;

            case BackgroundType.Toy:
                planeRenderer.material = toyMaterial;
                break;

            default:
                planeRenderer.material = cityMaterial;
                break;
        }
    }
    public void ChangeBackground(BackgroundType newBackgroundType)
    {
        currentBackgroundType = newBackgroundType;
        SwitchBackground(newBackgroundType);
    }*/
}
