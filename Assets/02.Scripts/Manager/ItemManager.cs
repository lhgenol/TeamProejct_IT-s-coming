using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public int coinMultiplier = 1;
    public void StartExternalCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
