using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    [SerializeField] private string _bossName;

    private UIBossHealthBar _bossHealthBar;
    private EnemyStats _enemyStats;

    private void Awake()
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        _enemyStats = GetComponent<EnemyStats>();
    }
    private void Start()
    {
        _bossHealthBar.SetBossName(_bossName);
        _bossHealthBar.SetBossMaxHealth(_enemyStats.maxHealth);
    }
}
