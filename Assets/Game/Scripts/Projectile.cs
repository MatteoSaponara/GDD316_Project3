
using UnityEngine;

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

            Debug.Log("Hit object: " + collision.name);
            Debug.Log("Hit player: " + collision.GetComponentInParent<PlayerHealth>());
            Debug.Log("Owner: " + owner);
            Debug.Log("Soul tag: " + collision.CompareTag("Soul"));

            // Destroy the projectile when it hits the environment.
            Destroy(gameObject);
        }
    }
}
