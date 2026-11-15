using Client;
using Data;
using MyPrint;
using UnityEngine;

namespace Item
{
    public class Barquette : BasePickableItem
    { 
        public Commande Command { get; set; }
        
        private bool _isOnLanceur;

        private Vector3 _startPosition;
        private Vector3 _direction;

        [SerializeField] private Commande _currentCommand;

        void Start()
        {
            _startPosition = transform.position - transform.forward;
            _direction = transform.forward;
        }
        
        public override void StopInteract()
        {
            base.StopInteract();
            
            if (!_isOnLanceur)
            {
                _startPosition = transform.position;
                _direction = transform.forward;
                
                Debug.DrawLine(_startPosition, _direction.normalized * 5f, Color.red);
                
                if (Physics.Raycast(_startPosition, _direction, out RaycastHit hit))
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
        
        public void AddProduitToCommande(Produit type) => _currentCommand.produitType = type;
        public void AddSauceFriteToCommande(SauceFrite type) => _currentCommand.sauceFrite = type;
        public void AddSauceMouleToCommande(SauceMoule type) => _currentCommand.sauceMoule = type;

        public void OnCollisionEnter(Collision c)
        {
            if (c.collider.TryGetComponent(out ClientCommand client))
            {
                float addScore = client.CheckIfItsTheGoodCommand(Command);
                ABPrint.Print("+ number points : " + addScore);
            }
        }
        
    }
}