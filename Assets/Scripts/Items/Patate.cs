using System;
using System.Collections;
using UnityEngine;

namespace Item
{
    public class Patate : BasePickableItem, IFriable
    {
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
            _normalMesh.SetActive(true);
            _cutMesh.SetActive(false);
        }

        public void CutCutPatate()
        {
            _normalMesh.SetActive(false);
            _cutMesh.SetActive(true);
            cutState = CutState.frite;
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