﻿using Assets.Scripts.Common.Types;
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
        [SerializeField] protected Transform trebuchetThrowTransform;
        [SerializeField] protected Collider2D collider;

        protected ActionType actionType;

        protected AttackType currentAttack;

        protected Projectile projectile;

        protected UICitadelBase uiCitadel;
        protected CoreLoopFacade coreLoopFacade { get; private set; }
        protected TrajectoryManager trajectoryManager { get; private set; }
        protected CameraManager cameraManager { get; private set; }
        protected ConfigurationManager configurationManager { get; private set; }


        [Inject]
        public void Construct(CoreLoopFacade coreLoopFacade,
            Projectile projectile,
            TrajectoryManager trajectoryManager,
            CameraManager cameraManager,
            ConfigurationManager configurationManager)
        {
            this.coreLoopFacade = coreLoopFacade;
            this.projectile = projectile;
            this.trajectoryManager = trajectoryManager;
            this.cameraManager = cameraManager;
            this.configurationManager = configurationManager;
        }

        protected virtual void Awake()
        {
            collider = GetComponent<Collider2D>();

            //TODO: Update after implementing trebuchet assets
            trebuchetThrowTransform = transform;
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
            //Renew();
        }

        public void Renew()
        {
            currentHealth = coreLoopFacade.ConfigurationManager.GameConfigs.MaxHealth;
            currentEnergy = coreLoopFacade.ConfigurationManager.GameConfigs.MaxEnergy;
            uiCitadel.UpdateHealthText(currentHealth);
            uiCitadel.UpdateEnergyText(currentEnergy);
            uiCitadel.ToggleIndicators(false);
            uiCitadel.ToggleAttackSelectionButtons(false);

            Debug.Log("Oooo   " + gameObject.name);
        }

        public async virtual Task UpdateTurn(CancellationToken cancellationToken = default)
        {
            await Task.FromResult(actionType = ActionType.Select);

            uiCitadel.ToggleAttackSelectionButtons(true);

            Debug.Log("Oooowww shieeet   " + gameObject.name);


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
            uiCitadel.ToggleAttackSelectionButtons(false);

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
            Debug.Log("spanw");

            var launchPos = transform.position;
            launchPos.y += 2f;
            projectile.Spawn(launchPos, currentAttack);
        }

        protected virtual void ThrowProjectile()
            => projectile.Throw(throwDirection, throwForce);

        public async Task<bool> TakeDamageAsync(int damageAmount, CancellationToken cancellationToken = default)
        {
            currentHealth -= damageAmount;

            uiCitadel.UpdateHealthText(currentHealth);

            await Task.Delay(1000);

            if (currentHealth > 0)
                return false;

            currentHealth = 0;
            await coreLoopFacade.GameManager.CompleteRun();
            return true;
        }
    }
}
