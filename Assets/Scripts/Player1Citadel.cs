using UnityEngine;

public class Player1Citadel : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isDragging = false;

    [SerializeField] private float maxDragDistance = 3f;
    [SerializeField] private float maxThrowForce = 10f;
    [SerializeField] private float minThrowForce = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private LineRenderer dragLine;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        dragLine = GetComponent<LineRenderer>();
        dragLine.enabled = false;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentTurn == GameManager.Turn.Player1)
        {
            HandlePlayerInput();
        }
    }

    private void HandlePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDragStart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnDragEnd();
        }

        if (isDragging)
        {
            UpdateDrag();
        }
    }

    private void OnDragStart()
    {
        isDragging = true;
        startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPosition.z = 0;

        dragLine.enabled = true;
        dragLine.SetPosition(0, startPosition);
        dragLine.SetPosition(1, startPosition);
    }

    private void OnDragEnd()
    {
        isDragging = false;
        endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;

        dragLine.enabled = false;

        Vector2 dragVector = startPosition - endPosition;
        float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);

        // Calculate throw force based on drag distance and angle
        float throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, dragDistance / maxDragDistance);
        Vector2 throwDirection = dragVector.normalized;

        // Spawn and throw a projectile
        SpawnAndThrowProjectile(throwDirection, throwForce);
    }

    private void UpdateDrag()
    {
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentPosition.z = 0;

        // Check if the drag distance exceeds the threshold
        Vector3 dragVector = startPosition - currentPosition;
        float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);
        if (dragDistance >= maxDragDistance)
        {
            dragVector = dragVector.normalized * maxDragDistance;
        }

        // Update the line renderer to visualize the drag
        dragLine.SetPosition(1, startPosition - dragVector);
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
