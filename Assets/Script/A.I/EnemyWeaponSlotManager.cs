using UnityEngine;

namespace DS
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public override void GrantWeaponAttackingPoiseBonus()
        {
            _characterStatsManager.currentPoiseDefence = _characterStatsManager.currentPoiseDefence + _characterStatsManager.offensivePoiseBonus;
        }
        public override void ResetWeaponAttackingPoiseBonus()
        {
            _characterStatsManager.currentPoiseDefence = _characterStatsManager.totalPoiseDefence;
        }
    }
    
}