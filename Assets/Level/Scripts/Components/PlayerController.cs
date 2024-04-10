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

            if (!IsGrounded)
            {
                MoveAirborne(movement);
                return;
            }

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

        private void OnJumpFinish()
        {
            IsJumping = false;
        }

        private void OnCrouch()
        {
            if (!IsGrounded)
            {
                Debug.Log("FastFall");
                FastFall();
                return;
            }

            Debug.Log("Crouch");

            Crouch();
        }

        private void OnTrick()
        {
            Debug.Log("Trick");
        }
    }
}