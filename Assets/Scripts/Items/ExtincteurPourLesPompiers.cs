using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Item
{
    public class ExtincteurPourLesPompiers : BasePickableItem
    {
        [SerializeField] private ParticleSystem _particleMousse;
        
        PlayerInputs _playerInputs;
        
        public static event Action fireOff;

        void OnEnable()
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Enable();

            _playerInputs.Gameplay.RightClick.performed += ApplyMousse;
        }
        
        private void OnDisable()
        {
            _playerInputs.Gameplay.RightClick.performed -= ApplyMousse;

            _playerInputs.Disable();
        }

        private void ApplyMousse(InputAction.CallbackContext obj)
        {
            if (_isInHand)
            {
                if(_particleMousse.isPlaying) return;
                
                _particleMousse.Play();
                fireOff?.Invoke();
                
                Destroy(gameObject, _particleMousse.main.duration);
            }
        }
    }
}