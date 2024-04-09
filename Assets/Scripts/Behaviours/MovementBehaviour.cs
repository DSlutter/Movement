using UnityEngine;

namespace Behaviours
{
    public class MovementBehaviour : MonoBehaviour
    {
        protected bool IsGrounded => _characterController.isGrounded;

        [SerializeField]
        private float _moveSpeed;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private float _gravityMultiplier;

        private float _gravity = -3f;

        private CharacterController _characterController;

        private Vector3 _direction = new();

        private float _velocity;

        protected void Move(Vector2 movement)
        {
            _direction = new Vector3(movement.x, _direction.y, movement.y);
        }

        protected void Jump()
        {
            Debug.Log("Jump");
            _velocity += (_jumpForce / 10);
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            ApplyGravity();
            ApplyMovement();
        }

        private void ApplyGravity()
        {
            // Reset velocity and make sure jumping is set to false.
            if (_characterController.isGrounded && _velocity < 0.0f)
            {
                _velocity = -0.1f;
            }
            // Apply downwards gravity.
            else
            {
                _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
            }

            _direction.y = _velocity;
        }

        private void ApplyMovement()
        {
            _characterController.Move(_direction * _moveSpeed * Time.deltaTime);
        }
    }
}