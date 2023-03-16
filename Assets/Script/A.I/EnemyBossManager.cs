using UnityEngine;

namespace DS
{
    public class EnemyBossManager : MonoBehaviour
    {
        [SerializeField] private string _bossName;
        [SerializeField] private float radiusExplosionAttack = 0.0f;
        [SerializeField] private float damageExplosionAttack = 0.0f;

        private EnemyManager _enemy;
        private BossCombatStanceState _bossCombatStanceState;
        private UIManager bossUI;

        [Header("Second Phase FX")]
        public GameObject particleFX;
        private void Awake()
        {
            bossUI = FindObjectOfType<UIManager>();

            _enemy = GetComponent<EnemyManager>();
            _bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }
        private void Start()
        {
            bossUI.bossHealthBar.SetBossName(_bossName);
            bossUI.bossHealthBar.SetBossMaxHealth(_enemy.enemyStatsManager.maxHealth);

            _enemy.characterCombatManager.teamID = _enemy.characterStatsManager.teamIDNumber;
            _enemy.characterCombatManager.radius = radiusExplosionAttack;
            _enemy.characterCombatManager.explosiveDamage = damageExplosionAttack;
            _enemy.enemyAnimatorManager.PlayTargetAnimationWithRootMotion("Spawn Boss", true);
        }
        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            bossUI.bossHealthBar.SetBossCurrentHealth(currentHealth);

            if (currentHealth <= maxHealth / 2 && !_bossCombatStanceState.hasPhaseShifted)
            {
                ShiftToSecondPhase();
            }
        }
        public void ShiftToSecondPhase()
        {
            _enemy.animator.SetBool("isInvulnerable", true);
            _enemy.animator.SetBool("isPhaseShifting", true);
            _enemy.enemyAnimatorManager.PlayTargetAnimationWithRootMotion("Phase Shift", true);
            _bossCombatStanceState.hasPhaseShifted = true;
        }
    }
}