
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("References")]
        [SerializeField] GameObject playerSoul;
        [SerializeField] GameObject playerBody;
        [SerializeField] GameObject firePoint;

        [Tooltip("Movement")]
        [SerializeField] float maxSoulSpeed = 15f;
        [SerializeField] float maxBodySpeed = 10f;

        [SerializeField] float accelerationTime = 0.5f;
        [SerializeField] float decelerationTime = 0.25f;

        private Vector2 currentSoulVelocity;
        private Vector2 currentBodyVelocity;

        private Vector2 lastSoulInput;
        private Vector2 lastBodyInput;

        private void OnMoveSoul(InputValue value)
        {
            lastSoulInput = value.Get<Vector2>();
        }

        private void OnMoveBody(InputValue value)
        {
            lastBodyInput = value.Get<Vector2>();
        }

        // Rotates the body to face the direction is it moving in
        private void RotateBody()
        {
            // Only rotate when the body is moving.
            if (currentBodyVelocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(
                    currentBodyVelocity.y,
                    currentBodyVelocity.x
                ) * Mathf.Rad2Deg;

                playerBody.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            // Keep diagonal movement from exceeding the maximum speed.
            Vector2 soulInput = Vector2.ClampMagnitude(lastSoulInput, 1f);
            Vector2 bodyInput = Vector2.ClampMagnitude(lastBodyInput, 1f);

            // Calculate the target velocity for each character.
            Vector2 targetSoulVelocity = soulInput * maxSoulSpeed;
            Vector2 targetBodyVelocity = bodyInput * maxBodySpeed;

            // Accelerate when moving, decelerate when there is no input.
            float soulTime = soulInput.sqrMagnitude > 0f
                ? accelerationTime : decelerationTime;

            float bodyTime = bodyInput.sqrMagnitude > 0f
                ? accelerationTime : decelerationTime;

            float soulRate = maxSoulSpeed / Mathf.Max(soulTime, 0.001f);
            float bodyRate = maxBodySpeed / Mathf.Max(bodyTime, 0.001f);

            currentSoulVelocity = Vector2.MoveTowards(
                currentSoulVelocity,
                targetSoulVelocity,
                soulRate * Time.deltaTime
            );

            currentBodyVelocity = Vector2.MoveTowards(
                currentBodyVelocity,
                targetBodyVelocity,
                bodyRate * Time.deltaTime
            );

            // Apply movement independently.
            playerSoul.transform.position +=
                (Vector3)(currentSoulVelocity * Time.deltaTime);

            playerBody.transform.position +=
                (Vector3)(currentBodyVelocity * Time.deltaTime);

            RotateBody();
        }

        private void OnShoot()
        {
            FireProjectile();
        }

        private void FireProjectile()
        {
            Debug.Log("Projectile fired");
        }
    }
}
