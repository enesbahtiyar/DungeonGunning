using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int maxMinutes = 2; 
    [SerializeField] private int maxSeconds = 0; 

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

            if (minutes > maxMinutes || (minutes == maxMinutes && seconds >= maxSeconds))
            {
                maxTimeReached = true;
                Debug.Log("Maksimum süre !");
            }
        }
    }

    void UpdateTimerUI()
    {
                 //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);   ibrahim eski timertext. dilerseniz silebilirsiniz

        string maxTimeStr = string.Format("{0:00}:{1:00}", maxMinutes, maxSeconds);
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
