using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestrutiveObject : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
