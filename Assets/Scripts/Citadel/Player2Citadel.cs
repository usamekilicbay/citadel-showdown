using CitadelShowdown.DI;
using CitadelShowdown.UI.Citadel;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.Citadel
{
    public class Player2Citadel : CitadelBase
    {
        private Player1Citadel _playerCitadel;

        [Inject]
        public void Construct(CoreLoopFacade coreLoopFacade,
            UIPlayer2Citadell uiPlayer2Citadel,
            Player1Citadel player1Citadel)
        {
            _playerCitadel = player1Citadel;
            uiCitadel = uiPlayer2Citadel;
        }

        protected override void Start()
        {
            base.Start();
            currentHealth = maxHealth;

            UpdateUI();
        }

        public async void SimulateAITurn()
        {
            await Task.Delay(3000);

            // Calculate AI's throw direction, force, and attack type
            throwDirection = CalculateThrowDirection();
            throwForce = CalculateThrowForce();

            // Spawn and throw a projectile
            SpawnProjectile();
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
            float throwForce = Random.Range(minThrowForce, maxThrowForce);

            return throwForce;
        }
    }
}
