using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    new static Camera camera=Camera.main;

    public static void Moveleft()
    {
        camera.gameObject.transform.position += new Vector3(-3.5f, 0, 0);
    }
    public static void MoveRight()
    {
        camera.gameObject.transform.position += new Vector3(3.5f, 0, 0);
    }
}
