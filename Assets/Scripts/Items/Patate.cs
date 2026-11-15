using System;
using Data;
using Item.Interfaces;
using MyPrint;
using UnityEngine;

namespace Item
{
    public class Patate : BasePickableItem, IAliments
    {
        [SerializeField] private Transform _rayStart;
        
        private void Start()
        {
            SetNewCuissonState(AlimentCuisson.PasCoupé);
        }

        public void CutCutPatate()
        {
            SetNewCuissonState(AlimentCuisson.Crut);
        }

        public AlimentCuisson Cuisson {get; set;}
        [SerializeField] private GameObject[] _states;
        
        public void SetNewCuissonState(AlimentCuisson cuisson)
        {
            Cuisson = cuisson;
            
            for (int i = 0; i < _states.Length; i++)
            {
                _states[i].SetActive(i == (int)Cuisson);
            }
        }

        [ContextMenu("Cuit")]
        public void Cuit() => SetNewCuissonState(AlimentCuisson.Cuit);

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
                    b.AddProduitToCommande(Produit.Frite);
                    Destroy(gameObject);
                }
            }
        }
    }
}