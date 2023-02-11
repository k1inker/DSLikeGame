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

        [Header("Interactable")]
        private InteractableUI _interactableUI;
        public GameObject interactableUIGameObject;

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
            _interactableUI = GetComponentInChildren<InteractableUI>();
            interactableUIGameObject = _interactableUI.transform.GetChild(0).gameObject;
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
            
            CheckForInteractableObject();   
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
            inputHandler._space_Input = false;
            inputHandler.tap_rb_Input = false;
            inputHandler.tap_lb_Input = false;
            inputHandler.hold_rb_Input = false;
            inputHandler.a_Input = false;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;
            if(Physics.SphereCast(transform.position, 0.3f,transform.forward, out hit,1f, cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        _interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);
                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }
            }
        }
    }
}