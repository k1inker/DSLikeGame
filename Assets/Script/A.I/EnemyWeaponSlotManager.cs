using UnityEngine;

namespace DS
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public override void GrantWeaponAttackingPoiseBonus()
        {
            _character.characterStatsManager.currentPoiseDefence = _character.characterStatsManager.currentPoiseDefence + _character.characterStatsManager.offensivePoiseBonus;
        }
        public override void ResetWeaponAttackingPoiseBonus()
        {
            _character.characterStatsManager.currentPoiseDefence = _character.characterStatsManager.totalPoiseDefence;
        }
    }
    
}