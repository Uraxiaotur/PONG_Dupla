using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector2 direction = ;
    [SerializeField] private float speed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
