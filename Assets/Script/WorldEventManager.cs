using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public UIBossHealthBar bossHealthBar;

    private void Awake()
    {
        bossHealthBar = FindObjectOfType<UIBossHealthBar>();
    }
    public void ActiveBossFight()
    {
        bossHealthBar.SetHealthBarToActive();
    }
    public void BossHasDefeated()
    {

    }
}
