using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    
    [Header("Coin Value Increase Settings")]
    public int valueIncreaseAmount = 1;
    public float valueIncreaseInterval = 30f;
    
    private TimerDisplay timerDisplay;
    private float lastValueIncreaseTime = 0f;
    private bool hasStartedValueIncrease = false;

    void Start()
    {
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

            Destroy(gameObject);
        }
    }
}