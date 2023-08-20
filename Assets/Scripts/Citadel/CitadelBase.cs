using CitadelShowdown.DI;
using CitadelShowdown.Managers;
using CitadelShowdown.ProjectileNamespace;
using CitadelShowdown.UI.Citadel;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Citadel
{
    public abstract class CitadelBase : MonoBehaviour
    {
        [SerializeField] protected float maxDragDistance = 3f;
        [SerializeField] protected float maxThrowForce = 10f;
        [SerializeField] protected float minThrowForce = 0f;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected float maxHealth = 100;

        [SerializeField] protected float currentHealth;
        [SerializeField] protected float currentEnergy;
        [SerializeField] protected Vector2 throwDirection;
        [SerializeField] protected float throwForce;
        [SerializeField] protected Vector2 spawnPos;

        protected UICitadelBase uiCitadel;

        [SerializeField] protected bool isDragging = false;

        protected Projectile projectile;
        protected CoreLoopFacade coreLoopFacade { get; private set; }
        protected TrajectoryManager trajectoryManager { get; private set; }

        [Inject]
        public void Construct(CoreLoopFacade coreLoopFacade,
            Projectile projectile,
            TrajectoryManager trajectoryManager)
        {
            this.coreLoopFacade = coreLoopFacade;
            this.projectile = projectile;
            this.trajectoryManager = trajectoryManager;
        }

        protected virtual void Start()
        {
            Setup();
        }

        public void Setup()
        {
            spawnPos = transform.position;
            spawnPos.y += 2;
            currentHealth = coreLoopFacade.ConfigurationManager.GameConfigs.MaxHealth;
            currentEnergy = coreLoopFacade.ConfigurationManager.GameConfigs.MaxEnergy;
        }

        public void Renew()
        {
            currentHealth = coreLoopFacade.ConfigurationManager.GameConfigs.MaxHealth;
            currentEnergy = coreLoopFacade.ConfigurationManager.GameConfigs.MaxEnergy;
            UpdateUI();
        }

        protected void SpawnProjectile()
        {
            var launchPos = transform.position;
            launchPos.y += 2f;
            projectile.UpdateThisBaby(spawnPos);
            projectile.Renew();
        }

        protected virtual void ThrowProjectile()
        {
            projectile?.Throw(throwDirection, throwForce, coreLoopFacade.CurrentTurn);
        }

        public void RenewEnergy()
        {
            currentEnergy = Mathf.Min(currentEnergy + coreLoopFacade.ConfigurationManager.GameConfigs.EnergyRenewRate,
                coreLoopFacade.ConfigurationManager.GameConfigs.MaxEnergy);

            UpdateUI();
        }

        protected void SpendEnergy(int amount)
        {
            currentEnergy = Mathf.Max(currentEnergy - amount, 0);

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
            uiCitadel.UpdateEnergy(currentEnergy);
            uiCitadel.UpdateThrowForce(throwForce, minThrowForce, maxThrowForce);
            uiCitadel.UpdateThrowAngle(throwDirection);
        }
    }
}
