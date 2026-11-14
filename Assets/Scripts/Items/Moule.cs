using System;
using Item.Interfaces;
using UnityEngine;

namespace Item
{
    public class Moule : BasePickableItem,IAliments
    {
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

    }
}