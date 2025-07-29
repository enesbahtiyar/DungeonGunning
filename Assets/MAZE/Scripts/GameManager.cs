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
    }
    
    public void SetState(GameState newGameState)
    {
        currentGameState=newGameState;
        OnGameStateChanged?.Invoke(currentGameState);

    }

}
