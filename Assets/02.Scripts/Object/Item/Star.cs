using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Item
{
    public float speedMultiPlier = 2f;
    public float duration = 10f; 
    public Renderer playerRenderer;
    public Color emissionColor = Color.white; 
    public float blinkDuration = 0.1f; 


    protected override void ApplyEffect(GameObject player) 
    {
        playerRenderer = player.GetComponentInChildren<Renderer>();
        MapManager.Instance.SpeedUp(speedMultiPlier, duration);
        //PlayerManager.Instance.Player.health = 2;
        ItemManager.Instance.StartExternalCoroutine(Invincible());
        ItemManager.Instance.StartExternalCoroutine(BlinkEmissionEffect());
    }

    public IEnumerator Invincible()
    {
        float time = 0f;
        //PlayerManager.Instance.Player.controller.isInvincible = true;
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        //PlayerManager.Instance.Player.controller.isInvincible = false;
    }

    private IEnumerator BlinkEmissionEffect()
    {
        Material mat = playerRenderer.material;
        Color originalEmission = mat.GetColor("_EmissionColor"); // 원래 Emission 색상 저장
        mat.EnableKeyword("_EMISSION"); // Emission 활성화
        float time = 0f;

        while (time < duration)
        {
            mat.SetColor("_EmissionColor", emissionColor * 2f); // 빛나는 효과 강화
            yield return new WaitForSeconds(blinkDuration);
            mat.SetColor("_EmissionColor", originalEmission); // 원래 색상으로 복귀
            yield return new WaitForSeconds(blinkDuration);
            time += Time.deltaTime;
        }
    }
}
