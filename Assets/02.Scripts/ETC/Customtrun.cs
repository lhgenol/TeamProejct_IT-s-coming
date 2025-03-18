using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customtrun : MonoBehaviour
{
    public float rotationSpeed = 100f;  // 회전 속도

    void Update()
    {
        // 매 프레임마다 y축을 기준으로 회전
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
