using System;
using System.Collections;
using System.Collections.Generic;
using Item.Interfaces;
using MyPrint;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Item
{
    public class CocotteDeEliott : MonoBehaviour, IInteractable
    {
        private bool _isAllumer = false;
        
        [SerializeField] private GameObject _marmiteParticles;
        
        [Header("End Cuisson")]
        [SerializeField] private float _throwForce;
        [SerializeField] private float _angle = 10;
        [SerializeField] private Transform _posStartDrop;

        [Header("Timer")] 
        [SerializeField] private float _maxCuissonTime = 3f;
        [SerializeField, Range(0, 1)] private float _percentIsCuit = 0.8f;
        private float _elapsedTimeCuisson = 0;

        [Header("View")] 
        [SerializeField] private Image _percentIsCuitImage;
        [SerializeField] private Image _timerImage;
        
        Coroutine _coroutineCuisson = null;

        public Moule _mouleDansLaMarmite = null;

        private void Start()
        {
            _percentIsCuitImage.fillAmount = _percentIsCuit;
        }

        public void StartInteract()
        {
            if(!_isAllumer)
                return;

            EndCuisson();
        }
        
        public void StopInteract() { }

        void StartHeating()
        {
            _isAllumer = true;
            _marmiteParticles.SetActive(true);
            
            _elapsedTimeCuisson = 0;
            
            if(_coroutineCuisson == null)
                _coroutineCuisson = StartCoroutine(CuissonRoutine());
        }

        private void EndCuisson()
        {
            _isAllumer = false;
            _marmiteParticles.SetActive(false);

            if (_coroutineCuisson != null)
            {
                StopCoroutine(_coroutineCuisson);
                _coroutineCuisson = null;
            }
            
            SetNewCuissonState();
            
            if (_mouleDansLaMarmite != null)
            {
                _mouleDansLaMarmite.transform.position = _posStartDrop.position;
                
                Rigidbody rb = _mouleDansLaMarmite.GetComponent<Rigidbody>();
                
                float direction = Random.Range(0, 2) == 0 ? -1f : 1f;

                Vector3 force = new Vector3(direction * _angle, _throwForce, 0f);
                
                rb.AddForce(force, ForceMode.Impulse);
                rb.AddTorque(Vector3.forward * 50, ForceMode.Impulse);
            }
        }

        private void SetNewCuissonState()
        {
            if (!_mouleDansLaMarmite) return;
            
            if (_elapsedTimeCuisson < _maxCuissonTime * _percentIsCuit)
            {
                _mouleDansLaMarmite.SetNewCuissonState(AlimentCuisson.Crut);
            }else if (_elapsedTimeCuisson > _maxCuissonTime * _percentIsCuit && _elapsedTimeCuisson < _maxCuissonTime)
            {
                _mouleDansLaMarmite.SetNewCuissonState(AlimentCuisson.Cuit);
            }
            else if(_elapsedTimeCuisson >= _maxCuissonTime)
            {
                _mouleDansLaMarmite.SetNewCuissonState(AlimentCuisson.Brulé);
            }
            
            ABPrint.Print("New cuisson : " + _mouleDansLaMarmite.Cuisson + " / " + _elapsedTimeCuisson);
        }

        private IEnumerator CuissonRoutine()
        {
            while (_elapsedTimeCuisson <= _maxCuissonTime)
            {
                _elapsedTimeCuisson += Time.deltaTime;
                
                yield return null;
            }
            
            yield return new WaitForSeconds(0.2f);

            _coroutineCuisson = null;
            
            EndCuisson();
        }

        private void Update()
        {
            _timerImage.transform.parent.gameObject.SetActive(_coroutineCuisson != null);
            _timerImage.fillAmount = _elapsedTimeCuisson / _maxCuissonTime;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Moule moule))
            {
                if (moule.Cuisson == AlimentCuisson.Brulé || moule.Cuisson == AlimentCuisson.PasCoupé || moule.Cuisson == AlimentCuisson.Brulé)
                {
                    _elapsedTimeCuisson = _maxCuissonTime;
                    EndCuisson();
                    return;
                }

                _mouleDansLaMarmite = moule;
                StartHeating();
            }
        }
    }
}