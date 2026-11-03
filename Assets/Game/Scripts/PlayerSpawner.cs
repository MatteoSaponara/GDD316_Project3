
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;

        private int nextSpawnIndex = 0;

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            if (spawnPoints.Length == 0)
                return;

            // Choose the next available spawn point.
            Transform spawnPoint = spawnPoints[nextSpawnIndex];

            // Move the newly spawned player.
            playerInput.transform.position = spawnPoint.position;
            playerInput.transform.rotation = spawnPoint.rotation;

            // Move to the next spawn point.
            nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Length;
        }
    }
}
