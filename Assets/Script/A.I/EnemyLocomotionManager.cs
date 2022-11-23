using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace DS
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager _enemyManager;
        private EnemyAnimatorManager _enemyAnimatorManager;
        private NavMeshAgent _navmeshAgent;
        

        public Rigidbody enemyRigidbody;
        public float distanceFromTarget;
        public float stoppingDistance = 1f;

        public float rotationSpeed = 15;
        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            enemyRigidbody = GetComponent<Rigidbody>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            _navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        }
        private void Start()
        {
            _navmeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }
        public void HandleMoveToTarget()
        {
            if (_enemyManager.isPerformingAction)
                return;

            Vector3 targetDirection = _enemyManager.currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(_enemyManager.currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if(_enemyManager.isPerformingAction)
            {
                _enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                _navmeshAgent.enabled = false;
            }
            else
            {
                if(distanceFromTarget > stoppingDistance)
                {
                    _enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    _enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }
            }
            HandleRotateTowardsTarget();
            _navmeshAgent.transform.localPosition = Vector3.zero;
            _navmeshAgent.transform.localRotation = Quaternion.identity;
        }
        private void HandleRotateTowardsTarget()
        {
            //Rotate manualy
            if(_enemyManager.isPerformingAction)
            {
                Vector3 direction = _enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
            //Rotate with navmesh
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(_navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyRigidbody.velocity;

                _navmeshAgent.enabled = true;
                _navmeshAgent.SetDestination(_enemyManager.currentTarget.transform.position);
                enemyRigidbody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, _navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }
    }
}