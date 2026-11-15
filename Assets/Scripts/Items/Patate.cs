using System;
using Item.Interfaces;
using UnityEngine;

namespace Item
{
    public class Patate : BasePickableItem, IAliments
    {
        [SerializeField] private GameObject _normalMesh;
        [SerializeField] private GameObject _cutMesh;

        private void Start()
        {
            _normalMesh.SetActive(true);
            _cutMesh.SetActive(false);

            SetNewCuissonState(AlimentCuisson.PasCoupé);
        }

        public void CutCutPatate()
        {
            _normalMesh.SetActive(false);
            _cutMesh.SetActive(true);
        }

        public AlimentCuisson Cuisson {get; set;}
        
        public void SetNewCuissonState(AlimentCuisson cuisson)
        {
            Cuisson = cuisson;
        }
    }
}