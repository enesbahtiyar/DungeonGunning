using System;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private GameState currentGameState;

    public event Action<GameState> OnGameStateChanged;
    private void Start()
    {
        currentGameState = GameState.Playing;
        OnGameStateChanged?.Invoke(currentGameState);
        Time.timeScale = 1f; // Ensure game starts at normal speed
    }
    
    public void SetState(GameState newGameState)
    {
        if (currentGameState == newGameState) return;

        currentGameState = newGameState;
        
        switch (currentGameState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }
        
        OnGameStateChanged?.Invoke(currentGameState);
    }

    public GameState GetCurrentState()
    {
        return currentGameState;
    }

}

