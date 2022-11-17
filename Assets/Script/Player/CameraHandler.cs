using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public LayerMask ignoreLayers;

        private InputHandler _inputHandler;
        private Transform _selfTransform;
        private Vector3 _cameraTransformPosition;
        private Vector3 _cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singelton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float _targetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;

        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        public Transform currentLockOnTarget;

        private List<CharacterManager> _availableTargets = new List<CharacterManager>();
        public float maximumLockOnDisctance = 30;
        public Transform nearestLockOnTarget;
        private void Awake()
        {
            singelton = this;
            _selfTransform = transform;
            _defaultPosition = cameraTransform.position.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            _inputHandler =FindObjectOfType<InputHandler>();
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(_selfTransform.position, targetTransform.position, ref _cameraFollowVelocity, delta / followSpeed);
            _selfTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (!_inputHandler.lockOnFlag && currentLockOnTarget == null)
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
                float velocity = 0;

                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;
                
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
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
            float shortestDistance = Mathf.Infinity;
            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 13);

            for(int i =0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if(character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float vievwableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    if(character.transform.root != targetTransform.transform.root
                        && vievwableAngle > -50 && vievwableAngle < 50
                        && distanceFromTarget <= maximumLockOnDisctance)
                    {
                        _availableTargets.Add(character);
                    }
                }
            }
            for(int k = 0; k < _availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, _availableTargets[k].transform.position);
                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = _availableTargets[k].lockOnTransform;
                }
            }
        }
        public void ClearLockOnTargets()
        {
            _availableTargets.Clear();
            currentLockOnTarget = null;
        }
    }
}