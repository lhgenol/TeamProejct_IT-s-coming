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
        camera.transform.position = new Vector3(0, 6.5f, -10f);
    }

    public static void Moveleft()
    {
        camera.GetComponent<CameraController>().StartCoroutine(camera.GetComponent<CameraController>().Move(-3.5f));
    }
    public static void MoveRight()
    {
        camera.GetComponent<CameraController>().StartCoroutine(camera.GetComponent<CameraController>().Move(3.5f));
    }

    private IEnumerator Move(float x)
    {
        float elapsedTime = 0f;
        Vector3 InitPosition = camera.transform.position;
        Vector3 targetPosition = camera.transform.position + new Vector3(x, 0, 0);
        while (elapsedTime < duration)
        {
            camera.transform.position = Vector3.Lerp(InitPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        camera.transform.position = targetPosition;
    }
}
