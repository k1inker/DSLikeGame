using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DS
{
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
}
