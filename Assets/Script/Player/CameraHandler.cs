using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DS
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public LayerMask ignoreLayers;

        private PlayerManager _playerManager;
        private Transform _selfTransform;
        private Vector3 _cameraTransformPosition;
        private Vector3 _cameraFollowVelocity = Vector3.zero;
        private LayerMask _enviromentLayer;

        public static CameraHandler singelton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float _targetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;

        [SerializeField] private float minimumPivot = -35;
        [SerializeField] private float maximumPivot = 35;

        [SerializeField] private float cameraSphereRadius = 0.2f;
        [SerializeField] private float cameraCollisionOffset = 0.2f;
        [SerializeField] private float minimumCollisionOffset = 0.2f;
        [SerializeField] private float lockedPivotRotationX = 12;
        [SerializeField] private float lockedPivotPosition = 2.25f;
        [SerializeField] private float unlockedPivotPosition = 1.65f;

        public CharacterManager currentLockOnTarget;

        private List<CharacterManager> _availableTargets = new List<CharacterManager>();
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public CharacterManager nearestLockOnTarget;
        public float maximumLockOnDisctance = 30;
        private void Awake()
        {
            singelton = this;
            _selfTransform = transform;
            _defaultPosition = cameraTransform.position.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10 | 1 << 11 | 1 << 12);
            _playerManager = GetComponentInParent<PlayerManager>();
            targetTransform = _playerManager.transform;
        }
        private void Start()
        {
            _enviromentLayer = LayerMask.NameToLayer("Enviroment");
        }
        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(_selfTransform.position, targetTransform.position, ref _cameraFollowVelocity, delta / followSpeed);
            _selfTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (!_playerManager.inputHandler.lockOnFlag && currentLockOnTarget == null)
            {
                _lookAngle -= mouseXInput * lookSpeed * delta;
                _pivotAngle += mouseYInput * pivotSpeed * delta;
                _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = _lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                _selfTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = _pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                Vector3 dir = currentLockOnTarget.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;
                
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                dir.Normalize();

                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                eulerAngle.x = lockedPivotRotationX;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }  

        private void HandleCameraCollisions(float delta)
        {
            _targetPosition = _defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if(Physics.SphereCast
                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(_targetPosition), ignoreLayers))
            {
                float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                _targetPosition = -(distance - cameraCollisionOffset);
            }

            if(Mathf.Abs(_targetPosition) < minimumCollisionOffset)
            {
                _targetPosition -= minimumCollisionOffset;
            }

            _cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, _targetPosition, delta / 0.2f);
            cameraTransform.localPosition = _cameraTransformPosition;
        }
        public void HandleLockOn()
        {
            _availableTargets.Clear();

            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 13);

            for(int i =0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if(character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float vievwableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if(character.transform.root != targetTransform.transform.root
                        && vievwableAngle > -70 && vievwableAngle < 70
                        && distanceFromTarget <= maximumLockOnDisctance)
                    {
                        if (Physics.Linecast(_playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            if(hit.transform.gameObject.layer != _enviromentLayer)
                                _availableTargets.Add(character);
                        }
                    }
                }
            }
            for(int k = 0; k < _availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, _availableTargets[k].transform.position);
                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = _availableTargets[k];
                }

                if(_playerManager.inputHandler.lockOnFlag)
                {
                    Vector3 relativeEnemyPosition = _playerManager.inputHandler.transform.InverseTransformDirection(_availableTargets[k].transform.position);
                    float distanceFromLeftTarget = relativeEnemyPosition.x;
                    float distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget
                        && _availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = _availableTargets[k];
                    }
                    else  if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                        && _availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = _availableTargets[k];
                    }

                }
            }
        }
        public void ClearLockOnTargets()
        {
            _availableTargets.Clear();
            currentLockOnTarget = null;
        }
        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);
            if(currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}