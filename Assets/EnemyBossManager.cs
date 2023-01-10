using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    [SerializeField] private string _bossName;

    private EnemyStats _enemyStats;
    private EnemyAnimatorManager _enemyAnimatorManager;
    private BossCombatStanceState _bossCombatStanceState;

    public  UIBossHealthBar bossHealthBar;

    [Header("Second Phase FX")]
    public GameObject particleFX;
    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        _enemyStats = GetComponent<EnemyStats>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }
    private void Start()
    {
        bossHealthBar.SetBossName(_bossName);
        bossHealthBar.SetBossMaxHealth(_enemyStats.maxHealth);
    }
    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);

        if(currentHealth <= maxHealth / 2 && !_bossCombatStanceState.hasPhaseShifted)
        {
            _enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            ShiftToSecondPhase();
        }
    }
    public void ShiftToSecondPhase()
    {
        _enemyAnimatorManager.animator.SetBool("isInvulnerable", true);
        _enemyAnimatorManager.animator.SetBool("isPhaseShifting", true);
        _enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        _bossCombatStanceState.hasPhaseShifted = true;
    }
}
