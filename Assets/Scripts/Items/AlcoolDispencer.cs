using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Item
{
    public class AlcoolDispencer : BasePickableItem
    {
        [SerializeField] private Transform _maxYValueToTrigger;
        [SerializeField] private Transform _dropPos;
        [SerializeField] private BouteilleAlcool _alcoolBottlePrefab;
        
        [Header("Rope")]
        [SerializeField] private Transform _startRope;
        [SerializeField] private Transform _endRope;
        [SerializeField] private LineRenderer _lineRenderer;
        
        private float _bufferDropAlcool = 1f;
        private bool _canDropAlcool = true;

        private void Start()
        {
            _lineRenderer.SetPosition(0,_startRope.localPosition);
        }

        protected override void Update()
        {
            base.Update();

            _lineRenderer.SetPosition(1,_endRope.localPosition);
            
            if (_canDropAlcool)
            {
                if (transform.position.y < _maxYValueToTrigger.position.y)
                {
                    _canDropAlcool = false;
                    StartCoroutine(PerformDropBottle());
                }
            }
        }

        IEnumerator PerformDropBottle()
        {
            BouteilleAlcool b = Instantiate(_alcoolBottlePrefab, _dropPos.position, Quaternion.identity);
            
            yield return new WaitForSeconds(_bufferDropAlcool);
            
            _canDropAlcool = true;
        }
    }
}