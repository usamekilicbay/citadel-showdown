using System.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage;
    [SerializeField] private Rigidbody2D rb;

    private bool hasReachedPeak = false;
    private float lastVelocityY = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CameraController.Instance.SwitchToProjectileCamera(transform);
    }

    private void Update()
    {
        //// Check if the velocity has changed from positive to negative
        //if (!hasReachedPeak && rb.velocity.y < 0 && lastVelocityY > 0)
        //{
        //    // Projectile has likely reached its peak height, update mass
        //    hasReachedPeak = true;
        //    rb.gravityScale = 2f;
        //}

        //lastVelocityY = rb.velocity.y;

        if (transform.position.y <= -10f)
            Vanish();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player 1 Citadel" || collision.gameObject.name == "Player 2 Citadel")
        {
            if (GameManager.Instance.CurrentTurn == GameManager.Turn.Player1)
            {
                collision.gameObject.TryGetComponent(out Player2Citadel victim);
                victim.TakeDamage(Damage);
            }
            else
            {
                collision.gameObject.TryGetComponent(out Player1Citadel victim);
                victim.TakeDamage(Damage);
            }
        }

        Vanish();
    }

    private async void Vanish()
    {
        await GameManager.Instance.MMFPlayerProjectile.PlayFeedbacksTask(transform.position);

        await Task.Delay(1000);
        // Switch turn after a successful throw
        GameManager.Instance.SwitchTurn();

        Destroy(gameObject);
    }
}
