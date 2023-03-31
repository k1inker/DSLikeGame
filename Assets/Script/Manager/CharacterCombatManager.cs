using UnityEngine;

namespace DS
{
    public class CharacterCombatManager : MonoBehaviour
    {
        private CharacterManager _character;
        private AudioManager _audioManager;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Attack Animations")]
        public string OH_Light_Attack_1 = "Light_Attack_01";
        public string OH_Light_Attack_2 = "Light_Attack_02";
        public string OH_Heavy_Attack_1 = "Heavy_Attack_01";
        public string OH_Heavy_Attack_2 = "Heavy_Attack_02";

        [Header("Team ID")]
        public int teamID = 0;

        [Header("Explosive settings")]
        public float radius = 0.0f;
        public float explosiveDamage = 0.0f;

        public string lastAttack;
        protected virtual void Awake()
        {
            _character = GetComponent<CharacterManager>();
            _audioManager = FindObjectOfType<AudioManager>();
        }
        public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
        {
            if (_character.isUsingRightHand)
            {
                _character.characterStatsManager.blockingStabilityRating = _character.characterWeaponSlotManager.rightWeapon.stability;
            }
            else if (_character.isUsingLeftHand)
            {
                _character.characterStatsManager.blockingStabilityRating = _character.characterWeaponSlotManager.leftWeapon.stability;
            }
        }
        public virtual void DrainStaminaBasedOnAttack()
        {
            //if you want AI to lose Stamina during attacks
        }
        public virtual void AttemptBlock(DamageCollider attackingWeapon, float damage, string blockAnimation)
        {
            float staminaDamageAbsorption = (damage * attackingWeapon.guardBreakModifier) * (_character.characterStatsManager.blockingStabilityRating / 100);
            float staminaDamage = damage * attackingWeapon.guardBreakModifier - staminaDamageAbsorption;

            _character.characterStatsManager.DeductStamina(staminaDamage);
            if (_character.characterStatsManager.currentStamina <= 0)
            {
                _character.isBlocking = false;
                _character.characterAnimatorManager.PlayTargetAnimationWithRootMotion("Destroy Block Guard", true);
                if (_character as EnemyManager is EnemyManager)
                {
                    _character.characterStatsManager.currentStamina = _character.characterStatsManager.maxStamina;
                }
            }
            else
            {
                _character.characterAnimatorManager.PlayTargetAnimationWithRootMotion(blockAnimation, true);
            }
        }
        public void EnableCanBeParried()
        {
            _character.canBeParried = true;
        }
        public void DisableCanBeParried()
        {
            _character.canBeParried = false;
        }
        public void ExplosionDamage()
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, radius);
            _audioManager.Play("Explosion");
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Ground")
                {
                    _character.characterEffectsManager.PlaySandDustFX(this.gameObject.transform.position);
                }
                if (hitCollider.tag == "Character")
                {
                    CharacterManager enemyCharacter = hitCollider.GetComponent<CharacterManager>();

                    if (enemyCharacter.characterStatsManager.teamIDNumber == teamID)
                        return;
                    if (enemyCharacter.isInvulnerable)
                        return;

                    enemyCharacter.characterStatsManager.TakeDamage(Mathf.RoundToInt(explosiveDamage), "Falling");
                    enemyCharacter.characterStatsManager.currentPoiseDefence = enemyCharacter.characterStatsManager.totalPoiseDefence;
                }
            }
        }
    }
}
