using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int P1Points;
    public int P2Points;
    public pToStart pToStart;

    private bool gameStarted;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        GameOM.OnPlayerScored += IncreasePoints;
    }

    void OnDisable()
    {
        GameOM.OnPlayerScored -= IncreasePoints;
    }
    
    public void StartGame()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            GameOM.StartGame();
        }
    }

    public void IncreasePoints(pToStart p, int points)
    {
        if (p == pToStart.P1)
        {
            pToStart = pToStart.P2;
            P1Points += 1;
        }
        else if (p == pToStart.P2)
        {
            pToStart = pToStart.P1;
            P2Points += 1;
        }

        if (P1Points == 10)
        {
            GameOM.OnGameOver(pToStart.P1);
        }
        else if (P2Points == 10)
        {
            GameOM.OnGameOver(pToStart.P2);
        }
    }

    public pToStart GetScoredPlayer()
    {
        return pToStart;
    }
}
