using System;
using System.Collections;
using MyPrint;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Item
{
    public class LanceBarquette : MonoBehaviour, IInteractable
    {
        [Header("Camera Lerp")] [SerializeField]
        private Transform _camAimPosition;

        [SerializeField] private float _durationToTravel = 0.25f;

        [Header("Barquette In Lanceur")] 
        [SerializeField]
        private Transform _barquettePosition;

        Transform _currentBarquetteInTheLanceur;

        [Header("Throw")] 
        [SerializeField] private float _maxForce = 50;
        [SerializeField] private float _forceGainPerSecond = 10f;
        [SerializeField] private float _forceLoosePerSecond = 40f;
        [SerializeField] private float _maxFOVThrow = 90;
        private float _currentForce = 0;
        private bool _isAiming = false;
        private bool _isInFrontLancer = false;
        [SerializeField] private Image _targetImage;
        [SerializeField] private Animator _animator;
        
        private Camera _camera;
        private Vector3 _baseCameraPosition;
        private float _baseCameraFOV;

        PlayerInputs _playerInputs;
        Coroutine _switchCameraCoroutine;

        private void Start()
        {
            _camera = Camera.main;

            if (_camera != null)
            {
                _baseCameraPosition = _camera.transform.position;
                _baseCameraFOV = _camera.fieldOfView;
            }
        }

        void OnEnable()
        {
            _playerInputs = new PlayerInputs();

            _playerInputs.Enable();

            _playerInputs.Gameplay.RightClick.performed += AddForceThrow;
            _playerInputs.Gameplay.RightClick.canceled += ResetForceThrow;
        }

        private void OnDisable()
        {
            _playerInputs.Gameplay.RightClick.performed -= AddForceThrow;
            _playerInputs.Gameplay.RightClick.canceled -= ResetForceThrow;

            _playerInputs.Disable();
        }

        public void StartInteract()
        {
            if (_switchCameraCoroutine != null)
            {
                StopCoroutine(_switchCameraCoroutine);
            }

            _isInFrontLancer = true;
            
            _switchCameraCoroutine = StartCoroutine(PerformSwitch(_durationToTravel, _baseCameraPosition, _camAimPosition.position));
        }

        public void StopInteract()
        {
            if (_switchCameraCoroutine != null)
            {
                StopCoroutine(_switchCameraCoroutine);
            }

            _isInFrontLancer = false;
            
            _switchCameraCoroutine = StartCoroutine(PerformSwitch(_durationToTravel, _camAimPosition.position, _baseCameraPosition));
        }

        IEnumerator PerformSwitch(float duration, Vector3 startPos, Vector3 endPos)
        {
            if (_camera == null)
                yield break;

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                _camera.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);

                yield return null;
            }

            _switchCameraCoroutine = null;
        }

        private void AddForceThrow(InputAction.CallbackContext obj)
        {
            if(!_isInFrontLancer)
                return;
            
            if(_currentBarquetteInTheLanceur == null)
                return;
            
            _isAiming = true;
        }
        
        private void ResetForceThrow(InputAction.CallbackContext obj)
        {
            if(!_isInFrontLancer)
                return;
            
            if(_currentBarquetteInTheLanceur == null)
                return;

            _animator.SetFloat("Throw", 0);
            
            _isAiming = false;
            
            Rigidbody rb = _currentBarquetteInTheLanceur.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                
                Vector3 throwDirection = (_camera.ScreenPointToRay(Input.mousePosition).direction).normalized;
                
                rb.AddForce(throwDirection * _currentForce, ForceMode.Impulse);
            }
            
            _camera.fieldOfView = _baseCameraFOV;
            
            _currentBarquetteInTheLanceur = null;
        }
        
        private void Update()
        {
            if (_isAiming)
            {
                _currentForce += Time.deltaTime * _forceGainPerSecond;
                
                _targetImage.gameObject.SetActive(true);
                _targetImage.rectTransform.position = Input.mousePosition;
                
                _animator.SetFloat("Throw", _currentForce / _maxForce);
            }
            else
            {
                _targetImage.gameObject.SetActive(false);
                _currentForce -= Time.deltaTime * _forceLoosePerSecond;
            }
            
            _currentForce = Mathf.Clamp(_currentForce, 0, _maxForce);
        }

        private void LateUpdate()
        {
            if(_currentBarquetteInTheLanceur != null && _isAiming)
                _camera.fieldOfView = Mathf.Lerp(_baseCameraFOV, _maxFOVThrow, _currentForce / _maxForce);
        }

        public (Vector3, Quaternion) SetTargetOnTheLanceur(Transform target)
        {
            _currentBarquetteInTheLanceur = target;
            target.parent = _barquettePosition;
            return (_barquettePosition.position, _barquettePosition.rotation);
        }
    }
}