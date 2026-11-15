using System;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player instance;
        
        [Header("Camera Truck")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _truckCamera;

        [Header("Drunk")] 
        [SerializeField] private float _drunkDuration = 10;
        [SerializeField] private float _drunkMagnitude = 0.1f;
        
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        public int Score = 0;
        
        PlayerInputs _inputs;
        
        void Awake()
        {
            _inputs = new PlayerInputs();

            instance = this;
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

        public void StartDrunk()
        {
            _mainCamera.GetComponent<CameraShake>().Shake(_drunkDuration, _drunkMagnitude);
        }

        private void Update()
        {
            _scoreText.text = Score.ToString();
        }
    }
}