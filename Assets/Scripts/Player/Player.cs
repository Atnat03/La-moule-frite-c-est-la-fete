using System;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [Header("Camera Truck")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _truckCamera;
        
        PlayerInputs _inputs;
        
        void Awake()
        {
            _inputs = new PlayerInputs();
        }

        private void Start()
        {
            SwitchCamera(true);
        }

        void OnEnable()
        {
            _inputs.Enable();
            
            _inputs.Gameplay.SwitchCamera.performed += ctx => SwitchCamera(false);
            _inputs.Gameplay.SwitchCamera.canceled += ctx => SwitchCamera(true);
            
        }

        private void SwitchCamera(bool isMain)
        {
            _mainCamera.gameObject.SetActive(isMain);
            _truckCamera.gameObject.SetActive(!isMain);
        }
    }
}