using UnityEngine;

namespace DS
{
    public class PlayerManager : CharacterManager 
    {
        [Header("Player")]
        public InputHandler inputHandler;
        public PlayerStatsManager playerStatsManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerLocomotionManager playerLocomotionManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;

        [Header("Camera")]
        public CameraHandler cameraHandler;

        [Header("UI")]
        public UIManager uiManager;

        [Header("Ads")]
        public InterAds interAds;
        protected override void Awake()
        {
            base.Awake();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            inputHandler = GetComponent<InputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            cameraHandler = GetComponentInChildren<CameraHandler>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            
            uiManager = GetComponentInChildren<UIManager>();
            interAds = GetComponent<InterAds>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isInvulnerable = animator.GetBool("isInvulnerable");
            canRotate = animator.GetBool("canRotate");

            animator.SetBool("isDead", isDead);
            animator.SetBool("isBlocking", isBlocking);

            inputHandler.rollFlag = false;

            inputHandler.TickInput();
            playerLocomotionManager.HandleRolling(delta);
            playerStatsManager.RegenerateStamina();
        }
        private void FixedUpdate()
        {
            float delta = Time.deltaTime;
            playerLocomotionManager.HandelMovement(delta);
            playerLocomotionManager.HandleRotation(delta);
        }
        private void LateUpdate()
        {
            float delta = Time.deltaTime;
            inputHandler.space_Input = false;
            inputHandler.tap_rb_Input = false;
            inputHandler.tap_lb_Input = false;
            inputHandler.hold_rb_Input = false;
            inputHandler.a_Input = false;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation();
            }
        }
    }
}