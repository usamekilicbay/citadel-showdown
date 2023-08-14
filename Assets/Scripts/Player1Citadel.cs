using UnityEngine;

public class Player1Citadel : MonoBehaviour
{
    private Vector3 startScreenPosition;
    private bool isDragging = false;

    private Vector2 throwDirection;
    private float throwForce;
    private Vector2 spawnPos;

    [SerializeField] private float maxDragDistance = 3f;
    [SerializeField] private float maxThrowForce = 10f;
    [SerializeField] private float minThrowForce = 2f;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private int numTrajectoryPoints = 10;
    [SerializeField] private GameObject trajectoryPointPrefab;
    private GameObject[] trajectoryPoints;
    private int trajectoryPointIndex = 0;

    private LineRenderer trajectoryLineRenderer;

    private void Start()
    {
        spawnPos = transform.position;
        spawnPos.y += 2;
        currentHealth = maxHealth;
        InitializeTrajectoryPoints();

        trajectoryLineRenderer = GetComponent<LineRenderer>();
        trajectoryLineRenderer.positionCount = 2;
        trajectoryLineRenderer.enabled = false;
    }

    private void InitializeTrajectoryPoints()
    {
        trajectoryPoints = new GameObject[numTrajectoryPoints];
        for (int i = 0; i < numTrajectoryPoints; i++)
        {
            trajectoryPoints[i] = Instantiate(trajectoryPointPrefab);
            trajectoryPoints[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentTurn != GameManager.Turn.Player1)
        {
            return;
        }

        HandlePlayerInput();
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
            UpdateTrajectory();
        }
        else
        {
            HideTrajectory();
        }
    }

    private void OnDragStart()
    {
        isDragging = true;
        startScreenPosition = Input.mousePosition;
        startScreenPosition.z = 10; // Depth of the Camera
        startScreenPosition = Camera.main.ScreenToWorldPoint(startScreenPosition);
    }

    private void UpdateDrag()
    {
        Vector3 currentPosition = Input.mousePosition;
        currentPosition.z = 10; // Depth of the Camera
        currentPosition = Camera.main.ScreenToWorldPoint(currentPosition);

        // Check if the drag distance exceeds the threshold
        Vector3 dragVector = startScreenPosition - currentPosition;
        float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);

        throwDirection = dragVector.normalized;
        throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, dragDistance / maxDragDistance);

        UpdateTrajectoryLine(startScreenPosition, currentPosition);
    }

    private void OnDragEnd()
    {
        isDragging = false;

        SpawnAndThrowProjectile(transform.position);
        trajectoryLineRenderer.enabled = false;
    }

    private void SpawnAndThrowProjectile(Vector3 spawnPosition)
    {
        spawnPosition.y += 2f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.Damage = 10;

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }

    private void UpdateTrajectory()
    {
        float timeStep = 0.1f;
        for (int i = 0; i < numTrajectoryPoints; i++)
        {
            float time = i * timeStep;
            Vector3 pointPosition = CalculatePointPosition(spawnPos, throwDirection, throwForce, time);
            trajectoryPoints[i].transform.position = pointPosition;
            trajectoryPoints[i].SetActive(true);
        }
    }

    private void UpdateTrajectoryLine(Vector3 start, Vector3 current)
    {
        Vector3 dragVector = start - current;
        float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);
        Vector3 clampedEnd = start - dragVector.normalized * dragDistance;

        trajectoryLineRenderer.enabled = true;
        trajectoryLineRenderer.SetPosition(0, start);
        trajectoryLineRenderer.SetPosition(1, clampedEnd);
    }

    private Vector3 CalculatePointPosition(Vector3 initialPosition, Vector2 direction, float force, float time)
    {
        float gravity = Physics2D.gravity.y;
        Vector2 displacement = direction * force * time + 0.5f * Vector2.up * gravity * time * time;
        return initialPosition + new Vector3(displacement.x, displacement.y, 0);
    }

    private void HideTrajectory()
    {
        foreach (GameObject point in trajectoryPoints)
        {
            point.SetActive(false);
        }

        trajectoryLineRenderer.enabled = false;
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
