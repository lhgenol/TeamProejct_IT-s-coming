using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Item
{
    public float magnetRadius = 5f;  // 감지 반경
    public float magnetSpeed = 10f;  // 코인이 빨려오는 속도
    public float magnetDuration = 5f; // 마그넷 지속 시간
    private Transform player;
    

    protected override void ApplyEffect(GameObject playerObj)
    {
        this.player = playerObj.transform;
        ItemManager.Instance.StartExternalCoroutine(MagnetEffect());
    }

    public IEnumerator MagnetEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < magnetDuration)
        {
            Collider[] colliders = Physics.OverlapSphere(player.position, magnetRadius);
            List<Coin> nearbyCoins = new List<Coin>();

            foreach (Collider collider in colliders)
            {
                Coin coin = collider.GetComponent<Coin>(); // Coin 컴포넌트가 있는지 확인
                if (coin != null)
                {
                    nearbyCoins.Add(coin);
                }
            }

            foreach (var coin in nearbyCoins)
            {

                ItemManager.Instance.StartExternalCoroutine(MoveCoinToPlayer(coin.gameObject));

            }

            elapsedTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("MagnetEffect finished");
    }

    private IEnumerator MoveCoinToPlayer(GameObject coin)
    {
        while (coin != null && coin.activeSelf)
        {
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, player.position, magnetSpeed * Time.deltaTime);
            Vector3 distance = player.position - coin.transform.position;
            yield return null;
        }

        yield break;
    }
}
