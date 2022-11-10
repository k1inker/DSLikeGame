using UnityEngine;

namespace SG
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private Transform _cameraObject;
        private InputHandler _inputHandler;
        public Vector3 moveDirection;

        private Transform _selfTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        private float _groundDetectionRayStartPoint = 0.3f;
        [SerializeField]
        private float _minimumDistanceNeededToFall = 1f;
        [SerializeField]
        private float _groundDirectionRayDistance = 0.2f;
        private LayerMask _ignoreGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        private float _movementSpeed = 5;
        [SerializeField]
        private float _rotationSpeed = 10;
        [SerializeField]
        private float _fallingSpeed = 75;

        void Start()
        {
            _playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _cameraObject = Camera.main.transform;
            _selfTransform = transform;
            animatorHandler.Initialize();

            _playerManager.isGrounded = true;
            _ignoreGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement 
        private Vector3 _normalVector;
        private Vector3 _targetPosition;

        public void HandelMovement(float delta)
        {
            if (!animatorHandler.canMove)
            {
                rigidbody.velocity = Vector3.zero;
                return;
            }
            if (_inputHandler.rollFlag)
                return;
            if (_playerManager.isInteracting)
                return;
            setMoveDirection();

            rigidbody.velocity = ProjectedVelocity(_movementSpeed);

            animatorHandler.UpdateAnimatorValues(_inputHandler.moveAmount, 0);
            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }


        public void HandleAttackMovement(float delta)
        {
            if (_inputHandler.rollFlag || !animatorHandler.canMove)
                return;
            if (_playerManager.isInteracting)
                return;
            if (_inputHandler.attackFlag)
            {
                setMoveDirection();

                float attackSpeedMove = _movementSpeed * 0.5f;
                moveDirection = _selfTransform.forward;
                rigidbody.velocity = ProjectedVelocity(attackSpeedMove);
            }
        }

        public void HandleRolling(float delta)
        {
            if (_playerManager.isInteracting || !animatorHandler.canMove)
                return;

            if(_inputHandler.rollFlag)
            {
                setMoveDirection();

                animatorHandler.PlayTargetAnimation("Rolling", true);
                float rollSpeed = _movementSpeed * 1.3f;
                Quaternion rotation;
                if (moveDirection != Vector3.zero)
                {
                    rotation = Quaternion.LookRotation(moveDirection);
                    rigidbody.velocity = ProjectedVelocity(rollSpeed);
                    _selfTransform.rotation = rotation;
                }
                else
                {
                    moveDirection = _selfTransform.forward;

                    rigidbody.velocity = ProjectedVelocity(rollSpeed);
                }

            }
        }


        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = _inputHandler.moveAmount;

            targetDir = _cameraObject.forward * _inputHandler.vertical;
            targetDir += _cameraObject.right *  _inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = _selfTransform.forward;

            float rs = _rotationSpeed;

            Quaternion buffTransform = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(_selfTransform.rotation, buffTransform, rs * delta);

            _selfTransform.rotation = targetRotation;
        }
        public void HadleFalling(float delta, Vector3 moveDirection)
        {
            _playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = _selfTransform.position;
            origin.y += _groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, _selfTransform.forward, out hit, 0.4f))
                moveDirection = Vector3.zero;

            if (_playerManager.isFalling)
            {
                rigidbody.AddForce(Vector3.down * _fallingSpeed);
                rigidbody.AddForce(moveDirection * _fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * _groundDirectionRayDistance;

            _targetPosition = _selfTransform.position;

            Debug.DrawRay(origin, Vector3.down * _minimumDistanceNeededToFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, Vector3.down, out hit, _minimumDistanceNeededToFall, _ignoreGroundCheck))
            {
                _normalVector = hit.normal;

                _playerManager.isGrounded = true;

                _targetPosition.y = hit.point.y;
                if (_playerManager.isFalling)
                {
                    if (inAirTimer > 0.5f)
                    {
                        //animation приземления EP7
                                
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                    }
                    inAirTimer = 0;
                    _playerManager.isFalling = false;
                }
            }
            else
            {
                if (_playerManager.isGrounded)
                    _playerManager.isGrounded = false;

                if (_playerManager.isFalling == false)
                {
                    if (_playerManager.isInteracting == false)
                    {     //animation falling
                    }

                    Vector3 velocity = rigidbody.velocity;
                    velocity.Normalize();
                    rigidbody.velocity = velocity * (_movementSpeed / 2);
                    _playerManager.isFalling = true;
                }
            }
            _selfTransform.position = _targetPosition;
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