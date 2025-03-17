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
    public BackgroundType currentBackgroundType;

    // �� ��� ���¿� �´� Material�� ����
    public Material cityMaterial;
    public Material westernMaterial;
    public Material toyMaterial;

    private Renderer planeRenderer;

    private void Start()
    {
        planeRenderer = GetComponent<Renderer>();
        SwitchBackground(currentBackgroundType);
    }

    public void SwitchBackground(BackgroundType backgroundType)
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
    }
}
