using UnityEngine;

namespace Item
{
    public class Barquette : BasePickableItem
    { 
        private bool _isOnLanceur;

        private Vector3 startPosition;
        private Vector3 direction;

        void Start()
        {
            startPosition = transform.position - transform.forward;
            direction = transform.forward;
        }
        
        public override void StopInteract()
        {
            base.StopInteract();
            
            if (!_isOnLanceur)
            {
                startPosition = transform.position;
                direction = transform.forward;
                
                Debug.DrawLine(startPosition, direction.normalized * 5f, Color.red);
                
                if (Physics.Raycast(startPosition, direction, out RaycastHit hit))
                {
                    if (hit.transform.TryGetComponent(out LanceBarquette lanceBarquette))
                    {
                        transform.position = lanceBarquette.SetTargetOnTheLanceur(transform);
                        _rigidbody.isKinematic = true;
                        _isOnLanceur = true;
                    }
                }
            }
        }
    }
}