using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Game
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputManager playerInputManager;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private Transform[] cornerSlots;

        private Dictionary<PlayerInput, int> playerSlots = new Dictionary<PlayerInput, int>();

        private bool[] occupiedSlots;

        private void Awake()
        {
            if (playerInputManager == null)
            {
                Debug.LogError("PlayerInputManager is not assigned!");
            }

            if (healthBarPrefab == null)
            {
                Debug.LogError("Health Bar Prefab is not assigned!");
            }

            if (cornerSlots == null || cornerSlots.Length == 0)
            {
                Debug.LogError("No corner slots assigned!");
            }

            occupiedSlots = new bool[cornerSlots.Length];
        }

        public void OnPlayerJoined(PlayerInput player)
        {
            Debug.Log("Detected player joined: " + player.playerIndex);

            // Find the first available corner
            for (int i = 0; i < cornerSlots.Length; i++)
            {
                if (!occupiedSlots[i])
                {
                    if (cornerSlots[i] == null)
                    {
                        Debug.LogError("Corner slot " + i + " is not assigned!");
                        return;
                    }

                    occupiedSlots[i] = true;
                    playerSlots.Add(player, i);

                    // Create the player's health bar
                    GameObject healthBar = Instantiate(
                        healthBarPrefab,
                        cornerSlots[i],
                        false
                    );

                    healthBar.transform.localPosition = Vector3.zero;
                    healthBar.transform.localRotation = Quaternion.identity;
                    healthBar.transform.localScale = Vector3.one;

                    // Connect the health bar to the player's health
                    HealthBarUI healthBarUI = healthBar.GetComponentInChildren<HealthBarUI>();
                    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                    Debug.Log(
                     $"UI: PlayerIndex={player.playerIndex}, " +
                     $"PlayerInputID={player.GetEntityId()}, " +
                     $"HealthID={playerHealth.GetEntityId()}");

                    healthBarUI.Initialize(playerHealth);

                    Debug.Log("Health bar initialized!");

                    Debug.Log("Created health bar in slot " + i);

                    break;
                }
            }
        }

        public void OnPlayerLeft(PlayerInput player)
        {
            if (playerSlots.TryGetValue(player, out int slot))
            {
                occupiedSlots[slot] = false;
                playerSlots.Remove(player);
            }
        }
    }
}