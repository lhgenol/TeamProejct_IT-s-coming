using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public static Camera camera;
    private float duration = 0.5f;
    private void Awake()
    {
        camera = Camera.main;
    }
    public static void Init()
    {
        camera.transform.position = new Vector3(0, 6.5f, -13f);
    }

    public static void CameraMove(Vector3 targetPos)
    {
        camera.GetComponent<CameraController>().StartCoroutine(camera.GetComponent<CameraController>().Move(targetPos));
    }

    /// <summary>
    /// 플레이어가 움직일때 따라가주는 메서드
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    private IEnumerator Move(Vector3 targetPos)
    {
        float elapsedTime = 0f;
        Vector3 initPosition = camera.transform.position;
        Vector3 targetPositon = new Vector3(targetPos.x, initPosition.y, initPosition.z);
        while (elapsedTime < duration)
        {
            camera.transform.position = Vector3.Lerp(initPosition, targetPositon, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        camera.transform.position = targetPositon;
    }
}
