using System;
using UnityEngine;

public class GameOM
{
    public static Action OnGameStart;

    public static void StartGame()
    {
        OnGameStart?.Invoke();
    }
    
    public static Action<pToStart> OnPlayerScored;

    public static void PlayerScored(pToStart p);
    {
        OnPlayerScored?.Invoke(p);
    }
}
