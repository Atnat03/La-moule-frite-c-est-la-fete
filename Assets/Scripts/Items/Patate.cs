using System;
using UnityEngine;

namespace Item
{
    public class Patate : BasePickableItem
    {
        [SerializeField] private GameObject _normalMesh;
        [SerializeField] private GameObject _cutMesh;

        private void Start()
        {
            _normalMesh.SetActive(true);
            _cutMesh.SetActive(false);
        }

        public void CutCutPatate()
        {
            _normalMesh.SetActive(false);
            _cutMesh.SetActive(true);
        }
    }
}