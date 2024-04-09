using Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components
{
    public class PlayerController : MovementBehaviour
    {
        private void OnMove(InputValue inputValue)
        {
            var movement = inputValue.Get<Vector2>();

            //Debug.Log($"Move: {movement}");

            Move(movement);
        }

        private void OnAccelerate()
        {
            Debug.Log($"Accelerate");

            Accelerate();
        }

        private void OnAccelerateFinish()
        {
            Debug.Log($"Decelerate");

            Decelerate();
        }

        private void OnJump()
        {
            if (!IsGrounded)
            {
                return;
            }

            Debug.Log("Jump");

            Jump();
        }

        private void OnCrouch()
        {
            if (!IsGrounded)
            {
                return;
            }

            Debug.Log("Crouch");

            Jump();
        }

        private void OnTrick()
        {
            if (!IsGrounded)
            {
                return;
            }

            Debug.Log("Trick");

            Jump();
        }
    }
}