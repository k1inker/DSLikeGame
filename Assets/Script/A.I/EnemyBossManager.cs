using UnityEngine;

namespace DS
{
    public class EnemyBossManager : MonoBehaviour
    {
        [SerializeField] private string _bossName;

        private EnemyManager _enemy;
        private BossCombatStanceState _bossCombatStanceState;
        private UIBossHealthBar _bossHealthBar;

        [Header("Second Phase FX")]
        public GameObject particleFX;
        private void Awake()
        {
            _bossHealthBar = FindObjectOfType<UIBossHealthBar>();

            _enemy = GetComponent<EnemyManager>();
            _bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }
        private void Start()
        {
            _bossHealthBar.SetBossName(_bossName);
            _bossHealthBar.SetBossMaxHealth(_enemy.enemyStatsManager.maxHealth);
            _bossHealthBar.SetHealthBarToActive();
        }
        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            _bossHealthBar.SetBossCurrentHealth(currentHealth);

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