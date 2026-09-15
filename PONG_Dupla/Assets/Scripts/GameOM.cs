using System;
using UnityEngine;

public enum pToStart
{
    None,
    P1,
    P2
}

public class GameOM
{
    public static Action OnGameStart;

    public static void StartGame()
    {
        OnGameStart?.Invoke();
    }
    
    public static Action<pToStart, int> OnPlayerScored;

    public static void PlayerScored(pToStart p, int score)
    {
        OnPlayerScored?.Invoke(p, score);
    }
    
    public static Action<pToStart> OnGameOver;
    public static void GameOver(pToStart p)
    {
        OnGameOver?.Invoke(p);
    }
    
}
