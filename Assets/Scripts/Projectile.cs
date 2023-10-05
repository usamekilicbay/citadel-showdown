using Assets.Scripts.Common.Types;
using CitadelShowdown.Citadel;
using CitadelShowdown.DI;
using CitadelShowdown.Managers;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.ProjectileNamespace
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float radius = 0.5f;
        [SerializeField] private ProjectileState state;

        [SerializeField] private ParticleSystem _trailEffect;
        private bool _hasReachedPeak = false;
        private float _lastVelocityY = 0f;
        private Attack _attack;

        private CoreLoopFacade _coreLoopFacade;
        private CameraManager _cameraManager;

        [Inject]
        public void Construct(CoreLoopFacade coreLoopFacade,
             CameraManager cameraManager)
        {
            _coreLoopFacade = coreLoopFacade;
            _cameraManager = cameraManager;
        }

        private void Awake()
        {
            Setup();
        }

        public void Setup()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            _trailEffect = GetComponentInChildren<ParticleSystem>();
            Renew();
        }

        public void Spawn(Vector2 launchPos, AttackType attackType)
        {
            _attack = _coreLoopFacade.AttackManager.GetAttack(attackType);
            transform.position = launchPos;
            Renew();
            spriteRenderer.enabled = true;
            _trailEffect.Play();
        }

        public void Renew()
        {
            var layer = _coreLoopFacade.BattleState ==  BattleState.Player1
                ? 7 // Set the layer number for Player1
                : 6; // Set the layer number for Player2

            // Create a LayerMask using the layer number
            layerMask = (1 << layer) | (1 << 8); // Add layer 8 to the LayerMask

            _trailEffect.Stop();
            spriteRenderer.enabled = false;
            rb.gravityScale = 0;
            state = ProjectileState.Respawned;
        }

        public void Throw(Vector2 throwDirection, float throwForce)
        {
            state = ProjectileState.Thrown;
            rb.gravityScale = 1;
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

            _cameraManager.SwitchToProjectileCamera();
        }

        private void Update()
        {
            if (state != ProjectileState.Thrown)
                return;

            if (transform.position.y <= -10f)
                Vanish(true);
        }

        private void FixedUpdate()
        {
            if (state != ProjectileState.Thrown)
                return;

            var position = transform.position;

            var hit = Physics2D.CircleCast(position, radius, Vector2.zero, 0f, layerMask);

            if (hit.collider != null)
                HandleCollision(hit.collider);
        }

        private async void HandleCollision(Collider2D collider)
        {
            if (state == ProjectileState.Vanished)
                return;

            Vanish(false);

            Debug.Log(collider.name);

            if (collider.gameObject.TryGetComponent(out CitadelBase targetCitadel))
            {
                var isGameOver = await targetCitadel.TakeDamageAsync(_attack.Damage);

                if (!isGameOver)
                    _coreLoopFacade.SwitchTurn();
            }
        }

        private async void Vanish(bool isFall)
        {
            Time.timeScale = 1f;

            state = ProjectileState.Vanished;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            spriteRenderer.enabled = false;
            await _coreLoopFacade.GameManager.MMFPlayerProjectile.PlayFeedbacksTask(transform.position);

            if (isFall)
                _coreLoopFacade.SwitchTurn();
        }

        private void OnDrawGizmos()
        {
            var origin = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin, radius);
        }
    }
}
