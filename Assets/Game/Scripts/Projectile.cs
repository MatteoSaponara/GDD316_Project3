
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private float projectileSpeed = 1f;
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private float damage = 5f;

        private Rigidbody2D rb;
        private PlayerHealth owner;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void SetOwner(PlayerHealth player)
        {
            owner = player;
        }

        public void Launch(Vector2 direction)
        {
            rb.linearVelocity = direction.normalized * projectileSpeed;
        }

        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Find the player that owns the object we hit.
            PlayerHealth hitPlayer = collision.GetComponentInParent<PlayerHealth>();

            if (hitPlayer != null)
            {
                PlayerInput hitInput = hitPlayer.GetComponent<PlayerInput>();

                Debug.Log(
                    $"Projectile: PlayerIndex={(hitInput != null ? hitInput.playerIndex : -1)}, " +
                    $"PlayerInputID={(hitInput != null ? hitInput.GetEntityId() : -1)}, " +
                    $"HealthID={hitPlayer.GetEntityId()}"
                );

                // Ignore the firing player's own objects.
                if (hitPlayer == owner)
                    return;

                // Only damage another player's Soul.
                if (collision.CompareTag("Soul"))
                {
                    hitPlayer.TakeDamage(damage);
                    Destroy(gameObject);
                }

                return;
            }

            // Destroy the projectile when it hits the environment.
            Destroy(gameObject);
        }
    }
}
