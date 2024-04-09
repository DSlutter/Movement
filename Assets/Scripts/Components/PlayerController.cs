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

            Debug.Log($"Move: {movement}");

            Move(movement);
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
    }
}