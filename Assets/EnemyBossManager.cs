using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    [SerializeField] private string _bossName;

    private UIBossHealthBar _bossHealthBar;
    private EnemyStats _enemyStats;
    private EnemyAnimatorManager _enemyAnimatorManager;
    private BossCombatStanceState _bossCombatStanceState;

    [Header("Second Phase FX")]
    public GameObject particleFX;
    private void Awake()
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        _enemyStats = GetComponent<EnemyStats>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }
    private void Start()
    {
        _bossHealthBar.SetBossName(_bossName);
        _bossHealthBar.SetBossMaxHealth(_enemyStats.maxHealth);
    }
    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        _bossHealthBar.SetBossCurrentHealth(currentHealth);

        if(currentHealth <= maxHealth / 2 && !_bossCombatStanceState.hasPhaseShifted)
        {
            _enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            ShiftToSecondPhase();
        }
    }
    public void ShiftToSecondPhase()
    {
        _enemyAnimatorManager.anim.SetBool("isInvulnerable", true);
        _enemyAnimatorManager.anim.SetBool("isPhaseShifting", true);
        _enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        _bossCombatStanceState.hasPhaseShifted = true;
    }
}
