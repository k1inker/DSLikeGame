using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager _enemyManager;

        public CharacterStats currentTarget;
        [SerializeField] private LayerMask _detectionLayer;

        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
        }
        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _enemyManager.detectionRadius);
            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].GetComponent<CharacterStats>();
                if(characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if(viewableAngle > _enemyManager.minimumDetectionAngle && viewableAngle < _enemyManager.maximumDetectionAngle)
                    {
                        currentTarget = characterStats;   
                    }    
                }
            }
        }
    }
}