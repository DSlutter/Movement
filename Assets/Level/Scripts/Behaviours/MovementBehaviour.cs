using System.Collections;
using UnityEngine;

namespace Behaviours
{
    public class MovementBehaviour : MonoBehaviour
    {
        protected bool IsGrounded => _characterController.isGrounded;

        protected bool IsJumping;

        private readonly float _gravity = -3f;

        private readonly float _maxRotation = 45f;

        private readonly float _neutralRotationMargin = 5f;

        private readonly float _airSteeringSensitivity = 0.3f;

        private readonly float _rayCastDistance = 2f;

        [SerializeField]
        private GameObject _rider;

        [SerializeField]
        private GameObject _model;

        [SerializeField]
        private float _baseSpeed;

        [SerializeField]
        private float _rotationSpeed;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private float _gravityMultiplier;

        [SerializeField]
        private float _crouchHeightModifier;

        private float _maxSpeed;

        private float _moveSpeed;

        private CharacterController _characterController;

        private Vector3 _direction = Vector3.zero;

        private float _horizontalRotation;

        private float _velocity;

        private Vector3 _riderSize;

        private Vector3 _riderPosition;

        private bool _isAccelerating;

        private bool _isCrouched;

        private bool _wasGrounded;

        private bool _isFastFalling;

        private RaycastHit _previousGroundHit;

        protected void Move(Vector2 movement)
        {
            _direction = new Vector3(movement.x, _direction.y, movement.y);
        }

        protected void MoveAirborne(Vector2 movement)
        {
            _direction = new Vector3(movement.x * _airSteeringSensitivity, _direction.y, movement.y);
        }

        protected void Accelerate()
        {
            _isAccelerating = true;
        }

        protected void Decelerate()
        {
            _isAccelerating = false;
        }

        protected void Jump()
        {
            _velocity += _jumpForce / 10;
            StartCoroutine(WaitForLongJump());
        }

        protected void Crouch()
        {
            if (_isCrouched)
            {
                _isCrouched = false;

                ChangeRiderHeight(_riderSize.y, _riderPosition.y);

                return;
            }

            _isCrouched = true;

            var riderCrouchHeight = _riderSize.y * _crouchHeightModifier;
            var riderPosition = _riderPosition.y - (_riderSize.y - riderCrouchHeight);

            ChangeRiderHeight(riderCrouchHeight, riderPosition);
        }

        protected void FastFall()
        {
            _isFastFalling = true;
        }

        private void Awake()
        {
            _moveSpeed = _baseSpeed;
            _maxSpeed = _baseSpeed * 3f;

            _characterController = GetComponent<CharacterController>();
            _wasGrounded = IsGrounded;

            var riderTransform = _rider.transform;
            _riderSize = riderTransform.localScale;
            _riderPosition = riderTransform.localPosition;
        }

        private void Update()
        {
            KeepGrounded();

            if (_wasGrounded != IsGrounded)
            {
                IsGroundedChanged(IsGrounded);
            }

            _wasGrounded = IsGrounded;
        }

        private void FixedUpdate()
        {
            AlterSpeed();
            ApplyGravity();
            ApplyMovement();
            ApplyRotation();
        }

        private void ApplyGravity()
        {
            // Reset velocity and make sure jumping is set to false.
            if (IsGrounded && _velocity < 0.0f)
            {
                _velocity = -0.1f;
            }
            // Apply downwards gravity.
            else
            {
                var gravityEffect = _gravity * _gravityMultiplier * Time.deltaTime;

                if (_isFastFalling)
                {
                    gravityEffect *= 6;
                }

                _velocity += gravityEffect;
            }

            _direction.y = _velocity;
        }

        private void ApplyMovement()
        {
            _characterController.Move(_moveSpeed * Time.deltaTime * _direction);
        }

        private void ApplyRotation()
        {
            var modelEulerAngles = _model.transform.localEulerAngles;
            if (_direction.x == 0.0f && _horizontalRotation != 0.0f)
            {
                var rotationValueY = _rotationSpeed * Time.deltaTime;
                if (_horizontalRotation > _neutralRotationMargin)
                {
                    _horizontalRotation -= rotationValueY;
                }
                else if (_horizontalRotation < -_neutralRotationMargin)
                {
                    _horizontalRotation += rotationValueY;
                }
                else
                {
                    _horizontalRotation = 0;
                }

                _model.transform.localEulerAngles = new Vector3(modelEulerAngles.x, _horizontalRotation, modelEulerAngles.z);

                return;
            }

            var rotationValue = _rotationSpeed * Time.deltaTime * _direction.x;

            if (_direction.z < 0.0f)
            {
                _horizontalRotation -= rotationValue;
            }
            else
            {
                _horizontalRotation += rotationValue;
            }

            // Limit the rotation
            _horizontalRotation = Mathf.Clamp(_horizontalRotation, -_maxRotation, _maxRotation);

            // Set the rotation directly
            _model.transform.localEulerAngles = new Vector3(modelEulerAngles.x, _horizontalRotation, modelEulerAngles.z);
        }

        private void AlterSpeed()
        {
            if (!IsGrounded)
            {
                return;
            }

            if (_direction.x == 0.0f && _direction.z == 0.0f && _moveSpeed > _baseSpeed)
            {
                _moveSpeed = _baseSpeed;

                return;
            }

            if (_isAccelerating && _moveSpeed < _maxSpeed)
            {
                _moveSpeed += _maxSpeed * 0.01f;

                if (_moveSpeed > _maxSpeed)
                {
                    _moveSpeed = _maxSpeed;
                }

                return;
            }

            if (!_isAccelerating && _moveSpeed > _baseSpeed)
            {
                _moveSpeed -= _maxSpeed * 0.01f;

                if (_moveSpeed < _baseSpeed)
                {
                    _moveSpeed = _baseSpeed;
                }

            }
        }

        private IEnumerator WaitForLongJump()
        {
            IsJumping = true;

            // Wait for 1 second
            yield return new WaitForSeconds(0.3f);

            if (!IsJumping)
            {
                yield break;
            }

            ExtendJump();

            IsJumping = false;
        }

        private void ExtendJump()
        {
            Debug.Log("ExtendJump");
            _velocity += (_jumpForce / 3f) / 10;
        }

        private void ChangeRiderHeight(float height, float position)
        {
            _rider.transform.localScale = new Vector3(_riderSize.x, height, _riderSize.z);
            _rider.transform.localPosition = new Vector3(_riderPosition.x, position, _riderPosition.z);
        }

        private void KeepGrounded()
        {
            var ray = new Ray(transform.position, -Vector3.up);

            Debug.DrawRay(ray.origin, ray.direction);

            if (!Physics.Raycast(ray, out RaycastHit hit, _rayCastDistance) || _previousGroundHit.normal.Equals(hit.normal))
            {
                return;
            }

            if (hit.collider.CompareTag("Collectible"))
            {
                return;
            }

            _previousGroundHit = hit;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        private void IsGroundedChanged(bool isGrounded)
        {
            if (!isGrounded)
            {
                _isFastFalling = false;
            }
        }
    }
}