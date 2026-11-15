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

        [SerializeField] private GameObject _friteMesh;
        [SerializeField] private GameObject _mouleMesh;

        void Start()
        {
            Command = new Commande();
            
            _startPosition = transform.position - transform.forward;
            _direction = transform.forward;
            
            _friteMesh.SetActive(false);
            _mouleMesh.SetActive(false);
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
                        var data = lanceBarquette.SetTargetOnTheLanceur(transform);
                        transform.position = data.Item1;
                        transform.rotation = data.Item2;
                        _rigidbody.isKinematic = true;
                        _isOnLanceur = true;
                    }
                }
            }
        }

        public void AddProduitToCommande(Produit type)
        {
            Command.produitType = type;
            if (type == Produit.Frite)
            {
                if (_mouleMesh.activeSelf)
                {
                    Command.produitType = Produit.FriteMoule;
                }
                _friteMesh.SetActive(true);
            }
            if (type == Produit.Moule)
            {
                if (_friteMesh.activeSelf)
                {
                    Command.produitType = Produit.FriteMoule;
                }
                _mouleMesh.SetActive(true);
            }
        }
        public void AddSauceFriteToCommande(SauceFrite type) => Command.sauceFrite = type;
        public void AddSauceMouleToCommande(SauceMoule type) => Command.sauceMoule = type;

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