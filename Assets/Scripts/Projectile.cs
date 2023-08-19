using CitadelShowdown.Citadel;
using CitadelShowdown.DI;
using CitadelShowdown.Managers;
using System.Reflection.Emit;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.ProjectileNamespace
{
    public class Projectile : MonoBehaviourBase
    {
        public int Damage;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool hasReachedPeak = false;
        private float lastVelocityY = 0f;
        private bool isUsed = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Renew();
        }

        public void Renew()
        {
            rb.gravityScale = 0;
        }

        public void Throw(Vector2 throwDirection, float throwForce, TurnType attacker)
        {
            Damage = 10;

            rb.gravityScale = 1;
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

            CameraManager.Instance.SwitchToProjectileCamera(transform);
            //Time.timeScale = 0.3f;
        }

        private void Update()
        {
            if (isUsed)
                return;

            //// Check if the velocity has changed from positive to negative
            //if (!hasReachedPeak && rb.velocity.y < 0 && lastVelocityY > 0)
            //{
            //    // Projectile has likely reached its peak height, update mass
            //    hasReachedPeak = true;
            //    rb.gravityScale = 2f;
            //}

            //lastVelocityY = rb.velocity.y;

            if (transform.position.y <= -10f)
                Vanish();
        }

        private async void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Player 1 Citadel" || collision.gameObject.name == "Player 2 Citadel")
            {
                if (coreLoopFacade.GameManager.CurrentTurn == TurnType.Player1)
                {
                    if (collision.gameObject.TryGetComponent(out Player2Citadel victim))
                        await victim.TakeDamageAsync(Damage);
                }
                else
                {
                    if (collision.gameObject.TryGetComponent(out Player1Citadel victim))
                        await victim.TakeDamageAsync(Damage);
                }
            }

            Vanish();
        }

        private async void Vanish()
        {
            Time.timeScale = 1f;

            if (isUsed)
                return;

            isUsed = true;

            Destroy(spriteRenderer);
            Destroy(rb);

            await coreLoopFacade.GameManager.MMFPlayerProjectile.PlayFeedbacksTask(transform.position);


            //await Task.Delay(1000);
            // Switch turn after a successful throw
            coreLoopFacade.GameManager.SwitchTurn();

            Destroy(gameObject);
        }
    }
}
