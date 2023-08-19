using CitadelShowdown.UI.Citadel;
using System.Threading.Tasks;
using UnityEngine;

namespace CitadelShowdown.Citadel
{
    public class Player2Citadel : CitadelBase
    {
        public static Player2Citadel Instance { get; private set; }

        private GameObject playerCitadel;

        protected override void Start()
        {
            base.Start();
            currentHealth = maxHealth;

            playerCitadel = FindAnyObjectByType<Player1Citadel>().gameObject;
            uiCitadel = FindAnyObjectByType<UIPlayer2Citadel>();

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