using Assets.Scripts.Common.Types;
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
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float currentEnergy;
        [SerializeField] protected Vector2 throwDirection;
        [SerializeField] protected float throwForce;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected Vector2 spawnPos;
        [SerializeField] protected bool isDragging = false;

        protected AttackType currentAttack;

        protected Projectile projectile;

        protected UICitadelBase uiCitadel;
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
            uiCitadel.UpdateHealthText(currentHealth);
            uiCitadel.UpdateEnergyText(currentEnergy);
            uiCitadel.ToggleIndicators(false);
        }

        public void Renew()
        {
            currentHealth = coreLoopFacade.ConfigurationManager.GameConfigs.MaxHealth;
            currentEnergy = coreLoopFacade.ConfigurationManager.GameConfigs.MaxEnergy;
            uiCitadel.UpdateHealthText(currentHealth);
            uiCitadel.UpdateEnergyText(currentEnergy);
            uiCitadel.ToggleIndicators(false);
        }

        protected void SpawnProjectile()
        {
            var launchPos = transform.position;
            launchPos.y += 2f;
            projectile.UpdateThisBaby(spawnPos, currentAttack);
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

            uiCitadel.UpdateEnergyText(currentEnergy);
        }

        public void SetAttackType(AttackType attackType)
        {
            currentAttack = attackType;
        }

        protected void SpendEnergy(int amount)
        {
            currentEnergy = Mathf.Max(currentEnergy - amount, 0);

            uiCitadel.UpdateEnergyText(currentEnergy);
        }

        public async Task TakeDamageAsync(int damageAmount)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                await coreLoopFacade.GameManager.CompleteRun();
            }

            uiCitadel.UpdateHealthText(currentHealth);
        }
    }
}
