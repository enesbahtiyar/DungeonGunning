using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownHandler : MonoBehaviour
{
    public void StartCoolDown(Image image,float duration,Action onComplete)
    {
        StartCoroutine(CooldownRoutine(image,duration,onComplete));
    }
    private IEnumerator CooldownRoutine(Image image,float duration,Action onComplete)
    {
        if (duration <= 0f)
        {
            yield break;
        }
        image.fillAmount = 1f;
        float timer = duration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            image.fillAmount = t;
            yield return null;
        }
        image.fillAmount = 0f;
        onComplete?.Invoke();
    }
}
