using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Item
{
    public class PlayerInteractBehavior : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera _camera;
        PlayerInputs _playerInputs;
        
        private Transform _currentTarget = null;
        BasePickableItem _pickableItem;
        
        void OnEnable()
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Gameplay.LeftClick.performed += OnPerformLeftClick;            
            _playerInputs.Gameplay.LeftClick.canceled += OnCancelLeftClick;            
            
            _playerInputs.Enable();
        }

        private void OnDisable()
        {
            _playerInputs.Gameplay.LeftClick.performed -= OnPerformLeftClick;            
            _playerInputs.Gameplay.LeftClick.canceled -= OnCancelLeftClick;  
            
            _playerInputs.Disable();
        }
        
        #region Actions
        
        private void OnPerformLeftClick(InputAction.CallbackContext input)
        {
            if (_currentTarget != null) return;
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _currentTarget = hit.transform;
                
                if (_currentTarget.TryGetComponent(out IInteractable interactable))
                {
                    interactable.StartInteract();
                }
            }
        }
        
        private void OnCancelLeftClick(InputAction.CallbackContext input)
        {
            if (_currentTarget == null) return;
            
            if (_currentTarget.TryGetComponent(out IInteractable interactable))
            {
                interactable.StopInteract();
            }
            
            _currentTarget = null;
        }
        
        #endregion
        
        
        
        
        public bool HasItemInHand => _currentTarget != null;

    }
}