using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    
    [Header("Coin Value Increase Settings")]
    public int valueIncreaseAmount = 1;
    public float valueIncreaseInterval = 30f;
    
    public ParticleSystem collectEffect; 

    private TimerDisplay timerDisplay;
    private float lastValueIncreaseTime = 0f;
    private bool hasStartedValueIncrease = false;
    private bool isCollected = false;

    void Start()
    {
        timerDisplay = FindObjectOfType<TimerDisplay>();
        collectEffect = GetComponentInChildren<ParticleSystem>();
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
        if (collision.CompareTag("Player") && !isCollected)
        {
            if (collectEffect != null)
        {
            collectEffect.Play();
        }
            isCollected = true;
            PlayerStats.Instance.AddCoins(coinValue);
            StartCoroutine(CollectAnimation());
        }
    }

    private IEnumerator CollectAnimation()
    {        
        GetComponent<Collider2D>().enabled = false;

        float animationDuration = 0.3f;
        float moveHeight = 1.5f;
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            transform.position = startPosition + new Vector3(0, Mathf.Sin(t * Mathf.PI) * moveHeight, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        if(GetComponent<SpriteRenderer>() != null)
        {            
            GetComponent<SpriteRenderer>().enabled = false;
        }
        
        Destroy(gameObject);
    }
}
