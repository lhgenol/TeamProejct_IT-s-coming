using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, Magnet, JumpBoost, Star , Trophy }
    public ItemType itemType;
    public float rotationSpeed = 100f; // ȸ�� �ӵ� ����

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // ������ ȸ��
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect(other.gameObject);
            Destroy(gameObject); // �������� ������ ����
        }
    }

    void ApplyEffect(GameObject player)
    {
        switch (itemType)
        {
            case ItemType.Coin:
                // GameManager.Instance.AddScore(10); // ���� ����
                break;
            case ItemType.Magnet:
                // player.GetComponent<PlayerController>().ActivateMagnet(); // �ڼ� ȿ��
                break;
            case ItemType.JumpBoost:
                // player.GetComponent<PlayerController>().ActivateJumpBoost(); // ���� �ν���
                break;
            case ItemType.Star:
                // player.GetComponent<PlayerController>().ActivateStar(); //����
                break;
            case ItemType.Trophy:
                // GameManager.Instance.ActivateScore(); // ���� �� ��
                break;
        }
    }
}
