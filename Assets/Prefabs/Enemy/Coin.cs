using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    
    [Header("Coin Value Increase Settings")]
    public int valueIncreaseAmount = 1;
    public float valueIncreaseInterval = 30f;
    
    public AudioSource audioSource;
    private TimerDisplay timerDisplay;
    private float lastValueIncreaseTime = 0f;
    private bool hasStartedValueIncrease = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timerDisplay = FindObjectOfType<TimerDisplay>();
    }
    
    void Update()
    {
        if (timerDisplay != null)
        {
            float currentTime = timerDisplay.GetElapsedTime();
            
            if (!hasStartedValueIncrease && currentTime > 0)
            {
                hasStartedValueIncrease = true;
                lastValueIncreaseTime = currentTime;
            }
            
            if (hasStartedValueIncrease && currentTime - lastValueIncreaseTime >= valueIncreaseInterval)
            {
                coinValue += valueIncreaseAmount;
                lastValueIncreaseTime = currentTime;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats.Instance.AddCoins(coinValue);
            if (audioSource != null)
            {
                audioSource.Play();
                StartCoroutine(DestroyAfterSound());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}