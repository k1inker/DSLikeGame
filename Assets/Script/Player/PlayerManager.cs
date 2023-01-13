using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class PlayerManager : CharacterManager 
    {
        private InputHandler _inputHandler;
        private PlayerLocomotionManager _playerLocomotionManager;
        private Animator _animator;
        private CameraHandler _cameraHandler;
        private InteractableUI _interactableUI;
        private PlayerStatsManager _playerStatsManager;
        private PlayerAnimatorManager _playerAnimatorManager;
        
        public GameObject interactableUIGameObject;

        private void Awake()
        {
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _interactableUI = FindObjectOfType<InteractableUI>();
            interactableUIGameObject = _interactableUI.transform.GetChild(0).gameObject;

            _inputHandler = GetComponent<InputHandler>();
            _animator = GetComponent<Animator>();
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            _playerStatsManager = GetComponent<PlayerStatsManager>();
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }
        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = _animator.GetBool("isInteracting");
            canDoCombo = _animator.GetBool("canDoCombo");
            isUsingLeftHand = _animator.GetBool("isUsingLeftHand");
            isUsingRightHand = _animator.GetBool("isUsingRightHand");
            isInvulnerable = _animator.GetBool("isInvulnerable");
            _playerAnimatorManager.canRotate = _animator.GetBool("canRotate");

            _animator.SetBool("isDead", _playerStatsManager.isDead);

            _inputHandler.rollFlag = false;

            _inputHandler.TickInput(delta);
            _playerLocomotionManager.HandleRolling(delta);
            _playerStatsManager.RegenerateStamina();
            
            CheckForInteractableObject();   
        }
        private void FixedUpdate()
        {
            float delta = Time.deltaTime;
            _playerLocomotionManager.HandelMovement(delta);
            _playerLocomotionManager.HandleRotation(delta);
        }
        private void LateUpdate()
        {
            float delta = Time.deltaTime;
            _inputHandler.space_Input = false;
            _inputHandler.rb_Input = false;
            _inputHandler.lb_Input = false;
            _inputHandler.a_Input = false;
            if (_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
            }
        }

        public void CheckForInteractableObject()
        {
            RaycastHit hit;
            if(Physics.SphereCast(transform.position, 0.3f,transform.forward, out hit,1f, _cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        _interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);
                        if (_inputHandler.a_Input)
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