using UnityEngine;
using UnityEngine.UI;

public class UIPoints : MonoBehaviour
{
    [SerializeField] private Text P1PointsText;
    [SerializeField] private Text P2PointsText;

    private int P1Points;
    private int P2Points;
    void Start()
    {
        
    }

    void OnEnable()
    {
        GameOM.OnPlayerScored += ChangeText;
    }
    
    void OnDisable()
    {
        GameOM.OnPlayerScored -= ChangeText;
    }

    // Update is called once per frame
    void ChangeText(pToStart p, int points)
    {
        if (p == pToStart.P2)
        {
            P1Points += points;
            P1PointsText.text = P1Points.ToString();
        }
        else if (p == pToStart.P1)
        {
            P2Points += points;
            P2PointsText.text = P2Points.ToString();
        }
    }
}
