using System;
using MyPrint;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(Rigidbody))]
    public class BasePickableItem : MonoBehaviour, IInteractable
    {
        [Header("When pickup")]
        [SerializeField] private float _followSpeed = 15f;
        [SerializeField] private float _distanceToCamera = 3f;
        protected bool _isInHand = false;
        float _fixedZ;

        private Vector3 _targetPosition;
        
        protected Rigidbody _rigidbody;
        protected Camera _camera;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;
            
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationY |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezePositionZ;
        }

        public virtual void StartInteract()
        {
            _isInHand = true;
            _rigidbody.useGravity = false;
            _fixedZ = _rigidbody.position.z;
        }

        public virtual void StopInteract()
        {
            _isInHand = false;
            _rigidbody.useGravity = true;
        }

        protected virtual void Update()
        {
            if (!_isInHand || !_camera)
                return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, _fixedZ));

            if (plane.Raycast(ray, out float distance))
            {
                _targetPosition = ray.GetPoint(distance);
            }
        }
        
        private void FixedUpdate()
        {
            if (!_isInHand || !_rigidbody)
                return;

            Vector3 direction = _targetPosition - _rigidbody.position;

            direction.z = 0;

            _rigidbody.linearVelocity = direction * _followSpeed;
        }

        public void RecentreItem()
        {
            _isInHand = false;
            
            transform.position = Player.Player.instance.CenterPos.position;
            transform.rotation = Player.Player.instance.CenterPos.rotation;
            
            _rigidbody.useGravity = true;
        }
    }
}