using System;
using Data;
using Item.Interfaces;
using UnityEngine;

namespace Item
{
    public class Moule : BasePickableItem,IAliments
    {
        [SerializeField] private Transform _rayStart;
        
        [SerializeField] private MeshRenderer _meshRenderers;
        [SerializeField] private Material[] _materials;
        
        public AlimentCuisson Cuisson { get; set; }
        
        private void Start()
        {
            SetNewCuissonState(AlimentCuisson.Crut);
        }
        
        public void SetNewCuissonState(AlimentCuisson cuisson)
        {
            Cuisson = cuisson;

            _meshRenderers.material = _materials[(int)cuisson];
        }

        public override void StopInteract()
        {
            base.StopInteract();
            
            Debug.DrawLine(_rayStart.position, _rayStart.forward.normalized * 5f, Color.red);

            if (Cuisson != AlimentCuisson.Cuit)
                return;
                
            if (Physics.Raycast(_rayStart.position, _rayStart.forward.normalized * 5f, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out Barquette b))
                {
                    b.AddProduitToCommande(Produit.Moule);
                    Destroy(gameObject);
                }
            }
        }
    }
}