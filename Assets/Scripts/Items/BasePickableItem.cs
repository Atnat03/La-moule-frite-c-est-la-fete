using System;
using MyPrint;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(Rigidbody))]
    public class BasePickableItem : MonoBehaviour, IInteractable
    {
        [Header("When pickup")]
        [SerializeField] private float _lerpSeep;
        [SerializeField] private float _followSpeed = 15f;
        [SerializeField] private float _distanceToCamera = 3f;
        private bool _isInHand = false;

        private Vector3 _targetPosition;
        
        Rigidbody _rigidbody;
        Camera _camera;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;
        }

        public void StartInteract()
        {
            ABPrint.Print("Starting interact");
            _isInHand = true;
            _rigidbody.useGravity = false;
        }

        public void StopInteract()
        {
            ABPrint.Print("Stopping interact");
            _isInHand = false;
            _rigidbody.useGravity = true;
        }

        private void Update()
        {
            if (!_isInHand || !_camera)
                return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            _targetPosition = ray.GetPoint(_distanceToCamera);
        }
        
        private void FixedUpdate()
        {
            if (!_isInHand || !_rigidbody)
                return;

            Vector3 direction = _targetPosition - _rigidbody.position;
            direction.z = 0;

            _rigidbody.linearVelocity = direction * _followSpeed;
        }
    }
}