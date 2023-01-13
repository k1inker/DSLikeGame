using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    [SerializeField] private string _bossName;

    private EnemyStatsManager _enemyStats;
    private EnemyAnimatorManager _enemyAnimatorManager;
    private BossCombatStanceState _bossCombatStanceState;
    private UIBossHealthBar _bossHealthBar;

    [Header("Second Phase FX")]
    public GameObject particleFX;
    private void Awake()
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();

        _enemyStats = GetComponent<EnemyStatsManager>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }
    private void Start()
    {
        _bossHealthBar.SetBossName(_bossName);
        _bossHealthBar.SetBossMaxHealth(_enemyStats.maxHealth);
        _bossHealthBar.SetHealthBarToActive();
    }
    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        _bossHealthBar.SetBossCurrentHealth(currentHealth);

        if(currentHealth <= maxHealth / 2 && !_bossCombatStanceState.hasPhaseShifted)
        {
            ShiftToSecondPhase();
        }
    }
    public void ShiftToSecondPhase()
    {
        _enemyAnimatorManager.animator.SetBool("isInvulnerable", true);
        _enemyAnimatorManager.animator.SetBool("isPhaseShifting", true);
        _enemyAnimatorManager.PlayTargetAnimationWithRootMotion("Phase Shift", true);
        _bossCombatStanceState.hasPhaseShifted = true;
    }
}
