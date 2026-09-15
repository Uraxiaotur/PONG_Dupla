using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float speed;

    private bool inMovement;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.linearVelocity != Vector2.zero)
        {
            inMovement = true;
        }
        
        if (!inMovement && Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBall();
        }
    }

    void OnEnable()
    {
        GameOM.OnGameStart += ThrowBall;
    }

    void OnDisable()
    {
        GameOM.OnGameStart -= ThrowBall;
    }
    
    private void ThrowBall()
    {
        SetDirection();
        direction = direction.normalized;
        rb.linearVelocity = direction * speed;
    }

    private void SetDirection()
    {
        if (GameManager.Instance.GetScoredPlayer() == pToStart.None)
        {
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));   
        }
        else if (GameManager.Instance.GetScoredPlayer() == pToStart.P1)
        {
            direction = new Vector2(-1f, Random.Range(-1f, 1f));   
        }
        else if (GameManager.Instance.GetScoredPlayer() == pToStart.P2)
        {
            direction = new Vector2(1f, Random.Range(-1f, 1f));   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            direction.x *= -1f;
            speed += 0.5f;
        }
        else if (collision.gameObject.CompareTag("Parede"))
        {
            direction.y *= -1f;
        }

        if (collision.gameObject.CompareTag("Score1"))
        {
            direction = Vector2.zero;
            GameOM.OnPlayerScored(pToStart.P1, 1);
        }
        else if (collision.gameObject.CompareTag("Score2"))
        {
            direction = Vector2.zero;
            GameOM.OnPlayerScored(pToStart.P2, 1);
        }
    }
    
}
