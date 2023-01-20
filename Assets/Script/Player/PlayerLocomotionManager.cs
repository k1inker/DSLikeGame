using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace DS
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        private PlayerManager _player;

        public Vector3 moveDirection;
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
            _player = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollied, true);
        }

        #region Movement 
        private Vector3 _normalVector;
        private Vector3 _targetPosition;
        public void HandelMovement(float delta)
        {
            if (_player.inputHandler.rollFlag)
                return;
            if (_player.isInteracting)
                return;
            setMoveDirection();

            if (_player.inputHandler.moveAmount <= 0.52)
            {
                rigidbody.velocity = ProjectedVelocity(_movementSpeed / 2);
            }
            else
            {
                rigidbody.velocity = ProjectedVelocity(_movementSpeed);
            }

            if (_player.inputHandler.lockOnFlag)
                _player.playerAnimatorManager.UpdateAnimatorValues(_player.inputHandler.vertical, _player.inputHandler.horizontal);
            else
                _player.playerAnimatorManager.UpdateAnimatorValues(_player.inputHandler.moveAmount, 0);
        }

        public void HandleRolling(float delta)
        {
            if (_player.isInteracting)
                return;
            if (_player.playerStatsManager.currentStamina < _rollStaminaCost)
                return;

            if(_player.inputHandler.rollFlag)
            {
                setMoveDirection();

                _player.playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                float rollSpeed = _movementSpeed * 2f;
                Quaternion rotation;
                if (moveDirection != Vector3.zero)
                {
                    rotation = Quaternion.LookRotation(moveDirection);
                    rigidbody.velocity = ProjectedVelocity(rollSpeed);
                    _player.transform.rotation = rotation;
                    _player.playerStatsManager.DeductStamina(_rollStaminaCost);
                }
                else
                {
                    moveDirection = _player.transform.forward;

                    rigidbody.velocity = ProjectedVelocity(rollSpeed);
                }

            }
        }
        public void HandleRotation(float delta)
        {
            if (_player.canRotate)
            {
                if (_player.inputHandler.lockOnFlag)
                {
                    if (_player.inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = Vector2.zero;
                        targetDirection = _player.cameraHandler.cameraTransform.forward * _player.inputHandler.vertical;
                        targetDirection = _player.cameraHandler.cameraTransform.right * _player.inputHandler.horizontal;
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
                        rotationDirection = _player.cameraHandler.currentLockOnTarget.transform.position - transform.position;
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
                    float moveOverride = _player.inputHandler.moveAmount;

                    targetDir = _player.cameraHandler.cameraTransform.forward * _player.inputHandler.vertical;
                    targetDir += _player.cameraHandler.cameraTransform.right * _player.inputHandler.horizontal;

                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                        targetDir = _player.transform.forward;

                    float rs = _rotationSpeed;

                    Quaternion buffTransform = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(_player.transform.rotation, buffTransform, rs * delta);
                    _player.transform.rotation = targetRotation;
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
            moveDirection = _player.cameraHandler.cameraTransform.forward * _player.inputHandler.vertical;
            moveDirection += _player.cameraHandler.cameraTransform.right * _player.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;
        }
    }
}