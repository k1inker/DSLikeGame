using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Windows;

namespace SG
{
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private PlayerLocomotion _playerLocomotion;
        private Animator _anim;
        private CameraHandler _cameraHandler;
        private InteractableUI _interactableUI;
        
        public GameObject interactableUIGameObject;

        public bool isInteracting;

        [Header("Player flag")]
        public bool isFalling;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isAttack;
        void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _anim = GetComponentInChildren<Animator>();
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _interactableUI = FindObjectOfType<InteractableUI>();
        }
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
            }
        }
        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = _anim.GetBool("isInteracting");
            canDoCombo = _anim.GetBool("canDoCombo");

            _inputHandler.rollFlag = false;
            _inputHandler.attackFlag = false;
            
            CheckForInteractableObject();

            _inputHandler.TickInput(delta);
            _playerLocomotion.HandelMovement(delta);
            _playerLocomotion.HadleFalling(delta, _playerLocomotion.moveDirection);
            _playerLocomotion.HandleRolling(delta);
            _playerLocomotion.HandleAttackMovement(delta);

        }
        private void LateUpdate()
        {
            _inputHandler.space_Input = false;
            _inputHandler.rb_Input = false;
            _inputHandler.lb_Input = false;
            _inputHandler.a_Input = false;
            if (isFalling)
            {
                _playerLocomotion.inAirTimer = _playerLocomotion.inAirTimer + Time.deltaTime;
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