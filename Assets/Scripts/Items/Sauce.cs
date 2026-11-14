using Data;
using Item.Interfaces;
using MyPrint;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Item
{
    public class Sauce : BasePickableItem
    {
        [SerializeField] private SauceFrite _sauceFrite;
        [SerializeField] private SauceMoule _sauceMoule;
        
        [SerializeField] private GameObject _saucePrefab;
        [SerializeField] private Transform _sauceTransformSpawn;

        [Header("Rotate")] 
        [SerializeField] private float _rotateStep = 5;
        float _rotateAngle;
        
        PlayerInputs _playerInputs;
        
        void OnEnable()
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Enable();

            _playerInputs.Gameplay.Scroll.performed += UpdateRotation;
            _playerInputs.Gameplay.RightClick.performed += AddSauce;
        }
        
        private void OnDisable()
        {
            _playerInputs.Gameplay.Scroll.performed -= UpdateRotation;
            _playerInputs.Gameplay.RightClick.performed -= AddSauce;

            _playerInputs.Disable();
        }

        
        private void UpdateRotation(InputAction.CallbackContext obj)
        {
            if(!_isInHand)
                return;
        
            float d = obj.ReadValue<float>();
        
            _rotateAngle = (_rotateAngle + _rotateStep * d) % 360f;
            transform.rotation = Quaternion.Euler(0f, 0f, _rotateAngle);
        }
        
        private void AddSauce(InputAction.CallbackContext obj)
        {
            if (_isInHand)
            {
                Vector3 startPos = _sauceTransformSpawn.position;
                Vector3 direction = _sauceTransformSpawn.up;

                if (Physics.Raycast(startPos, direction, out RaycastHit hit))
                {
                    if (hit.transform.TryGetComponent(out Barquette b))
                    {
                        if(_sauceFrite != SauceFrite.Empty)
                            b.AddSauceFriteToCommande(_sauceFrite);
                        
                        if(_sauceMoule != SauceMoule.Empty)
                            b.AddSauceMouleToCommande(_sauceMoule);
                        
                        Instantiate(_saucePrefab, hit.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}