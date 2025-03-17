using UnityEngine;

public class TransformSpawner : MonoBehaviour
{
    public Transform startPoint;  // 시작 지점
    public Transform endPoint;    // 끝 지점
    public Transform parent;      // 부모 오브젝트 (선택 사항)
    public string objName = "Coin"; // 생성될 오브젝트 이름
    public GameObject prefab;     // 생성할 프리팹 (선택 사항)

    public float spacing = 2f; // 오브젝트 간격 (무조건 2 유지)

    public void SpawnObjects()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("올바른 설정을 확인하세요!");
            return;
        }

        // 시작과 끝 지점 사이 거리 계산
        Vector3 startPosition = startPoint.position;
        Vector3 endPosition = endPoint.position;
        Vector3 direction = (endPosition - startPosition).normalized;

        float totalDistance = Vector3.Distance(startPosition, endPosition);
        int count = Mathf.FloorToInt(totalDistance / spacing); // 개수 자동 계산 (2 간격 유지)

        for (int i = 0; i <= count; i++)
        {
            Vector3 spawnPosition = startPosition + direction * spacing * i;
            GameObject obj;
            obj = new GameObject(objName);
            obj.transform.position = spawnPosition;
            if (parent != null) obj.transform.SetParent(parent);

            if (prefab != null)
            {
                // 프리팹이 있을 경우 프리팹을 생성
                obj = Instantiate(prefab, spawnPosition, Quaternion.identity,obj.transform);
            }
        }

        Debug.Log($"{count}개의 오브젝트가 {spacing} 간격으로 생성되었습니다!");
    }
}