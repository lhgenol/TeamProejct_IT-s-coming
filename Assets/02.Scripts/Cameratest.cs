using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public float normalFOV = 60f;  // 일반 FOV 값
    public float curveFOV = 100f;  // 휘어지는 시점의 FOV 값
    public float transitionSpeed = 0.5f;  // 전환 속도

    private Camera cam;
    private float targetFOV;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetFOV = normalFOV;
    }

    void Update()
    {
        // 가까운 물체일수록 더 넓은 FOV를 적용
        float distance = Vector3.Distance(transform.position, Vector3.zero);  // 카메라와 맵의 중심 간의 거리
        targetFOV = Mathf.Lerp(normalFOV, curveFOV, Mathf.Clamp01(distance / 10f));  // FOV 전환

        // FOV를 부드럽게 변화시킴
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, transitionSpeed * Time.deltaTime);
    }
}