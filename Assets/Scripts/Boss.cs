using UnityEngine;

public class Boss : MonoBehaviour
{
    private TimerDisplay timerDisplay;
    private void OnEnable()
    {
        timerDisplay = FindFirstObjectByType<TimerDisplay>();
        timerDisplay.StopTimer();
    }
    private void OnDisable()
    {
        timerDisplay.ResumeTimer();
        FindFirstObjectByType<EnemySpawner>().BossIsDead();
    }
}


