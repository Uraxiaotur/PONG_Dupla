using UnityEngine;

public enum pToStart
{
    None,
    P1,
    P2
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int P1Points;
    public static int P2Points;
}
