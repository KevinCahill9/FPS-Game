// Note: This script may include code or patterns modified from Unity tutorials.
// It has been modified and extended to suit the requirments of the project.
// Source: https://www.youtube.com/playlist?list=PLtLToKUhgzwm1rZnTeWSRAyx9tl8VbGUE

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
