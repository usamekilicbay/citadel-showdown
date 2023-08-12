using System.Threading.Tasks;
using UnityEngine;

public class Player2Citadel : MonoBehaviour
{
    public static Player2Citadel Instance { get; private set; }

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float maxThrowForce = 10f;
    [SerializeField] private float minThrowForce = 2f;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private GameObject playerCitadel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate Player2Citadel objects
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;

        playerCitadel = GameObject.Find("Player 1 Citadel");
    }

    public async void SimulateAITurn()
    {
        await Task.Delay(3000);

        // Calculate AI's throw direction, force, and attack type
        Vector2 throwDirection = CalculateThrowDirection();
        float throwForce = CalculateThrowForce();

        // Spawn and throw a projectile
        SpawnAndThrowProjectile(throwDirection, throwForce);
    }

    private Vector2 CalculateThrowDirection()
    {
        // Calculate a random angle for the throw
        float randomAngle = Random.Range(0f, 90f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, -randomAngle);

        // Calculate the throw direction based on the rotation
        Vector2 throwDirection = rotation * Vector2.left;

        return throwDirection;
    }

    private float CalculateThrowForce()
    {
        // Calculate a random throw force within the specified range
        float throwForce = Random.Range(minThrowForce, maxThrowForce);

        return throwForce;
    }

    private void SpawnAndThrowProjectile(Vector2 throwDirection, float throwForce)
    {
        var spawnPosition = transform.position;
        spawnPosition.y += 2f;
        var projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity)
          .GetComponent<Projectile>();
        projectile.Damage = 10;
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        // Apply throw force to the projectile
        projectileRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameManager.Instance.GameOver();
        }
    }
}
