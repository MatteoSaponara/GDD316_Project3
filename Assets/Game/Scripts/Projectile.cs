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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            ///rb.velocity = transform.

            Destroy(gameObject, lifetime);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandleHit(collision.gameObject);
        }

        private void HandleHit(GameObject hitObject)
        {
            if (hitObject.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
