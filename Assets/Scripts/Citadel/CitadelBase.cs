using CitadelShowdown.DI;
using CitadelShowdown.Managers;
using CitadelShowdown.ProjectileNamespace;
using CitadelShowdown.UI.Citadel;
using System.Threading.Tasks;
using UnityEngine;

namespace CitadelShowdown.Citadel
{
    public abstract class CitadelBase : MonoBehaviourBase
    {
        [SerializeField] protected float maxDragDistance = 3f;
        [SerializeField] protected float maxThrowForce = 10f;
        [SerializeField] protected float minThrowForce = 2f;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected float maxHealth = 100;

        [Header("Trajectory")]
        [SerializeField] protected int numTrajectoryPoints = 10;
        [SerializeField] protected GameObject trajectoryPointPrefab;
        [SerializeField] protected GameObject[] trajectoryPoints;
        [SerializeField] protected int trajectoryPointIndex = 0;

        [SerializeField] protected LineRenderer trajectoryLineRenderer;

        [SerializeField] protected float currentHealth;
        [SerializeField] protected float currentStamina;
        [SerializeField] protected Vector2 throwDirection;
        [SerializeField] protected float throwForce;
        [SerializeField] protected Vector2 spawnPos;

        protected UICitadelBase uiCitadel;

        [SerializeField] protected bool isDragging = false;

        protected Projectile currentProjectile;

        protected virtual void Start()
        {
            spawnPos = transform.position;
            spawnPos.y += 2;
            currentHealth = maxHealth;
            currentStamina = StaminaManager.Instance.MaxStamina;
            InitializeTrajectoryPoints();

            trajectoryLineRenderer = GetComponent<LineRenderer>();
            trajectoryLineRenderer.positionCount = 2;
            trajectoryLineRenderer.enabled = false;
        }

        public void Renew()
        {
            HideTrajectory();
            currentHealth = maxHealth;
            currentStamina = StaminaManager.Instance.MaxStamina;
            UpdateUI();
        }

        protected void SpawnProjectile()
        {
            var spawnPosition = transform.position;
            spawnPosition.y += 2f;
            var projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            currentProjectile = projectile.GetComponent<Projectile>();
        }

        protected virtual void ThrowProjectile()
        {
            currentProjectile?.Throw(throwDirection, throwForce, coreLoopFacade.GetCurrentTurn());
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

        protected void UpdateTrajectory()
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

        protected void UpdateTrajectoryLine(Vector3 start, Vector3 current)
        {
            Vector3 dragVector = start - current;
            float dragDistance = Mathf.Clamp(dragVector.magnitude, 0, maxDragDistance);
            Vector3 clampedEnd = start - dragVector.normalized * dragDistance;

            trajectoryLineRenderer.enabled = true;
            trajectoryLineRenderer.SetPosition(0, start);
            trajectoryLineRenderer.SetPosition(1, clampedEnd);
        }

        protected Vector3 CalculatePointPosition(Vector3 initialPosition, Vector2 direction, float force, float time)
        {
            float gravity = Physics2D.gravity.y;
            Vector2 displacement = direction * force * time + 0.5f * Vector2.up * gravity * time * time;
            return initialPosition + new Vector3(displacement.x, displacement.y, 0);
        }

        protected void HideTrajectory()
        {
            foreach (GameObject point in trajectoryPoints)
            {
                point.SetActive(false);
            }

            trajectoryLineRenderer.enabled = false;
        }

        public void RenewStamina()
        {
            currentStamina = Mathf.Min(currentStamina + StaminaManager.Instance.StaminaRenewAmount, StaminaManager.Instance.MaxStamina);

            UpdateUI();
        }

        protected void SpendStamina(int amount)
        {
            currentStamina = Mathf.Max(currentStamina - amount, 0);

            UpdateUI();
        }

        public async Task TakeDamageAsync(int damageAmount)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
               await coreLoopFacade.GameManager.CompleteRun();
            }

            UpdateUI();
        }

        protected void UpdateUI()
        {
            uiCitadel.UpdateHealth(currentHealth);
            uiCitadel.UpdateStamina(currentStamina);
            uiCitadel.UpdateThrowForce(throwForce, minThrowForce, maxThrowForce);
            uiCitadel.UpdateThrowAngle(throwDirection);
        }
    }
}