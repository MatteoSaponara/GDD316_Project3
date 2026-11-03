using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image healthFill;

        private PlayerHealth playerHealth;

        public void Initialize(PlayerHealth health)
        {
            Debug.Log("HealthBarUI Initialize called on: " + gameObject.name);
            Debug.Log("HealthFill assigned: " + (healthFill != null));
            playerHealth = health;

            playerHealth.OnHealthChanged += UpdateHealthBar;

            // Initialize the health bar
            UpdateHealthBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        private void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            Debug.Log("Updating health bar: " + currentHealth + " / " + maxHealth);

            healthFill.fillAmount = currentHealth / maxHealth;
        }

        private void OnDestroy()
        {
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged -= UpdateHealthBar;
            }
        }
    }
}