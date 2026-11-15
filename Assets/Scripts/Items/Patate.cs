using System;
using Data;
using Item.Interfaces;
using MyPrint;
using System.Collections;
using UnityEngine;

namespace Item
{
    public class Patate : BasePickableItem, IAliments, IFriable
    {
        [SerializeField] private Transform _rayStart;

        [SerializeField] private GameObject _normalMesh;
        [SerializeField] private GameObject _cutMesh;

        [SerializeField] private float cookingTimeToBurn = 5;
        [SerializeField] private float cookingTime = 3;

        private CookingState cookingState = CookingState.raw;
        private CutState cutState = CutState.patate;

        public enum CookingState
        {
            raw,
            cooked,
            burned
        }

        public enum CutState
        {
            patate,
            frite
        }

        public static event Action OnCooked;

        private void Start()
        {
            SetNewCuissonState(AlimentCuisson.PasCoupé);
            cutState = CutState.frite;
        }

        public void CutCutPatate()
        {
            SetNewCuissonState(AlimentCuisson.Crut);
        }

        public AlimentCuisson Cuisson { get; set; }
        [SerializeField] private GameObject[] _states;

        public void SetNewCuissonState(AlimentCuisson cuisson)
        {
            Cuisson = cuisson;

            for (int i = 0; i < _states.Length; i++)
            {
                _states[i].SetActive(i == (int) Cuisson);
            }
        }

        [ContextMenu("Cuit")]
        public void Cuit() =>
            SetNewCuissonState(AlimentCuisson.Cuit);

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

        public void Frying(CookingTimerBar cookingTimerBar)
            {
                if (cutState == CutState.frite && cookingState != CookingState.burned)
                {
                    StartCoroutine(TimerPatate(cookingTimerBar));
                    cookingTimerBar.gameObject.SetActive(true);
                    cookingTimerBar.InitializeTimerBar(cookingTimeToBurn - cookingTime, cookingTime, cookingTimeToBurn);
                }
                else
                    Burned();
            }

        IEnumerator TimerPatate(CookingTimerBar cookingTimerBar)
        {
            bool notBurned = true;
            float timeElasped = 0;
            while (notBurned)
            {
                timeElasped += Time.deltaTime;
                cookingTimerBar.HandleTimer(timeElasped, cookingTimeToBurn - cookingTime, cookingTime);

                if (timeElasped >= cookingTimeToBurn)
                    notBurned = false;
                yield return null;
            }

            cookingTimerBar.gameObject.SetActive(false);
            cookingState = CookingState.cooked;
            OnCooked?.Invoke();
        }

        private void Burned()
        {
            cookingState = CookingState.burned;
            OnCooked?.Invoke();
        }
    }
}   