using UnityEngine;

namespace DS
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private Transform _cameraObject;
        private InputHandler _inputHandler;
        private CameraHandler _cameraHandler;
        private PlayerStatsManager _playerStatsManager;

        public Vector3 moveDirection;
        private Transform _selfTransform;

        private PlayerAnimatorManager _playerAnimatorManager;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Movement Stats")]
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _rotationSpeed = 10;

        [SerializeField] private CapsuleCollider _characterCollider;
        [SerializeField] private CapsuleCollider _characterCollisionBlockerCollied;

        [Header("Stamina cost")]
        [SerializeField] private float _rollStaminaCost = 15f;
        private void Awake()
        {
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _playerManager = GetComponent<PlayerManager>();
            _inputHandler = GetComponent<InputHandler>();
            _playerStatsManager = GetComponent<PlayerStatsManager>();
            rigidbody = GetComponent<Rigidbody>();
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }
        private void Start()
        {
            
            _cameraObject = Camera.main.transform;
            _selfTransform = transform;
            _playerAnimatorManager.Initialize();

            Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollied, true);
        }

        #region Movement 
        private Vector3 _normalVector;
        private Vector3 _targetPosition;
        public void HandelMovement(float delta)
        {
            if (_inputHandler.rollFlag)
                return;
            if (_playerManager.isInteracting)
                return;
            setMoveDirection();

            if (_inputHandler.moveAmount <= 0.52)
            {
                rigidbody.velocity = ProjectedVelocity(_movementSpeed / 2);
            }
            else
            {
                rigidbody.velocity = ProjectedVelocity(_movementSpeed);
            }

            if (_inputHandler.lockOnFlag)
                _playerAnimatorManager.UpdateAnimatorValues(_inputHandler.vertical, _inputHandler.horizontal);
            else
                _playerAnimatorManager.UpdateAnimatorValues(_inputHandler.moveAmount, 0);
        }

        public void HandleRolling(float delta)
        {
            if (_playerManager.isInteracting)
                return;
            if (_playerStatsManager.currentStamina < _rollStaminaCost)
                return;

            if(_inputHandler.rollFlag)
            {
                setMoveDirection();

                _playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                float rollSpeed = _movementSpeed * 2f;
                Quaternion rotation;
                if (moveDirection != Vector3.zero)
                {
                    rotation = Quaternion.LookRotation(moveDirection);
                    rigidbody.velocity = ProjectedVelocity(rollSpeed);
                    _selfTransform.rotation = rotation;
                    _playerStatsManager.TakeStaminaDamage(_rollStaminaCost);
                }
                else
                {
                    moveDirection = _selfTransform.forward;

                    rigidbody.velocity = ProjectedVelocity(rollSpeed);
                }

            }
        }
        public void HandleRotation(float delta)
        {
            if (_playerAnimatorManager.canRotate)
            {
                if (_inputHandler.lockOnFlag)
                {
                    if (_inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = Vector2.zero;
                        targetDirection = _cameraHandler.cameraTransform.forward * _inputHandler.vertical;
                        targetDirection = _cameraHandler.cameraTransform.right * _inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }
                        Quaternion buff = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, buff, _rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = _cameraHandler.currentLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();

                        Quaternion buff = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, buff, _rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    Vector3 targetDir = Vector3.zero;
                    float moveOverride = _inputHandler.moveAmount;

                    targetDir = _cameraObject.forward * _inputHandler.vertical;
                    targetDir += _cameraObject.right * _inputHandler.horizontal;

                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                        targetDir = _selfTransform.forward;

                    float rs = _rotationSpeed;

                    Quaternion buffTransform = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(_selfTransform.rotation, buffTransform, rs * delta);
                    _selfTransform.rotation = targetRotation;
                }
            }
        }
        #endregion
        private Vector3 ProjectedVelocity(float speed)
        {
            moveDirection *= speed;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, _normalVector);
            return projectedVelocity;
        }
        private void setMoveDirection()
        {
            moveDirection = _cameraObject.forward * _inputHandler.vertical;
            moveDirection += _cameraObject.right * _inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;
        }
    }
}