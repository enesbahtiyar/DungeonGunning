using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int maxSeconds = 120; 

    private float elapsedTime = 0f;
    private int seconds = 0;
    private int minutes = 0;
    private bool maxTimeReached = false;

    void Update()
    {
        if (maxTimeReached) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f)
        {
            elapsedTime -= 1f;
            seconds++;

            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
            }

            UpdateTimerUI();

            int totalElapsedSeconds = minutes * 60 + seconds;
            if (totalElapsedSeconds >= maxSeconds)
            {
                maxTimeReached = true;
                Debug.Log("Maksimum süre !");
            }
        }
    }

    void UpdateTimerUI()
    {
        int maxMinutes = maxSeconds / 60;
        int maxSecondsRemainder = maxSeconds % 60;
        string maxTimeStr = string.Format("{0:00}:{1:00}", maxMinutes, maxSecondsRemainder);
        
        if (maxTimeReached)
        {
            timerText.text = maxTimeStr + " / " + maxTimeStr;
        }
        else
        {
            string currentTimeStr = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = currentTimeStr + " / " + maxTimeStr;
        }
    }

    public bool IsMaxTimeReached()
    {
        return maxTimeReached;
    }

    public float GetElapsedTime()
    {
        return minutes * 60 + seconds;
    }
}
