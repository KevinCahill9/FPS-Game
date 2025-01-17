using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelfDestroy : MonoBehaviour
{
    public float timeUntilDestory;
    void Start()
    {
        StartCoroutine(DestroySelf(timeUntilDestory));
    }

    private IEnumerator DestroySelf(float timeUntilDestroy)
    {
        yield return new WaitForSeconds(timeUntilDestroy);

        Destroy(gameObject);
    }

    
}
