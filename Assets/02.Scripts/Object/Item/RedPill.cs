using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RedPill : Item
{
    public float jumpPower = 10f;  
    public float moveSpeed = 5f;   
    public float speedMultiplier = 1.5f;
    public float duration = 4f;  
    public float timeStep = 0.1f;
    public GameObject coin;
    private Transform chunkContainer;

    protected void Start()
    {
        chunkContainer = MapManager.Instance.chunkContainer.transform;
    }

    protected override void ApplyEffect(GameObject player)
    {
        //PlayerManager.Instance.Player.health = 2;
        MapManager.Instance.SpeedUp(speedMultiplier, duration);
        moveSpeed = MapManager.Instance.chunkContainer.moveSpeed * MapManager.Instance.chunkContainer.speedMultiplier;
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 jumpVelocity = new Vector3(0f, jumpPower, 0);
            rb.velocity = jumpVelocity;  

            ShowParabolaPath(player.transform.position);
        }
    }

    private void ShowParabolaPath(Vector3 startPosition)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        int maxSteps = Mathf.CeilToInt(duration / timeStep);
        float x = startPosition.x;

        for (int i = 0; i < maxSteps; i++)
        {
            float t = i * timeStep;
            float y = startPosition.y + (jumpPower * t) + (0.5f * -gravity * t * t);
            float z = startPosition.z + (moveSpeed * t);

            Vector3 newPosition = new Vector3(x, y, z);
            GameObject marker = new GameObject("Coin");
            marker.transform.position = newPosition;
            MapManager.Instance.itemPool.GetFromPool(coin, marker.transform, chunkContainer);

            Destroy(marker.gameObject,duration);

            if (y < startPosition.y) break; // 땅에 닿으면 종료
        }
    }
}