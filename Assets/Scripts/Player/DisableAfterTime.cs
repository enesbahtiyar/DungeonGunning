using System;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    private float timeToDisable = 0.8f;
    private float timer;

    private void OnEnable()
    {
        timer = timeToDisable;
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
