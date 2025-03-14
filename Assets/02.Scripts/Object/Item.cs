using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, Magnet, JumpBoost, Star , Trophy }
    public ItemType itemType;
    public float rotationSpeed = 100f; // 회전 속도 조절

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // 아이템 회전
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect(other.gameObject);
            Destroy(gameObject); // 아이템을 먹으면 제거
        }
    }

    void ApplyEffect(GameObject player)
    {
        switch (itemType)
        {
            case ItemType.Coin:
                // GameManager.Instance.AddScore(10); // 점수 증가
                break;
            case ItemType.Magnet:
                // player.GetComponent<PlayerController>().ActivateMagnet(); // 자석 효과
                break;
            case ItemType.JumpBoost:
                // player.GetComponent<PlayerController>().ActivateJumpBoost(); // 점프 부스터
                break;
            case ItemType.Star:
                // player.GetComponent<PlayerController>().ActivateStar(); //무적
                break;
            case ItemType.Trophy:
                // GameManager.Instance.ActivateScore(); // 점수 두 배
                break;
        }
    }
}
