using Assets.Scripts.Common.Types;
using CitadelShowdown.UI.Citadel;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Citadel
{
    public class Player2Citadel : CitadelBase
    {
        private Player1Citadel _playerCitadel;

        [Inject]
        public void Construct(UIPlayer2Citadell uiPlayer2Citadel,
            Player1Citadel player1Citadel)
        {
            _playerCitadel = player1Citadel;
            uiCitadel = uiPlayer2Citadel;
        }

        public override async Task UpdateTurn(CancellationToken cancellationToken = default)
        {
            if (coreLoopFacade.BattleState != BattleState.Player2)
                return;

            await base.UpdateTurn();

            //collider.enabled = coreLoopFacade.BattleState == BattleState.Player1;

            await SetAttackType(AttackType.RegularAttack, cancellationToken);

            await SimulateAITurn();
        }

        public async override Task SetAttackType(AttackType attackType, CancellationToken cancellationToken = default)
        {
            await base.SetAttackType(attackType);
        }

        public async Task SimulateAITurn(CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000);

            // Calculate AI's throw direction, force, and attack type
            throwDirection = CalculateThrowDirection();
            throwForce = CalculateThrowForce();

            // Spawn and throw a projectile
            ThrowProjectile();
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
            float throwForce = Random.Range(coreLoopFacade.ConfigurationManager.MovementConfigs.MinThrowForce, coreLoopFacade.ConfigurationManager.MovementConfigs.MaxThrowForce);

            return throwForce;
        }
    }
}
