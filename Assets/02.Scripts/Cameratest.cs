using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public float normalFOV = 60f;  // �Ϲ� FOV ��
    public float curveFOV = 100f;  // �־����� ������ FOV ��
    public float transitionSpeed = 0.5f;  // ��ȯ �ӵ�

    private Camera cam;
    private float targetFOV;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetFOV = normalFOV;
    }

    void Update()
    {
        // ����� ��ü�ϼ��� �� ���� FOV�� ����
        float distance = Vector3.Distance(transform.position, Vector3.zero);  // ī�޶�� ���� �߽� ���� �Ÿ�
        targetFOV = Mathf.Lerp(normalFOV, curveFOV, Mathf.Clamp01(distance / 10f));  // FOV ��ȯ

        // FOV�� �ε巴�� ��ȭ��Ŵ
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, transitionSpeed * Time.deltaTime);
    }
}