using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private int seconds = 0;
    private int minutes = 0;

    void Update()
    {
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
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
