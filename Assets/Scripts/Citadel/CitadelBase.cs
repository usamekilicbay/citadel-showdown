using Assets.Scripts.Common.Types;
using CitadelShowdown.DI;
using CitadelShowdown.Managers;
using CitadelShowdown.ProjectileNamespace;
using CitadelShowdown.UI.Citadel;
using System.Threading;
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
        [SerializeField] protected bool isDragging = false;

        [SerializeField] protected Collider2D collider;

        protected ActionType actionType;

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

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            coreLoopFacade.BattleManager.OnTurnChange += UpdateTurn;
        }

        private void OnDisable()
        {
            coreLoopFacade.BattleManager.OnTurnChange -= UpdateTurn;
        }

        protected virtual void Start()
        {
            Setup();
        }

        public void Setup()
        {
            Renew();
        }

        public void Renew()
        {
            currentHealth = coreLoopFacade.ConfigurationManager.GameConfigs.MaxHealth;
            currentEnergy = coreLoopFacade.ConfigurationManager.GameConfigs.MaxEnergy;
            uiCitadel.UpdateHealthText(currentHealth);
            uiCitadel.UpdateEnergyText(currentEnergy);
            uiCitadel.ToggleIndicators(false);
        }

        public async virtual Task UpdateTurn()
        {
            await Task.FromResult(actionType = ActionType.Select);

            RenewEnergy();
        }

        public void RenewEnergy()
        {
            currentEnergy = Mathf.Min(currentEnergy + coreLoopFacade.ConfigurationManager.GameConfigs.EnergyRenewRate,
                coreLoopFacade.ConfigurationManager.GameConfigs.MaxEnergy);

            uiCitadel.UpdateEnergyText(currentEnergy);
        }

        public async virtual Task SetAttackType(AttackType attackType, CancellationToken cancellationToken = default)
        {
            currentAttack = attackType;
            SpawnProjectile();
            actionType = ActionType.Attack;
        }

        protected void SpendEnergy(int amount)
        {
            currentEnergy = Mathf.Max(currentEnergy - amount, 0);

            uiCitadel.UpdateEnergyText(currentEnergy);
        }

        protected void SpawnProjectile()
        {
            var launchPos = transform.position;
            launchPos.y += 2f;
            projectile.UpdateThisBaby(launchPos, currentAttack);
        }

        protected virtual void ThrowProjectile()
            => projectile?.Throw(throwDirection, throwForce);

        public async Task TakeDamageAsync(int damageAmount, CancellationToken cancellationToken = default)
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
