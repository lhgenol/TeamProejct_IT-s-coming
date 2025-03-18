using UnityEngine;

public enum ItemType
{ Coin,Magnet, JumpBoost,Star,Trophy }
public enum UsedItemType 
{ Magnet,Star, Trophy,NonUse }
public class Item : MonoBehaviour
{
    public UsedItemType itemType;
    public float rotationSpeed = 100f; // 회전 속도 조절
    Item item;

    protected virtual void Awake()
    {
        item = GetComponent<Item>();
    }
    protected void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // 아이템 회전
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect(other.gameObject);
            MapManager.Instance.itemPool.ReturnToPool(item, gameObject); // 아이템을 먹으면 제거

            if (!(itemType == UsedItemType.NonUse))
            UIManager.Instance.GetItem(itemType);
        }
    }

    protected virtual void ApplyEffect(GameObject player) { }
        /*switch (itemType)
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
        }*/
    
}
